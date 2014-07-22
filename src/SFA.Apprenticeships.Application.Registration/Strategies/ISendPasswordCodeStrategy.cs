namespace SFA.Apprenticeships.Application.Registration.Strategies
{
    using System;

    public interface ISendPasswordCodeStrategy
    {
        void SendPasswordResetCode(string username);
    }
}
