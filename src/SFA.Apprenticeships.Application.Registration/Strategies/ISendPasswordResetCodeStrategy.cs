namespace SFA.Apprenticeships.Application.Registration.Strategies
{
    using System;

    public interface ISendPasswordResetCodeStrategy
    {
        void SendPasswordResetCode(string username);
    }
}
