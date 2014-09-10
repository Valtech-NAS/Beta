namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.VacancySummary
{
    using System.Collections.Generic;
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

        public int GetVacancyPageCount(VacancyLocationType vacancyLocationType)
        {
            // TODO: remove vacancyLocationType arg as results will contain all types.
            var request = new GetVacancySummaryRequest { PageNumber = 1 };

            Logger.Debug("Calling GetVacancySummaries webservice for vacancy index page count");

            var response = default(GetVacancySummaryResponse);

            _service.Use(client => response = client.GetVacancySummaries(request));

            if (response == null)
            {
                Logger.Error("Gateway GetVacancySummaries did not respond");

                throw new CustomException(
                    "Gateway GetVacancySummaries failed to retrieve page count from legacy system.",
                    ErrorCodes.GatewayServiceFailed);
            }

            return response.TotalPages;
        }

        public IEnumerable<Domain.Entities.Vacancies.VacancySummary> GetVacancySummaries(
            VacancyLocationType vacancyLocationType, int page)
        {
            // TODO: remove vacancyLocationType arg as results will contain all types.
            var request = new GetVacancySummaryRequest { PageNumber = page };

            Logger.Debug("Calling GetVacancySummaries webservice for vacancy data page {0}", page);

            var response = default(GetVacancySummaryResponse);

            _service.Use(client => response = client.GetVacancySummaries(request));

            if (response == null)
            {
                Logger.Error("Gateway GetVacancySummaries did not respond");

                throw new CustomException(
                    "Gateway GetVacancySummaries failed to retrieve page '" + page + "' from legacy system.",
                    ErrorCodes.GatewayServiceFailed);
            }

            return _mapper.Map<GatewayServiceProxy.VacancySummary[], IEnumerable<Domain.Entities.Vacancies.VacancySummary>>(
                response.VacancySummaries);
        }
    }
}
