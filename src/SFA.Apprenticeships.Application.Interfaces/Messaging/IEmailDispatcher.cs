namespace SFA.Apprenticeships.Application.Interfaces.Messaging
{
    using System;

    /// <summary>
    /// Used to queue / send an email message
    /// </summary>
    public interface IEmailDispatcher
    {
        //todo: finish IEmailDispatcher.SendEmail signature 
        void SendEmail(string to, string from, string subject, string message);
    }
}
