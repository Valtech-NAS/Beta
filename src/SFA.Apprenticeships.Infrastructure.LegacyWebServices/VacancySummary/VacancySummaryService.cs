namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.VacancySummary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SFA.Apprenticeships.Application.Interfaces.Vacancy;
    using SFA.Apprenticeships.Domain.Entities.Vacancy;
    using SFA.Apprenticeships.Infrastructure.Common.Mappers;
    using SFA.Apprenticeships.Infrastructure.Common.Wcf;
    using SFA.Apprenticeships.Infrastructure.LegacyWebServices.Configuration;
    using SFA.Apprenticeships.Infrastructure.LegacyWebServices.VacancySummaryProxy;

    public class VacancySummaryService : IVacancySummaryService
    {
        private readonly IWcfService<IVacancySummary> _service;
        private readonly ILegacyServicesConfiguration _legacyServicesConfiguration;
        private readonly IMapper _mapper;

        public VacancySummaryService(
            ILegacyServicesConfiguration legacyServicesConfiguration,
            IWcfService<IVacancySummary> service,
            IMapper mapper)
        {
            if (legacyServicesConfiguration == null)
            {
                throw new ArgumentNullException("legacyServicesConfiguration");
            }

            if (service == null)
            {
                throw new ArgumentNullException("service");
            }

            if (mapper == null)
            {
                throw new ArgumentNullException("mapper");
            }

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
                VacancySearchCriteria = new VacancySearchData()
                {
                    PageIndex = 1,
                    //VacancyLocationType =  vacancyLocationType.ToString()
                    VacancyLocationType = (VacancyDetailsSearchLocationType)vacancyLocationType
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
                VacancySearchCriteria = new VacancySearchData()
                {
                    PageIndex = page,
                    //VacancyLocationType = vacancyLocationType.ToString()
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
                return Enumerable.Empty<VacancySummary>().ToList();
            }

           return _mapper.Map<VacancySummaryData[], IEnumerable<VacancySummary>>(rs.ResponseData.SearchResults);
        }
    }
}
