namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using Domain.Entities.Users;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Interfaces.Messaging;
    using Interfaces.Users;

    public class QueuedLegacyActivateCandidateStrategy : IActivateCandidateStrategy
    {
        private readonly IMessageBus _messageBus;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserAccountService _registrationService;

        public QueuedLegacyActivateCandidateStrategy(IMessageBus messageBus, IUserReadRepository userReadRepository, IUserAccountService registrationService)
        {
            _messageBus = messageBus;
            _userReadRepository = userReadRepository;
            _registrationService = registrationService;
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

            _messageBus.PublishMessage(message);
        }
    }
}