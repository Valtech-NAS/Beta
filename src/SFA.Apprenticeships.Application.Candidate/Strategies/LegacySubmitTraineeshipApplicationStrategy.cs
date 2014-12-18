namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using System.Collections.Generic;
    using NLog;
    using SFA.Apprenticeships.Application.Interfaces.Messaging;
    using SFA.Apprenticeships.Application.Vacancy;
    using SFA.Apprenticeships.Domain.Entities.Applications;
    using SFA.Apprenticeships.Domain.Entities.Exceptions;
    using SFA.Apprenticeships.Domain.Interfaces.Messaging;
    using SFA.Apprenticeships.Domain.Interfaces.Repositories;

    public class LegacySubmitTraineeshipApplicationStrategy : ISubmitTraineeshipApplicationStrategy
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ICommunicationService _communicationService;
        private readonly IMessageBus _messageBus;
        private readonly ITraineeshipApplicationReadRepository _traineeshipApplicationReadRepository;
        private readonly IVacancyDataProvider _vacancyDataProvider;

        public LegacySubmitTraineeshipApplicationStrategy(IMessageBus messageBus,
            ICommunicationService communicationService,
            IVacancyDataProvider vacancyDataProvider,
            ICandidateReadRepository candidateReadRepository,
            ITraineeshipApplicationReadRepository traineeshipApplicationReadRepository)
        {
            _messageBus = messageBus;
            _communicationService = communicationService;
            _vacancyDataProvider = vacancyDataProvider;
            _candidateReadRepository = candidateReadRepository;
            _traineeshipApplicationReadRepository = traineeshipApplicationReadRepository;
        }

        public void SubmitApplication(Guid applicationId)
        {
            var applicationDetail = _traineeshipApplicationReadRepository.Get(applicationId, true);

            try
            {
                PublishMessage(applicationDetail);
                // NotifyCandidate(applicationDetail.CandidateId, applicationDetail.EntityId.ToString());
            }
            catch (Exception ex)
            {
                Logger.Debug("SubmitApplicationRequest could not be queued for ApplicationId={0}",
                    applicationDetail.EntityId);

                throw new CustomException("SubmitApplicationRequest could not be queued", ex,
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

        private void NotifyCandidate(Guid candidateId, string applicationId)
        {
            _communicationService.SendMessageToCandidate(candidateId, CandidateMessageTypes.ApplicationSubmitted,
                new[]
                {
                    new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.ApplicationId, applicationId)
                });
        }
    }
}