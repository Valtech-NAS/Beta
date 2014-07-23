namespace SFA.Apprenticeships.Application.Registration.Strategies
{
    using System;
    using Domain.Entities.Users;
    using Interfaces.Messaging;

    public class ResetForgottenPasswordStrategy : IResetForgottenPasswordStrategy
    {
        private readonly ICommunicationService _communicationService;
        private readonly ILockAccountStrategy _lockAccountStrategy;

        public ResetForgottenPasswordStrategy(ICommunicationService communicationService, ILockAccountStrategy lockAccountStrategy)
        {
            _communicationService = communicationService;
            _lockAccountStrategy = lockAccountStrategy;
        }

        public void ResetForgottenPassword(string username, string passwordCode, string newPassword)
        {
            //var user = _userReadRepository.Get(username);
            // TODO: NOTIMPL: check status of user (only allowed to change p/w if active or locked)

            // TODO: check if code is correct and not expired

            // TODO: if incorrect then increase counter. if too many fails (based on config setting) then lock account (using strategy) and throw ex
            //RegisterFailedPasswordReset(user);

            // TODO: if correct then set new p/w in AD, clear code and counter on user, also unlock if locked, send email
            //_communicationService.SendMessageToCandidate(); PasswordChanged

            throw new NotImplementedException();
        }

        #region Helpers
        private void RegisterFailedPasswordReset(User user)
        {
            //todo: if too many fails then lock the account
            if (user.PasswordResetIncorrectAttempts == 3) //todo: from config
            {
                _lockAccountStrategy.LockAccount(user);
            }
            else
            {
                //todo: decrement counter and save user
                user.PasswordResetIncorrectAttempts++;
                //_userWriteRepository.Save(user);
            }
        }
        #endregion
    }
}
