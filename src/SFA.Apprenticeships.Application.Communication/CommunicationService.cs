namespace SFA.Apprenticeships.Application.Communication
{
    using System;
    using System.Collections.Generic;
    using Domain.Interfaces.Repositories;
    using Interfaces.Messaging;
    using Strategies;

    public class CommunicationService : ICommunicationService
    {
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IUserReadRepository _userReadRepository;
        private readonly ISendActivationCodeStrategy _sendActivationCodeStrategy;

        public CommunicationService(ICandidateReadRepository candidateReadRepository, IUserReadRepository userReadRepository, ISendActivationCodeStrategy sendActivationCodeStrategy)
        {
            _candidateReadRepository = candidateReadRepository;
            _userReadRepository = userReadRepository;
            _sendActivationCodeStrategy = sendActivationCodeStrategy;
            //todo: other strategies go here...
        }

        public void SendMessageToCandidate(Guid candidateId, CandidateMessageTypes messageType, IEnumerable<KeyValuePair<string, string>> tokens)
        {
            switch (messageType)
            {
                case CandidateMessageTypes.SendActivationCode:
                    //todo: get candidate, invoke strategy to send activation code email / SMS to candidate
                    var candidate = _candidateReadRepository.Get(candidateId);
                    var user = _userReadRepository.Get(candidate.Username);
                    var activationCode = user.ActivationCode;
                    _sendActivationCodeStrategy.Send(candidate, activationCode);
                    break;

                case CandidateMessageTypes.SendPasswordCode:
                    //todo: get candidate, invoke strategy to send forgotten password email to candidate
                    break;

                case CandidateMessageTypes.ApplicationSubmitted:
                    //todo: get candidate, invoke strategy to send application acknowledgement email to candidate
                    break;

                case CandidateMessageTypes.PasswordChanged:
                    //todo: get candidate, invoke strategy to send password changed email to candidate
                    break;
                    
                default:
                    throw new ArgumentOutOfRangeException("messageType");
            }
        }
    }
}
