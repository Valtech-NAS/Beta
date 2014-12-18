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

    public class LegacySubmitTraineeshipApplicationStrategy : ISubmitApplicationStrategy
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ICommunicationService _communicationService;
        private readonly IMessageBus _messageBus;
        private readonly ITraineeshipApplicationReadRepository _traineeshipApplicationReadRepository;
        

        public LegacySubmitTraineeshipApplicationStrategy(IMessageBus messageBus,
            ICommunicationService communicationService,
            ITraineeshipApplicationReadRepository traineeshipApplicationReadRepository)
        {
            _messageBus = messageBus;
            _communicationService = communicationService;
            _traineeshipApplicationReadRepository = traineeshipApplicationReadRepository;
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
            _communicationService.SendMessageToCandidate(applicationDetail.CandidateId, CandidateMessageTypes.TraineeshipApplicationSubmitted,
                new[]
                {
                    new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.ApplicationId, applicationDetail.EntityId.ToString())
                });
        }
    }
}