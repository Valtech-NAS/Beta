namespace SFA.Apprenticeships.Application.Communication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;
    using Interfaces.Messaging;
    using Strategies;

    //TODO: MG: design breaks OCP - may refactor
    public class CommunicationService : ICommunicationService
    {
        private readonly IApplicationReadRepository _applicationReadRepository;
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
            ICandidateReadRepository candidateReadRepository,
            IApplicationReadRepository applicationReadRepository)
        {
            _sendActivationCodeStrategy = sendActivationCodeStrategy;
            _sendPasswordResetCodeStrategy = sendPasswordResetCodeStrategy;
            _sendApplicationSubmittedStrategy = sendApplicationSubmittedStrategy;
            _sendPasswordChangedStrategy = sendPasswordChangedStrategy;
            _sendAccountUnlockCodeStrategy = sendAccountUnlockCodeStrategy;
            _candidateReadRepository = candidateReadRepository;
            _applicationReadRepository = applicationReadRepository;
        }

        public void SendMessageToCandidate(Guid candidateId, CandidateMessageTypes messageType,
            IList<KeyValuePair<CommunicationTokens, string>> tokens)
        {
            var candidate = _candidateReadRepository.Get(candidateId);

            switch (messageType)
            {
                case CandidateMessageTypes.SendActivationCode:
                    _sendActivationCodeStrategy.Send(candidate, messageType, tokens);
                    break;
                case CandidateMessageTypes.SendPasswordResetCode:
                    _sendPasswordResetCodeStrategy.Send(candidate, messageType, tokens);
                    break;
                case CandidateMessageTypes.SendAccountUnlockCode:
                    _sendAccountUnlockCodeStrategy.Send(candidate, messageType, tokens);
                    break;
                case CandidateMessageTypes.ApplicationSubmitted:
                    var application = GetApplicationDetail(tokens);

                    _sendApplicationSubmittedStrategy.Send(candidate, application, messageType, tokens);
                    break;

                case CandidateMessageTypes.PasswordChanged:
                    _sendPasswordChangedStrategy.Send(candidate, messageType, tokens);
                    break;

                default:
                    throw new ArgumentOutOfRangeException("messageType");
            }
        }

        private ApplicationDetail GetApplicationDetail(IEnumerable<KeyValuePair<CommunicationTokens, string>> tokens)
        {
            var applicationId = Guid.Parse(tokens.First(m => m.Key == CommunicationTokens.ApplicationId).Value);

            return _applicationReadRepository.Get(applicationId);
        }
    }
}
