namespace SFA.Apprenticeships.Application.Registration.Strategies
{
    using System;
    using Interfaces.Messaging;

    public class ResendAccountUnlockCodeStrategy : IResendAccountUnlockCodeStrategy
    {
        private readonly ICommunicationService _communicationService;

        public ResendAccountUnlockCodeStrategy(ICommunicationService communicationService)
        {
            _communicationService = communicationService;
        }

        public void ResendAccountUnlockCode(string username)
        {
            //_communicationService.SendMessageToCandidate(); SendAccountUnlockCode

            throw new NotImplementedException();
        }
    }
}
