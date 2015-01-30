namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.ReferenceData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.ReferenceData;
    using Configuration;
    using Domain.Entities.ReferenceData;
    using LegacyReferenceDataProxy;
    using NLog;
    using Wcf;

    public class ReferenceDataProvider : IReferenceDataProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IWcfService<IReferenceData> _service;
        private readonly ILegacyServicesConfiguration _legacyServicesConfiguration;
        private readonly string[] _blacklistedCategoryCodes;

        public ReferenceDataProvider(IWcfService<IReferenceData> service, ILegacyServicesConfiguration legacyServicesConfiguration)
        {
            _service = service;
            _legacyServicesConfiguration = legacyServicesConfiguration;
            _blacklistedCategoryCodes = _legacyServicesConfiguration.BlacklistedCategoryCodes.Split(',');
        }

        public IEnumerable<Category> GetCategories()
        {
            GetApprenticeshipFrameworksResponse response = null;

            var request = new GetApprenticeshipFrameworksRequest(_legacyServicesConfiguration.SystemId, Guid.NewGuid(), _legacyServicesConfiguration.PublicKey);
            _service.Use("ReferenceData", client => response = client.GetApprenticeshipFrameworks(request));

            if (response == null || response.ApprenticeshipFrameworks == null || response.ApprenticeshipFrameworks.Length == 0)
            {
                Logger.Warn("No ApprenticeshipFrameworks data returned from the legacy GetApprenticeshipFrameworks service");
                return null;
            }

            var categories = new List<Category>();

            var topLevelCategories =
                response.ApprenticeshipFrameworks
                .Where(f => !_blacklistedCategoryCodes.Contains(f.ApprenticeshipOccupationCodeName))
                .Select(c =>
                        new Category()
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
                        new Category()
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
