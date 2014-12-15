namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Interfaces.Messaging;
    using NLog;

    public class LegacySubmitApplicationStrategy : ISubmitApplicationStrategy
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IApplicationReadRepository _applicationReadRepository;
        private readonly IApplicationWriteRepository _applicationWriteRepository;
        private readonly IMessageBus _messageBus;
        private readonly ICommunicationService _communicationService;

        public LegacySubmitApplicationStrategy(IMessageBus messageBus, IApplicationReadRepository applicationReadRepository,
            IApplicationWriteRepository applicationWriteRepository, ICommunicationService communicationService)
        {
            _messageBus = messageBus;
            _applicationReadRepository = applicationReadRepository;
            _applicationWriteRepository = applicationWriteRepository;
            _communicationService = communicationService;
        }

        public void SubmitApplication(Guid applicationId)
        {
            var applicationDetail = _applicationReadRepository.Get(applicationId, true);

            applicationDetail.AssertState("Submit apprenticeshipApplication", ApplicationStatuses.Draft);

            try
            {
                applicationDetail.SetStateSubmitting();
                _applicationWriteRepository.Save(applicationDetail);

                PublishMessage(applicationDetail);
                NotifyCandidate(applicationDetail.CandidateId, applicationDetail.EntityId.ToString());
            }
            catch (Exception ex)
            {
                Logger.Debug("SubmitApplicationRequest could not be queued for ApplicationId={0}", applicationId);

                throw new CustomException("SubmitApplicationRequest could not be queued", ex,
                    ErrorCodes.ApplicationQueuingError);
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
                _applicationWriteRepository.Save(apprenticeshipApplicationDetail);
                throw;
            }
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