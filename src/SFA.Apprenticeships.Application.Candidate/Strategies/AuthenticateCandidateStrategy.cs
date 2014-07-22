namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using Interfaces.Users;

    public class AuthenticateCandidateStrategy : IAuthenticateCandidateStrategy
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserReadRepository _userReadRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ILockAccountStrategy _lockAccountStrategy;

        public AuthenticateCandidateStrategy(IAuthenticationService authenticationService,
            IUserReadRepository userReadRepository, ICandidateReadRepository candidateReadRepository,
            ILockAccountStrategy lockAccountStrategy)
        {
            _authenticationService = authenticationService;
            _userReadRepository = userReadRepository;
            _candidateReadRepository = candidateReadRepository;
            _lockAccountStrategy = lockAccountStrategy;
        }

        public Candidate AuthenticateCandidate(string username, string password)
        {
            var user = _userReadRepository.Get(username);

            // TODO: NOTIMPL: check status of user (only allowed to authenticate if active or pending)
            //throw new Exception("Authentication not allowed"); // TODO: EXCEPTION: should use an application exception type

            var authenticated = _authenticationService.AuthenticateUser(user.EntityId, password);

            if (authenticated)
            {
                // TODO: if succeed auth, reset failure counter(s)
                // TODO: if succeed auth, MVC layer must subsequently check User.Status and branch to activate (etc) if necessary

                var candidate = _candidateReadRepository.Get(user.EntityId);

                //_auditLog.Info(AuditEvents.SuccessfulLogon, username); //TODO: audit successful logon (named logger)

                return candidate;
            }

            RegisterFailedLogin(user);
            // either way, throw an exception to indicate failed auth
            throw new Exception("Authentication failed"); // TODO: EXCEPTION: should use an application exception type
        }

        #region Helpers
        private void RegisterFailedLogin(User user)
        {
            //todo: if too many fails then lock the account
            if (user.LoginRemainingAttempts == 0)
            {
                _lockAccountStrategy.LockAccount(user);
            }
            else
            {
                //todo: decrement counter and save user
                user.LoginRemainingAttempts--;
                //_userWriteRepository.Save(user);
            }
        }
        #endregion
    }
}
