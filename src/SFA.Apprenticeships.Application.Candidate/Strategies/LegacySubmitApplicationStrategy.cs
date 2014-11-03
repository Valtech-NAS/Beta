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
    using SFA.Apprenticeships.Infrastructure.PerformanceCounters;

    public class LegacySubmitApplicationStrategy : ISubmitApplicationStrategy
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IApplicationReadRepository _applicationReadRepository;
        private readonly IApplicationWriteRepository _applicationWriteRepository;
        private readonly IMessageBus _messageBus;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ICommunicationService _communicationService;
        private readonly IPerformanceCounterService _performanceCounterService;

        public LegacySubmitApplicationStrategy(IMessageBus messageBus, IApplicationReadRepository applicationReadRepository,
            IApplicationWriteRepository applicationWriteRepository, ICommunicationService communicationService,
            ICandidateReadRepository candidateReadRepository, IPerformanceCounterService performanceCounterService)
        {
            _messageBus = messageBus;
            _applicationReadRepository = applicationReadRepository;
            _applicationWriteRepository = applicationWriteRepository;
            _communicationService = communicationService;
            _candidateReadRepository = candidateReadRepository;
            _performanceCounterService = performanceCounterService;
        }

        public void SubmitApplication(Guid applicationId)
        {
            var applicationDetail = _applicationReadRepository.Get(applicationId, true);

            // status check - should be in "draft" state
            applicationDetail.AssertState("Application is not in the correct state to be submitted", ApplicationStatuses.Draft);

            var candidate = _candidateReadRepository.Get(applicationDetail.CandidateId);

            try
            {
                var tempApplicationDetail = _applicationReadRepository.Get(applicationId, true);

                if (tempApplicationDetail.Status == ApplicationStatuses.Draft)
                {
                    // queue application for submission to legacy
                    var message = new SubmitApplicationRequest
                    {
                        ApplicationId = applicationDetail.EntityId
                    };

                    _messageBus.PublishMessage(message);

                    // update application status to "submitting"
                    applicationDetail.SetStateSubmitting();

                    _applicationWriteRepository.Save(applicationDetail);

                    _performanceCounterService.IncrementApplicationSubmissionCounter();

                    // send email acknowledgement to candidate
                    NotifyCandidate(candidate.EntityId, applicationDetail.EntityId.ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.Debug("SubmitApplicationRequest could not be queued for ApplicationId={0}", applicationId);
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