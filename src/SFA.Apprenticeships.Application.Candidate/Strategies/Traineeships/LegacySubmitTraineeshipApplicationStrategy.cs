namespace SFA.Apprenticeships.Application.Candidate.Strategies.Traineeships
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Interfaces.Messaging;
    using NLog;
    using MessagingErrorCodes = Application.Interfaces.Messaging.ErrorCodes;

    public class LegacySubmitTraineeshipApplicationStrategy : ISubmitTraineeshipApplicationStrategy
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ICommunicationService _communicationService;
        private readonly IMessageBus _messageBus;

        private readonly ITraineeshipApplicationReadRepository _traineeshipApplicationReadRepository;
        private readonly ITraineeshipApplicationWriteRepository _traineeshipApplicationWriteRepository;

        public LegacySubmitTraineeshipApplicationStrategy(
            IMessageBus messageBus,
            ICommunicationService communicationService,
            ITraineeshipApplicationReadRepository traineeshipApplicationReadRepository,
            ITraineeshipApplicationWriteRepository traineeshipApplicationWriteRepository)
        {
            _messageBus = messageBus;
            _communicationService = communicationService;
            _traineeshipApplicationReadRepository = traineeshipApplicationReadRepository;
            _traineeshipApplicationWriteRepository = traineeshipApplicationWriteRepository;
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
                    MessagingErrorCodes.ApplicationQueuingError);
            }
        }

        private void PublishMessage(TraineeshipApplicationDetail traineeshipApplicationDetail)
        {
            try
            {
                var message = new SubmitTraineeshipApplicationRequest
                {
                    ApplicationId = traineeshipApplicationDetail.EntityId
                };

                _messageBus.PublishMessage(message);
            }
            catch
            {
                // Compensate for failure to enqueue application submission by deleting the application.
                _traineeshipApplicationWriteRepository.Delete(traineeshipApplicationDetail.EntityId);
                throw;
            }
        }

        private void NotifyCandidate(TraineeshipApplicationDetail traineeshipApplicationDetail)
        {
            _communicationService.SendMessageToCandidate(traineeshipApplicationDetail.CandidateId, MessageTypes.TraineeshipApplicationSubmitted,
                new[]
                {
                    new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.ApplicationId, traineeshipApplicationDetail.EntityId.ToString())
                });
        }
    }
}