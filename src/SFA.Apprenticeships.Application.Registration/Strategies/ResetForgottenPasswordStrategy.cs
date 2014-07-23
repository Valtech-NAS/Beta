namespace SFA.Apprenticeships.Application.Registration.Strategies
{
    using System;
    using Interfaces.Messaging;

    public class ResetForgottenPasswordStrategy : IResetForgottenPasswordStrategy
    {
        private readonly ICommunicationService _communicationService;

        public ResetForgottenPasswordStrategy(ICommunicationService communicationService)
        {
            _communicationService = communicationService;
        }

        public void ResetForgottenPassword(string username, string passwordCode, string newPassword)
        {
            //var user = _userReadRepository.Get(username);
            // TODO: NOTIMPL: check status of user (only allowed to change p/w if active or locked)

            // TODO: check if code is correct and not expired

            // TODO: if incorrect then increase counter. if too many fails (based on config setting) then lock account (update status) and throw ex
            //e.g. RegisterFailedPasswordReset(user);

            // TODO: if correct then set new p/w in AD, clear code and counter on user, also unlock if locked, send email
            //_communicationService.SendMessageToCandidate(); PasswordChanged

            throw new NotImplementedException();
        }
    }
}
