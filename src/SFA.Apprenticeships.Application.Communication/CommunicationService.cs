namespace SFA.Apprenticeships.Application.Communication
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;
    using Interfaces.Messaging;
    using Strategies;

    //TODO: MG: design breaks OCP - may refactor
    public class CommunicationService : ICommunicationService
    {
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ISendAccountUnlockCodeStrategy _sendAccountUnlockCodeStrategy;
        private readonly ISendActivationCodeStrategy _sendActivationCodeStrategy;
        private readonly ISendApplicationSubmittedStrategy _sendApplicationSubmittedStrategy;
        private readonly ISendPasswordChangedStrategy _sendPasswordChangedStrategy;
        private readonly ISendPasswordResetCodeStrategy _sendPasswordResetCodeStrategy;

        public CommunicationService(ISendActivationCodeStrategy sendActivationCodeStrategy,
            ISendPasswordResetCodeStrategy sendPasswordResetCodeStrategy,
            ISendApplicationSubmittedStrategy sendApplicationSubmittedStrategy,
            ISendPasswordChangedStrategy sendPasswordChangedStrategy,
            ISendAccountUnlockCodeStrategy sendAccountUnlockCodeStrategy,
            ICandidateReadRepository candidateReadRepository)
        {
            _sendActivationCodeStrategy = sendActivationCodeStrategy;
            _sendPasswordResetCodeStrategy = sendPasswordResetCodeStrategy;
            _sendApplicationSubmittedStrategy = sendApplicationSubmittedStrategy;
            _sendPasswordChangedStrategy = sendPasswordChangedStrategy;
            _sendAccountUnlockCodeStrategy = sendAccountUnlockCodeStrategy;
            _candidateReadRepository = candidateReadRepository;
        }

        public void SendMessageToCandidate(Guid candidateId, CandidateMessageTypes messageType,
            IList<KeyValuePair<CommunicationTokens, string>> tokens)
        {
            Candidate candidate = _candidateReadRepository.Get(candidateId);
      
            switch (messageType)
            {
                case CandidateMessageTypes.SendActivationCode:
                    _sendActivationCodeStrategy.Send(candidate, messageType, tokens);
                    break;

                case CandidateMessageTypes.SendPasswordResetCode:
                    _sendPasswordResetCodeStrategy.Send(candidate, messageType, tokens);
                    break;

                case CandidateMessageTypes.SendAccountUnlockCode:
                    // TODO: NOTIMPL: get candidate, invoke strategy to send locked account / unlock code email to candidate
                    //var accountUnlockCode = user.AccountUnlockCode;
                    //_sendAccountUnlockCodeStrategy.Send();
                    break;

                case CandidateMessageTypes.ApplicationSubmitted:
                    // TODO: NOTIMPL: get candidate and application, invoke strategy to send application acknowledgement email to candidate
                    //_sendApplicationSubmittedStrategy.Send();
                    break;

                case CandidateMessageTypes.PasswordChanged:
                    _sendPasswordChangedStrategy.Send(candidate, messageType, tokens);
                    break;

                default:
                    throw new ArgumentOutOfRangeException("messageType");
            }
        }
    }
}