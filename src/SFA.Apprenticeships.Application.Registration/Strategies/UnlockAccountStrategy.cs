namespace SFA.Apprenticeships.Application.Registration.Strategies
{
    using System;

    public class UnlockAccountStrategy : IUnlockAccountStrategy
    {
        public void UnlockAccount(string username, string accountUnlockCode)
        {
            //var user = _userReadRepository.Get(username);
            // TODO: NOTIMPL: check status of user (only allowed if locked)

            // TODO: check if code is correct and not expired

            // TODO: if correct then update user account status and clear user properties related to locking

            throw new NotImplementedException();
        }
    }
}
