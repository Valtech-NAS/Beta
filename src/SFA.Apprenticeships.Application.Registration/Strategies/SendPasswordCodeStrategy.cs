namespace SFA.Apprenticeships.Application.Registration.Strategies
{
    using System;
    using Domain.Interfaces.Configuration;
    using Interfaces.Messaging;

    public class SendPasswordCodeStrategy : ISendPasswordCodeStrategy
    {
        private readonly ICommunicationService _communicationService;
        private readonly int _passwordResetCodeExpiryDays;

        public SendPasswordCodeStrategy(IConfigurationManager configurationManager, ICommunicationService communicationService)
        {
            _communicationService = communicationService;
            _passwordResetCodeExpiryDays = 1; //todo: configurationManager.GetAppSetting<int>("PasswordResetCodeExpiryDays");
        }

        public void SendPasswordResetCode(string username)
        {
            //todo: send/resend the email containing the code
            //todo: resend the SAME reset code if it exists and hasn't expired else send a new one

            //_communicationService.SendMessageToCandidate();

            throw new NotImplementedException();
        }
    }
}
