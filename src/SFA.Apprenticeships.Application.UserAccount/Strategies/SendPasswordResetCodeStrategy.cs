namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using System;
    using Domain.Interfaces.Configuration;
    using Interfaces.Messaging;

    public class SendPasswordResetCodeStrategy : ISendPasswordResetCodeStrategy
    {
        private readonly ICommunicationService _communicationService;
        private readonly int _passwordResetCodeExpiryDays;

        public SendPasswordResetCodeStrategy(IConfigurationManager configurationManager, ICommunicationService communicationService)
        {
            _communicationService = communicationService;
            _passwordResetCodeExpiryDays = 1; //todo: configurationManager.GetAppSetting<int>("PasswordResetCodeExpiryDays");
        }

        public void SendPasswordResetCode(string username)
        {
            //todo: send/resend the email containing the code
            //todo: resend the SAME reset code if it exists and hasn't expired else send a new one

            //_communicationService.SendMessageToCandidate(); SendPasswordResetCode

            throw new NotImplementedException();
        }
    }
}
