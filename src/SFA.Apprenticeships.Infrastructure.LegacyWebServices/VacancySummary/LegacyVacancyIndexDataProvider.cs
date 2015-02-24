namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.VacancySummary
{
    using System;
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
            try
            {
                _logger.Info("Calling Legacy.GetVacancySummaries for page count");

                var totalPages = InternalGetVacancyPageCount();

                _logger.Info("Vacancy summary page count retrieved from Legacy.GetVacancySummaries ({0})", totalPages);

                return totalPages;
            }
            catch (DomainException e)
            {
                _logger.Error(e);
                throw;
            }
            catch (BoundaryException e)
            {
                _logger.Error(e);
                throw new DomainException(ErrorCodes.GetVacancySummariesServiceFailed, e);
            }
            catch (Exception e)
            {
                _logger.Error(e);
                throw;
            }
        }

        public VacancySummaries GetVacancySummaries(int pageNumber)
        {
            var context = new { pageNumber };

            try
            {
                _logger.Info("Calling Legacy.GetVacancySummaries for page {0}", pageNumber);

                var vacancySummaries = InternalGetVacancySummaries(pageNumber);

                _logger.Info("Vacancy summaries (page {0}) were successfully retrieved from Legacy.GetVacancySummaries ({1})",
                    pageNumber, vacancySummaries.ApprenticeshipSummaries.Count() + vacancySummaries.TraineeshipSummaries.Count());

                return vacancySummaries;
            }
            catch (DomainException e)
            {
                _logger.Error(e);
                throw;
            }
            catch (BoundaryException e)
            {
                _logger.Error(e, context);
                throw new DomainException(ErrorCodes.GetVacancySummariesServiceFailed, e, context);
            }
            catch (Exception e)
            {
                _logger.Error(e, context);
                throw;
            }
        }

        #region Helpers

        private int InternalGetVacancyPageCount()
        {
            var request = new GetVacancySummaryRequest
            {
                PageNumber = 1
            };

            var response = default(GetVacancySummaryResponse);

            _service.Use("SecureService", client => response = client.GetVacancySummaries(request));

            if (response == null)
            {
                throw new DomainException(ErrorCodes.GetVacancySummariesServiceFailed, new { pageNumber = request.PageNumber });
            }

            return response.TotalPages;
        }

        private VacancySummaries InternalGetVacancySummaries(int pageNumber)
        {
            var request = new GetVacancySummaryRequest
            {
                PageNumber = pageNumber
            };

            var response = default(GetVacancySummaryResponse);

            _service.Use("SecureService", client => response = client.GetVacancySummaries(request));

            if (response == null)
            {
                throw new DomainException(ErrorCodes.GetVacancySummariesServiceFailed, new { pageNumber });
            }

            var apprenticeshipTypes = new[]
            {
                "IntermediateLevelApprenticeship",
                "AdvancedLevelApprenticeship",
                "HigherApprenticeship"
            };

            var apprenticeshipSummaries = _mapper.Map<VacancySummary[], IEnumerable<ApprenticeshipSummary>>(
                response.VacancySummaries.Where(vacancySummary => apprenticeshipTypes.Contains(vacancySummary.VacancyType)).ToArray());

            var traineeshipsSummaries = _mapper.Map<VacancySummary[], IEnumerable<TraineeshipSummary>>(
                response.VacancySummaries.Where(vacancySummary => vacancySummary.VacancyType == "Traineeship").ToArray());

            return new VacancySummaries(apprenticeshipSummaries, traineeshipsSummaries);
        }
    }

    #endregion
}
