namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.VacancySummary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.VacancyEtl;
    using Configuration;
    using NLog;
    using VacancySummaryProxy;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Mapping;
    using Wcf;

    public class LegacyVacancyIndexDataProvider : IVacancyIndexDataProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IWcfService<IVacancySummary> _service;
        private readonly ILegacyServicesConfiguration _legacyServicesConfiguration;
        private readonly IMapper _mapper;

        public LegacyVacancyIndexDataProvider(ILegacyServicesConfiguration legacyServicesConfiguration, IWcfService<IVacancySummary> service, IMapper mapper)
        {
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
                    VacancyLocationType = vacancyLocationType.ToString()
                }
            };

            Logger.Info("Calling Legacy webservice for VacancyPageCount MessageId={0}", vacancySummaryRequest.MessageId);

            var rs = default(VacancySummaryResponse);
            _service.Use(client => rs = client.Get(vacancySummaryRequest));

            if (rs == null || rs.ResponseData == null)
            {
                Logger.Info("No pages returned from Legacy webservice for VacancyPageCount MessageId={0}", vacancySummaryRequest.MessageId);
                return 0;
            }

            Logger.Info("{0} pages returned from Legacy webservice for VacancyPageCount MessageId={1}", rs.ResponseData.TotalPages, vacancySummaryRequest.MessageId);

            return rs.ResponseData.TotalPages;
        }

        public IEnumerable<VacancySummary> GetVacancySummaries(VacancyLocationType vacancyLocationType, int page = 1)
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

            Logger.Info("Calling Legacy webservice for VacancySummaries MessageId={0}", vacancySummaryRequest.MessageId);

            var rs = default(VacancySummaryResponse);
            _service.Use(client => rs = client.Get(vacancySummaryRequest));

            if (rs == null ||
                rs.ResponseData == null ||
                rs.ResponseData.SearchResults == null ||
                rs.ResponseData.SearchResults.Length == 0)
            {
                Logger.Info("No results returned from Legacy webservice for VacancySummaries");
                return Enumerable.Empty<VacancySummary>();
            }

            Logger.Info("{0} results returned from Legacy webservice for VacancySummaries", rs.ResponseData.SearchResults.Count());

            return _mapper.Map<VacancySummaryData[], IEnumerable<VacancySummary>>(rs.ResponseData.SearchResults);
        }
    }
}
