﻿namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using ApplicationUpdate;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Messaging;
    using Interfaces.Logging;
    using Vacancy;
    using VacancyEtl.Entities;
    using ErrorCodes = Interfaces.Vacancies.ErrorCodes;

    public class LegacyGetCandidateVacancyDetailStrategy<TVacancyDetail> : ILegacyGetCandidateVacancyDetailStrategy<TVacancyDetail>
        where TVacancyDetail : VacancyDetail
    {
        private readonly ILogService _logger;

        private readonly IVacancyDataProvider<TVacancyDetail> _vacancyDataProvider;
        private readonly IApplicationVacancyUpdater _applicationVacancyUpdater;
        private readonly IMessageBus _bus;

        public LegacyGetCandidateVacancyDetailStrategy(
            IVacancyDataProvider<TVacancyDetail> vacancyDataProvider,
            IApplicationVacancyUpdater applicationVacancyUpdater,
            ILogService logger,
            IMessageBus bus)
        {
            _bus = bus;
            _vacancyDataProvider = vacancyDataProvider;
            _applicationVacancyUpdater = applicationVacancyUpdater;
            _logger = logger;
        }

        public TVacancyDetail GetVacancyDetails(Guid candidateId, int vacancyId)
        {
            _logger.Debug("Calling LegacyGetCandidateVacancyDetailStrategy to get vacancy details for vacancy ID {0} and candidate ID {1}.", vacancyId, candidateId);

            try
            {
                var vacancyDetails = _vacancyDataProvider.GetVacancyDetails(vacancyId);

                _applicationVacancyUpdater.Update(candidateId, vacancyId, vacancyDetails);

                QueueVacancyStatusSummaryUpdate(vacancyDetails);

                return vacancyDetails;
            }
            catch (Exception e)
            {
                var message = string.Format("Get vacancy failed for vacancy ID {0} and candidate ID {1}.", vacancyId, candidateId);

                _logger.Debug(message, e);

                throw new CustomException(message, e, ErrorCodes.GetVacancyDetailsFailed);
            }
        }

        private void QueueVacancyStatusSummaryUpdate(TVacancyDetail vacancyDetails)
        {
            var vacancyStatusSummary = new VacancyStatusSummary
            {
                LegacyVacancyId = vacancyDetails.Id,
                VacancyStatus = vacancyDetails.VacancyStatus,
                ClosingDate = vacancyDetails.ClosingDate,
                DateTime = DateTime.Now
            };

            _bus.PublishMessage(vacancyStatusSummary);
        }
    }
}
