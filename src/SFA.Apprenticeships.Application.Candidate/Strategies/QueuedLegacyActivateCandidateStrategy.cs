namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using Domain.Entities.Users;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Interfaces.Logging;
    using Interfaces.Users;

    public class QueuedLegacyActivateCandidateStrategy : IActivateCandidateStrategy
    {
        private readonly IMessageBus _messageBus;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserAccountService _registrationService;
        private readonly ILogService _logService;

        public QueuedLegacyActivateCandidateStrategy(IMessageBus messageBus, IUserReadRepository userReadRepository, IUserAccountService registrationService, ILogService logService)
        {
            _messageBus = messageBus;
            _userReadRepository = userReadRepository;
            _registrationService = registrationService;
            _logService = logService;
        }

        public void ActivateCandidate(string username, string activationCode)
        {
            var user = _userReadRepository.Get(username);

            user.AssertState("Activate candidate", UserStatuses.PendingActivation);

            // Activate user before message submission so that they can continue using the site
            _registrationService.Activate(username, activationCode);
            
            // queue request for submission to legacy
            var message = new CreateCandidateRequest
            {
                CandidateId = user.EntityId
            };

            _logService.Info("Publishing CreateCandidateRequest for Candidate with Id: {0}", message.CandidateId);
            _messageBus.PublishMessage(message);
            _logService.Info("Successfully published CreateCandidateRequest for Candidate with Id: {0}", message.CandidateId);
        }
    }
}