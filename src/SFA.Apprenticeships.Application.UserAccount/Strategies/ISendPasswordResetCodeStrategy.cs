namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using System;

    public interface ISendPasswordResetCodeStrategy
    {
        void SendPasswordResetCode(string username);
    }
}
