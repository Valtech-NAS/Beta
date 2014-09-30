namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.VacancySummary
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.VacancyEtl;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Mapping;
    using GatewayServiceProxy;
    using NLog;
    using Wcf;
    using ErrorCodes = Application.VacancyEtl.ErrorCodes;

    public class GatewayVacancyIndexDataProvider : IVacancyIndexDataProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IWcfService<GatewayServiceContract> _service;
        private readonly IMapper _mapper;

        public GatewayVacancyIndexDataProvider(
            IWcfService<GatewayServiceContract> service,
            IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public int GetVacancyPageCount()
        {
            var request = new GetVacancySummaryRequest { PageNumber = 1 };

            Logger.Debug("Calling Gateway GetVacancySummaries webservice for vacancy index page count");

            var response = default(GetVacancySummaryResponse);

            //todo: remove endpoint config name once all new service operations integrated
            _service.Use("SecureService", client => response = client.GetVacancySummaries(request));

            if (response == null)
            {
                Logger.Error("Gateway GetVacancySummaries did not respond");

                throw new CustomException(
                    "Gateway GetVacancySummaries failed to retrieve page count from legacy system.",
                    ErrorCodes.GatewayServiceFailed);
            }

            return response.TotalPages;
        }

        public IEnumerable<Domain.Entities.Vacancies.VacancySummary> GetVacancySummaries(int page)
        {
            var request = new GetVacancySummaryRequest { PageNumber = page };

            Logger.Debug("Calling Gateway GetVacancySummaries webservice for vacancy data page {0}", page);

            var response = default(GetVacancySummaryResponse);

            //todo: remove endpoint config name once all new service operations integrated
            _service.Use("SecureService", client => response = client.GetVacancySummaries(request));

            if (response == null)
            {
                Logger.Error("Gateway GetVacancySummaries did not respond");

                throw new CustomException(
                    "Gateway GetVacancySummaries failed to retrieve page '" + page + "' from legacy system.",
                    ErrorCodes.GatewayServiceFailed);
            }

            var results = _mapper.Map<GatewayServiceProxy.VacancySummary[], IEnumerable<Domain.Entities.Vacancies.VacancySummary>>(
                response.VacancySummaries);

            return results.Where(r => r.VacancyType == VacancyType.Apprenticeship);
        }
    }
}
