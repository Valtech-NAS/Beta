namespace SFA.Apprenticeships.Application.Candidate.Strategies.Apprenticeships
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

    public class LegacySubmitApprenticeshipApplicationStrategy : ISubmitApprenticeshipApplicationStrategy
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly IApprenticeshipApplicationWriteRepository _apprenticeshipApplicationWriteRepository;
        private readonly IMessageBus _messageBus;
        private readonly ICommunicationService _communicationService;

        public LegacySubmitApprenticeshipApplicationStrategy(IMessageBus messageBus, IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            IApprenticeshipApplicationWriteRepository apprenticeshipApplicationWriteRepository, ICommunicationService communicationService)
        {
            _messageBus = messageBus;
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _apprenticeshipApplicationWriteRepository = apprenticeshipApplicationWriteRepository;
            _communicationService = communicationService;
        }

        public void SubmitApplication(Guid candidateId, int vacancyId)
        {
            var applicationDetail = _apprenticeshipApplicationReadRepository.GetForCandidate(candidateId, vacancyId, true);

            applicationDetail.AssertState("Submit apprenticeshipApplication", ApplicationStatuses.Draft);

            try
            {
                applicationDetail.SetStateSubmitting();
                _apprenticeshipApplicationWriteRepository.Save(applicationDetail);

                PublishMessage(applicationDetail);
                NotifyCandidate(applicationDetail.CandidateId, applicationDetail.EntityId.ToString());
            }
            catch (Exception ex)
            {
                Logger.Debug("SubmitApplicationRequest could not be queued for ApplicationId={0}", applicationDetail.EntityId);

                throw new CustomException("SubmitApplicationRequest could not be queued", ex,
                    MessagingErrorCodes.ApplicationQueuingError);
            }
        }

        private void PublishMessage(ApprenticeshipApplicationDetail apprenticeshipApplicationDetail)
        {
            try
            {
                var message = new SubmitApplicationRequest
                {
                    ApplicationId = apprenticeshipApplicationDetail.EntityId
                };

                _messageBus.PublishMessage(message);
            }
            catch
            {
                apprenticeshipApplicationDetail.RevertStateToDraft();
                _apprenticeshipApplicationWriteRepository.Save(apprenticeshipApplicationDetail);
                throw;
            }
        }

        private void NotifyCandidate(Guid candidateId, string applicationId)
        {
            _communicationService.SendMessageToCandidate(candidateId, MessageTypes.ApprenticeshipApplicationSubmitted,
                new[]
                {
                    new CommunicationToken(CommunicationTokens.ApplicationId, applicationId)
                });
        }
    }
}