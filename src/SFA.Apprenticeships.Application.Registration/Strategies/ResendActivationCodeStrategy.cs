namespace SFA.Apprenticeships.Application.Registration.Strategies
{
    using System;
    using Interfaces.Messaging;

    public class ResendActivationCodeStrategy : IResendActivationCodeStrategy
    {
        private readonly ICommunicationService _communicationService;

        public ResendActivationCodeStrategy(ICommunicationService communicationService)
        {
            _communicationService = communicationService;
        }

        public void ResendActivationCode(string username)
        {
            //todo: send same code if exists and not expired
            //_communicationService.SendMessageToCandidate(); SendActivationCode
            // note: similar to SendActivationCode(Guid newCandidateId, Candidate candidate, string activationCode)

            throw new NotImplementedException();
        }
    }
}
