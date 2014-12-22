namespace SFA.Apprenticeships.Application.Candidate.Strategies.Traineeships
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Interfaces.Messaging;
    using NLog;
    using Vacancy;

    public class LegacySubmitTraineeshipApplicationStrategy : ISubmitTraineeshipApplicationStrategy
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ICommunicationService _communicationService;
        private readonly IMessageBus _messageBus;

        private readonly ITraineeshipApplicationReadRepository _traineeshipApplicationReadRepository;
        private readonly IVacancyDataProvider<TraineeshipVacancyDetail> _traineeshipVacancyDataProvider;

        public LegacySubmitTraineeshipApplicationStrategy(IMessageBus messageBus,
            ICommunicationService communicationService,
            ITraineeshipApplicationReadRepository traineeshipApplicationReadRepository,
            IVacancyDataProvider<TraineeshipVacancyDetail> traineeshipVacancyDataProvider)
        {
            _messageBus = messageBus;
            _communicationService = communicationService;
            _traineeshipApplicationReadRepository = traineeshipApplicationReadRepository;
            _traineeshipVacancyDataProvider = traineeshipVacancyDataProvider;
        }

        public void SubmitApplication(Guid applicationId)
        {
            var traineeshipApplicationDetail = _traineeshipApplicationReadRepository.Get(applicationId, true);

            try
            {
                PublishMessage(traineeshipApplicationDetail);
                NotifyCandidate(traineeshipApplicationDetail);
            }
            catch (Exception ex)
            {
                Logger.Debug("SubmitTraineeshipApplicationRequest could not be queued for ApplicationId={0}", applicationId);

                throw new CustomException("SubmitTraineeshipApplicationRequest could not be queued", ex,
                    ErrorCodes.ApplicationQueuingError);
            }
        }

        private void PublishMessage(TraineeshipApplicationDetail traineeshipApplicationDetail)
        {
            var message = new SubmitTraineeshipApplicationRequest
            {
                ApplicationId = traineeshipApplicationDetail.EntityId
            };

            _messageBus.PublishMessage(message);
        }

        private void NotifyCandidate(TraineeshipApplicationDetail applicationDetail)
        {
            var vacancyDetail = _traineeshipVacancyDataProvider.GetVacancyDetails(applicationDetail.Vacancy.Id);

            _communicationService.SendMessageToCandidate(applicationDetail.CandidateId, CandidateMessageTypes.TraineeshipApplicationSubmitted,
                new[]
                {
                    new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.ApplicationId, applicationDetail.EntityId.ToString()),
                    new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.ApplicationVacancyReference, applicationDetail.Vacancy.VacancyReference),
                    new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.ProviderContact, vacancyDetail.Contact)
                });
        }
    }
}