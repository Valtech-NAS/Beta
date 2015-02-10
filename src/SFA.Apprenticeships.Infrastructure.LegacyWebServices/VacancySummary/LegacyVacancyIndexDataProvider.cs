namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.VacancySummary
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Logging;
    using Application.VacancyEtl;
    using Application.VacancyEtl.Entities;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.Traineeships;
    using Domain.Interfaces.Mapping;
    using GatewayServiceProxy;
    using Wcf;

    public class LegacyVacancyIndexDataProvider : IVacancyIndexDataProvider
    {
        private readonly ILogService _logger;
        private readonly IWcfService<GatewayServiceContract> _service;
        private readonly IMapper _mapper;

        public LegacyVacancyIndexDataProvider(
            IWcfService<GatewayServiceContract> service,
            IMapper mapper, ILogService logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        public int GetVacancyPageCount()
        {
            var request = new GetVacancySummaryRequest { PageNumber = 1 };

            var response = default(GetVacancySummaryResponse);

            _logger.Info("Calling Legacy.GetVacancySummaries for page count");

            _service.Use("SecureService", client => response = client.GetVacancySummaries(request));

            if (response == null)
            {
                var message = string.Format("Failed to retrieve page '{0}' from Legacy.GetVacancySummaries", request.PageNumber);

                _logger.Error(message);
                throw new CustomException(message, ErrorCodes.GetVacancySummariesServiceFailed);
            }

            _logger.Info("Vacancy summary page count retrieved from Legacy.GetApplicationsStatus ({0})", response.TotalPages);

            return response.TotalPages;
        }

        public VacancySummaries GetVacancySummaries(int page)
        {
            var request = new GetVacancySummaryRequest {PageNumber = page};

            var response = default(GetVacancySummaryResponse);

            _logger.Info("Calling Legacy.GetVacancySummaries for page {0}", page);

            _service.Use("SecureService", client => response = client.GetVacancySummaries(request));

            if (response == null)
            {
                var message = string.Format("Failed to retrieve page '{0}' from Legacy.GetVacancySummaries", page);

                _logger.Error(message);
                throw new CustomException(message, ErrorCodes.GetVacancySummariesServiceFailed);
            }

            var apprenticeshipTypes = new[]
            {
                "IntermediateLevelApprenticeship", "AdvancedLevelApprenticeship", "HigherApprenticeship"
            };

            var apprenticeshipSummaries = _mapper.Map<VacancySummary[], IEnumerable<ApprenticeshipSummary>>(
                response.VacancySummaries.Where(v => apprenticeshipTypes.Contains(v.VacancyType)).ToArray());

            var traineeshipsSummaries = _mapper.Map<VacancySummary[], IEnumerable<TraineeshipSummary>>(
                response.VacancySummaries.Where(v => v.VacancyType == "Traineeship").ToArray());

            _logger.Info("Vacancy summaries (page {0}) were successfully retrieved from Legacy.GetVacancySummaries ({1})",
                page, response.VacancySummaries.Count());

            return new VacancySummaries(apprenticeshipSummaries, traineeshipsSummaries);
        }
    }
}
