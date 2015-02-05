namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using System;
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using Interfaces.Communications;
    using Interfaces.Users;

    public class ResetForgottenPasswordStrategy : IResetForgottenPasswordStrategy
    {
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly ICommunicationService _communicationService;
        private readonly ILockAccountStrategy _lockAccountStrategy;
        private readonly IAuthenticationService _authenticationService;
        private readonly int _maximumPasswordAttemptsAllowed;

        public ResetForgottenPasswordStrategy(ICommunicationService communicationService,
            ILockAccountStrategy lockAccountStrategy,
            ICandidateReadRepository candidateReadRepository,
            IUserReadRepository userReadRepository,
            IAuthenticationService authenticationService,
            IConfigurationManager configurationManager,
            IUserWriteRepository userWriteRepository)
        {
            _communicationService = communicationService;
            _lockAccountStrategy = lockAccountStrategy;
            _candidateReadRepository = candidateReadRepository;
            _userReadRepository = userReadRepository;
            _authenticationService = authenticationService;
            _userWriteRepository = userWriteRepository;
            _maximumPasswordAttemptsAllowed = configurationManager.GetAppSetting<int>("MaximumPasswordAttemptsAllowed");
        }

        public void ResetForgottenPassword(string username, string passwordCode, string newPassword)
        {
            var user = _userReadRepository.Get(username);

            var candidate = _candidateReadRepository.Get(user.EntityId);

            if (user.PasswordResetCode != null && user.PasswordResetCode.Equals(passwordCode, StringComparison.CurrentCultureIgnoreCase))
            {
                if (user.PasswordResetCodeExpiry != null && DateTime.Now > user.PasswordResetCodeExpiry)
                {
                    throw new CustomException("Password reset code has expired.", Interfaces.Users.ErrorCodes.UserPasswordResetCodeExpiredError);
                }

                _authenticationService.ResetUserPassword(user.EntityId, newPassword);

                user.SetStateActive();

                _userWriteRepository.Save(user);

                SendPasswordResetConfirmationMessage(candidate);
            }
            else
            {
                RegisterFailedPasswordReset(user);

                throw new CustomException("Password reset code \"{0}\" is invalid for user \"{1}\"", Interfaces.Users.ErrorCodes.UserPasswordResetCodeIsInvalid, passwordCode, username);
            }
        }

        #region Helpers

        private void RegisterFailedPasswordReset(User user)
        {
            if (user.PasswordResetIncorrectAttempts == _maximumPasswordAttemptsAllowed)
            {
                _lockAccountStrategy.LockAccount(user);
                throw new CustomException("Maximum password attempts allowed reached, account is now locked.", Interfaces.Users.ErrorCodes.UserAccountLockedError);
            }

            user.PasswordResetIncorrectAttempts++;
            _userWriteRepository.Save(user);
        }

        private void SendPasswordResetConfirmationMessage(Candidate candidate)
        {
            var firstName = candidate.RegistrationDetails.FirstName;
            var emailAddress = candidate.RegistrationDetails.EmailAddress;

            _communicationService.SendMessageToCandidate(candidate.EntityId, MessageTypes.PasswordChanged,
                new[]
                {
                    new CommunicationToken(CommunicationTokens.CandidateFirstName, firstName),
                    new CommunicationToken(CommunicationTokens.Username, emailAddress)
                });
        }

        #endregion
    }
}
