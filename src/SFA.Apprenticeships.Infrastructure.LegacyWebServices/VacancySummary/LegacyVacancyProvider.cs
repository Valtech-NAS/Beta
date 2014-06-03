namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.VacancySummary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Vacancy;
    using CuttingEdge.Conditions;
    using Domain.Entities.Vacancy;
    using Domain.Interfaces.Services.Mapping;
    using Common.Wcf;
    using Configuration;
    using VacancySummaryProxy;

    public class LegacyVacancyProvider : IVacancyProvider
    {
        private readonly IWcfService<IVacancySummary> _service;
        private readonly ILegacyServicesConfiguration _legacyServicesConfiguration;
        private readonly IMapper _mapper;

        public LegacyVacancyProvider(
            ILegacyServicesConfiguration legacyServicesConfiguration,
            IWcfService<IVacancySummary> service,
            IMapper mapper)
        {
            Condition.Requires(legacyServicesConfiguration, "legacyServicesConfiguration").IsNotNull();
            Condition.Requires(service, "service").IsNotNull();
            Condition.Requires(mapper, "mapper").IsNotNull();

            _legacyServicesConfiguration = legacyServicesConfiguration;
            _service = service;
            _mapper = mapper;
        }

        public int GetVacancyPageCount(VacancyLocationType vacancyLocationType)
        {
            var vacancySummaryRequest = new VacancySummaryRequest
            {
                ExternalSystemId = _legacyServicesConfiguration.SystemId,
                PublicKey = _legacyServicesConfiguration.PublicKey,
                MessageId = Guid.NewGuid(),
                VacancySearchCriteria = new VacancySearchData
                {
                    PageIndex = 1,
                    VacancyLocationType =  vacancyLocationType.ToString()
                }
            };

            var rs = default(VacancySummaryResponse);
            _service.Use(client => rs = client.Get(vacancySummaryRequest));

            if (rs == null || rs.ResponseData == null)
            {
                return 0;
            }

            return rs.ResponseData.TotalPages;
        }

        public IEnumerable<VacancySummary> GetVacancySummary(VacancyLocationType vacancyLocationType, int page = 1)
        {
            var vacancySummaryRequest = new VacancySummaryRequest
            {
                ExternalSystemId = _legacyServicesConfiguration.SystemId,
                PublicKey = _legacyServicesConfiguration.PublicKey,
                MessageId = Guid.NewGuid(),
                VacancySearchCriteria = new VacancySearchData
                {
                    PageIndex = page,
                    VacancyLocationType = vacancyLocationType.ToString()
                }
            };

            var rs = default(VacancySummaryResponse);
            _service.Use(client => rs = client.Get(vacancySummaryRequest));

            if (rs == null || 
                rs.ResponseData == null || 
                rs.ResponseData.SearchResults == null ||
                rs.ResponseData.SearchResults.Length == 0)
            {
                return Enumerable.Empty<VacancySummary>().ToList();
            }

           return _mapper.Map<VacancySummaryData[], IEnumerable<VacancySummary>>(rs.ResponseData.SearchResults);
        }
    }
}
