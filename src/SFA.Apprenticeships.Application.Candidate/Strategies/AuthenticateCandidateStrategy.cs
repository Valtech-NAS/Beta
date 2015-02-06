namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using Interfaces.Users;
    using UserAccount.Strategies;

    public class AuthenticateCandidateStrategy : IAuthenticateCandidateStrategy
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ILockAccountStrategy _lockAccountStrategy;
        private readonly int _maximumPasswordAttemptsAllowed;

        public AuthenticateCandidateStrategy(
            IConfigurationManager configManager,
            IAuthenticationService authenticationService,
            IUserReadRepository userReadRepository,
            IUserWriteRepository userWriteRepository,
            ICandidateReadRepository candidateReadRepository,
            ILockAccountStrategy lockAccountStrategy)
        {
            _userWriteRepository = userWriteRepository;
            _authenticationService = authenticationService;
            _userReadRepository = userReadRepository;
            _candidateReadRepository = candidateReadRepository;
            _lockAccountStrategy = lockAccountStrategy;
            _maximumPasswordAttemptsAllowed = configManager.GetAppSetting<int>("MaximumPasswordAttemptsAllowed");
        }

        public Candidate AuthenticateCandidate(string username, string password)
        {
            var user = _userReadRepository.Get(username, false);

            if (user != null)
            {
                user.AssertState("Authenticate user", UserStatuses.Active, UserStatuses.PendingActivation, UserStatuses.Locked);

                if (_authenticationService.AuthenticateUser(user.EntityId, password))
                {
                    var candidate = _candidateReadRepository.Get(user.EntityId);

                    if (user.LoginIncorrectAttempts > 0)
                    {
                        user.SetStateActive();
                        _userWriteRepository.Save(user);
                    }

                    return candidate;
                }

                RegisterFailedLogin(user);
            }

            return null;
        }

        private void RegisterFailedLogin(User user)
        {
            user.LoginIncorrectAttempts++;

            if (user.LoginIncorrectAttempts < _maximumPasswordAttemptsAllowed)
            {
                _userWriteRepository.Save(user);
            }
            else
            {
                _lockAccountStrategy.LockAccount(user);
            }
        }
    }
}
