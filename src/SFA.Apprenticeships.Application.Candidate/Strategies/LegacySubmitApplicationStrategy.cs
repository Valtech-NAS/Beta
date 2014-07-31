namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Interfaces.Messaging;

    public class LegacySubmitApplicationStrategy : ISubmitApplicationStrategy
    {
        private readonly IApplicationReadRepository _applicationReadRepository;
        private readonly IApplicationWriteRepository _applicationWriteRepository;
        private readonly IMessageBus _bus;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ICommunicationService _communicationService;

        public LegacySubmitApplicationStrategy(IMessageBus bus, IApplicationReadRepository applicationReadRepository,
            IApplicationWriteRepository applicationWriteRepository, ICommunicationService communicationService,
            ICandidateReadRepository candidateReadRepository)
        {
            _bus = bus;
            _applicationReadRepository = applicationReadRepository;
            _applicationWriteRepository = applicationWriteRepository;
            _communicationService = communicationService;
            _candidateReadRepository = candidateReadRepository;
        }

        public void SubmitApplication(Guid applicationId)
        {
            var applicationDetail = _applicationReadRepository.Get(applicationId);

            if (applicationDetail == null)
            {
                throw new CustomException("Application detail was not found", ErrorCodes.ApplicationNotFoundError);
            }

            // status check - should be in "draft" state
            applicationDetail.AssertState("Application is not in the correct state to be submitted", ApplicationStatuses.Draft);

            var candidate = _candidateReadRepository.Get(applicationDetail.CandidateId);

            if (candidate == null)
            {
                throw new CustomException("Unknown candidate", ErrorCodes.UnknownCandidateError);
            }

            try
            {
                // queue application for submission to legacy
                var message = new SubmitApplicationRequest
                {
                    ApplicationId = applicationDetail.EntityId
                };

                _bus.PublishMessage(message);

                // update application status to "submitting"
                applicationDetail.SetStateSubmitting();

                _applicationWriteRepository.Save(applicationDetail);

                // send email acknowledgement to candidate
                NotifyCandidate(candidate.EntityId, applicationDetail.EntityId.ToString());
            }
            catch (Exception ex)
            {
                throw new CustomException("SubmitApplicationRequest could not be queued", ex,
                    ErrorCodes.ApplicationQueuingError);
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