namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.VacancySummary
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.VacancyEtl;
    using Application.VacancyEtl.Entities;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.Traineeships;
    using Domain.Interfaces.Mapping;
    using GatewayServiceProxy;
    using NLog;
    using Wcf;
    using ErrorCodes = Application.VacancyEtl.ErrorCodes;

    public class LegacyVacancyIndexDataProvider : IVacancyIndexDataProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IWcfService<GatewayServiceContract> _service;
        private readonly IMapper _mapper;

        public LegacyVacancyIndexDataProvider(
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

        public VacancySummaries GetVacancySummaries(int page)
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

            var apprenticeshipTypes = new[] {"IntermediateLevelApprenticeship", "AdvancedLevelApprenticeship", "HigherApprenticeship"};

            var apprenticeshipSummaries = _mapper.Map<GatewayServiceProxy.VacancySummary[], IEnumerable<ApprenticeshipSummary>>(
                                response.VacancySummaries.Where(v => apprenticeshipTypes.Contains(v.VacancyType)).ToArray());

            var traineeshipsSummaries = _mapper.Map<GatewayServiceProxy.VacancySummary[], IEnumerable<TraineeshipSummary>>(
                                response.VacancySummaries.Where(v => v.VacancyType == "Traineeship").ToArray());

            return new VacancySummaries(apprenticeshipSummaries, traineeshipsSummaries);
        }
    }
}
