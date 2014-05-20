namespace SFA.Apprenticeships.Services.Legacy.Vacancy.Service
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using SFA.Apprenticeships.Common.Configuration.LegacyServices;
    using SFA.Apprenticeships.Common.Entities.Vacancy;
    using SFA.Apprenticeships.Common.Interfaces.Enums;
    using SFA.Apprenticeships.Common.Interfaces.Services;
    using SFA.Apprenticeships.Services.Legacy.Vacancy.Abstract;
    using SFA.Apprenticeships.Services.Legacy.Vacancy.Proxy;

    public class VacancySummaryService : IVacancySummaryService
    {

        private readonly IWcfService<IVacancySummary> _service;
        private readonly ILegacyServicesConfiguration _legacyServicesConfiguration;

        public VacancySummaryService(ILegacyServicesConfiguration legacyServicesConfiguration, IWcfService<IVacancySummary> service)
        {
            _legacyServicesConfiguration = legacyServicesConfiguration;
            _service = service;
        }

        public IEnumerable<VacancySummary> GetVacancySummary(VacancyLocationType vacancyLocationType, int page = 1)
        {
            var vacancySummaryRequest = new VacancySummaryRequest
            {
                ExternalSystemId = _legacyServicesConfiguration.SystemId,
                PublicKey = _legacyServicesConfiguration.PublicKey,
                MessageId = Guid.NewGuid(),
                VacancySearchCriteria = new VacancySearchData()
                {
                    PageIndex = page,
                    VacancyLocationType = (VacancyDetailsSearchLocationType)vacancyLocationType
                }
            };

            var rs = default(VacancySummaryResponse);
            _service.Use(client => rs = client.Get(vacancySummaryRequest));

            if (rs == null || 
                rs.ResponseData == null || 
                rs.ResponseData.SearchResults == null ||
                rs.ResponseData.SearchResults.Length == 0)
            {
                return Enumerable.Empty<VacancySummary>();
            }

            return null;
            //return rs.ResponseData.SearchResults.Select(vacancySummary =>
            //{
            //    new VacancySummary
            //    {
            //        A
            //    }
            //});
        }
    }
}
