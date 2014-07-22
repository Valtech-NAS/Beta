namespace SFA.Apprenticeships.Application.Registration.Strategies
{
    using System;

    public class ResetForgottenPasswordStrategy : IResetForgottenPasswordStrategy
    {
        public void ResetForgottenPassword(string username, string passwordCode, string newPassword)
        {
            //var user = _userReadRepository.Get(username);
            // TODO: NOTIMPL: check status of user (only allowed to change p/w if active or locked)

            // TODO: check if code is correct

            // TODO: if incorrect then increase counter. if too many fails (based on config setting) then lock account (update status) and throw ex
            //e.g. RegisterFailedPasswordReset(user);
            // TODO: if correct then set new p/w in AD, clear code and counter on user, also unlock if locked

            throw new NotImplementedException();
        }
    }
}
