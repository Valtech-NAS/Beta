namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.ReferenceData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Logging;
    using Application.ReferenceData;
    using Configuration;
    using Domain.Entities.Exceptions;
    using Domain.Entities.ReferenceData;
    using LegacyReferenceDataProxy;
    using Wcf;

    public class ReferenceDataProvider : IReferenceDataProvider
    {
        private readonly ILogService _logger;
        private readonly IWcfService<IReferenceData> _service;
        private readonly ILegacyServicesConfiguration _legacyServicesConfiguration;

        public ReferenceDataProvider(IWcfService<IReferenceData> service, ILegacyServicesConfiguration legacyServicesConfiguration, ILogService logger)
        {
            _service = service;
            _legacyServicesConfiguration = legacyServicesConfiguration;
            _logger = logger;
        }

        public IEnumerable<Category> GetCategories()
        {
            GetApprenticeshipFrameworksResponse response = null;

            var request = new GetApprenticeshipFrameworksRequest(_legacyServicesConfiguration.SystemId, Guid.NewGuid(), _legacyServicesConfiguration.PublicKey);

            try
            {
                _logger.Debug("Calling ReferenceData.GetApprenticeshipFrameworks");

                _service.Use("ReferenceData", client => response = client.GetApprenticeshipFrameworks(request));
                var categories = GetCategories(response);
                
                _logger.Debug("ReferenceData.GetApprenticeshipFrameworks succeeded");

                return categories;
            }
            catch (BoundaryException ex)
            {
                _logger.Warn(ex);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            // Must return null or could be put in cache.
            return null;
        }

        private IEnumerable<Category> GetCategories(GetApprenticeshipFrameworksResponse response)
        {
            if (response == null || response.ApprenticeshipFrameworks == null || response.ApprenticeshipFrameworks.Length == 0)
            {
                _logger.Warn("No ApprenticeshipFrameworks data returned from the legacy GetApprenticeshipFrameworks service");
                return null;
            }

            var categories = new List<Category>();

            var topLevelCategories =
                response.ApprenticeshipFrameworks
                .Select(c =>
                        new Category
                        {
                            CodeName = c.ApprenticeshipOccupationCodeName,
                            ShortName = c.ApprenticeshipOccupationShortName,
                            FullName = c.ApprenticeshipOccupationFullName
                        }).Distinct(new CategoryComparer()).OrderBy(c => c.FullName);

            foreach (var topLevelCategory in topLevelCategories)
            {
                topLevelCategory.SubCategories =
                    response.ApprenticeshipFrameworks.Where(c => c.ApprenticeshipOccupationCodeName == topLevelCategory.CodeName)
                    .Select(d =>
                        new Category
                        {
                            ParentCategory = topLevelCategory,
                            CodeName = d.ApprenticeshipFrameworkCodeName,
                            ShortName = d.ApprenticeshipFrameworkShortName,
                            FullName = d.ApprenticeshipFrameworkFullName
                        }).OrderBy(c => c.FullName).ToList();

                categories.Add(topLevelCategory);
            }

            return categories;
        }

        class CategoryComparer : IEqualityComparer<Category>
        {
            public bool Equals(Category x, Category y)
            {
                return x.CodeName == y.CodeName;
            }

            public int GetHashCode(Category obj)
            {
                return obj.CodeName.GetHashCode();
            }
        }
    }
}
