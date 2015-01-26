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

            var response = default(GetVacancySummaryResponse);

            Logger.Debug("Calling Legyacy.GetVacancySummaries for page count");

            _service.Use("SecureService", client => response = client.GetVacancySummaries(request));

            if (response == null)
            {
                Logger.Error("Legacy.GetVacancySummaries for page count did not respond");
                throw new CustomException("Failed to retrieve application status pages from Legacy.GetVacancySummaries", ErrorCodes.GatewayServiceFailed);
            }

            Logger.Info("Vacancy summary page count retrieved from Legacy.GetApplicationsStatus ({0})", response.TotalPages);

            return response.TotalPages;
        }

        public VacancySummaries GetVacancySummaries(int page)
        {
            var request = new GetVacancySummaryRequest {PageNumber = page};

            var response = default(GetVacancySummaryResponse);

            Logger.Debug("Calling Legacy.GetVacancySummaries for page {0}", page);

            _service.Use("SecureService", client => response = client.GetVacancySummaries(request));

            if (response == null)
            {
                Logger.Error("Legacy.GetVacancySummaries did not respond");

                throw new CustomException("Failed to retrieve page '" + page + "' from Legacy.GetVacancySummaries",
                    ErrorCodes.GatewayServiceFailed);
            }

            var apprenticeshipTypes = new[]
            {
                "IntermediateLevelApprenticeship", "AdvancedLevelApprenticeship", "HigherApprenticeship"
            };

            var apprenticeshipSummaries = _mapper.Map<VacancySummary[], IEnumerable<ApprenticeshipSummary>>(
                response.VacancySummaries.Where(v => apprenticeshipTypes.Contains(v.VacancyType)).ToArray());

            var traineeshipsSummaries = _mapper.Map<VacancySummary[], IEnumerable<TraineeshipSummary>>(
                response.VacancySummaries.Where(v => v.VacancyType == "Traineeship").ToArray());

            Logger.Debug("Vacancy summaries (page {0}) were successfully retrieved from Legacy.GetVacancySummaries ({1})",
                page, response.VacancySummaries.Count());

            return new VacancySummaries(apprenticeshipSummaries, traineeshipsSummaries);
        }
    }
}
