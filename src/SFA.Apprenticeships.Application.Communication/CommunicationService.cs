namespace SFA.Apprenticeships.Application.Communication
{
    using System;
    using System.Collections.Generic;
    using Domain.Interfaces.Repositories;
    using Interfaces.Messaging;
    using Strategies;

    //TODO: MG: design breaks OCP - may refactor
    public class CommunicationService : ICommunicationService
    {
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IUserReadRepository _userReadRepository;
        private readonly ISendActivationCodeStrategy _sendActivationCodeStrategy;
        private readonly ISendPasswordResetCodeStrategy _sendPasswordResetCodeStrategy;
        private readonly ISendAccountUnlockCodeStrategy _sendAccountUnlockCodeStrategy;
        private readonly ISendApplicationSubmittedStrategy _sendApplicationSubmittedStrategy;
        private readonly ISendPasswordChangedStrategy _sendPasswordChangedStrategy;

        public CommunicationService(ICandidateReadRepository candidateReadRepository,
            IUserReadRepository userReadRepository, ISendActivationCodeStrategy sendActivationCodeStrategy,
            ISendPasswordResetCodeStrategy sendPasswordResetCodeStrategy, ISendApplicationSubmittedStrategy sendApplicationSubmittedStrategy,
            ISendPasswordChangedStrategy sendPasswordChangedStrategy, ISendAccountUnlockCodeStrategy sendAccountUnlockCodeStrategy)
        {
            _candidateReadRepository = candidateReadRepository;
            _userReadRepository = userReadRepository;
            _sendActivationCodeStrategy = sendActivationCodeStrategy;
            _sendPasswordResetCodeStrategy = sendPasswordResetCodeStrategy;
            _sendApplicationSubmittedStrategy = sendApplicationSubmittedStrategy;
            _sendPasswordChangedStrategy = sendPasswordChangedStrategy;
            _sendAccountUnlockCodeStrategy = sendAccountUnlockCodeStrategy;
        }

        public void SendMessageToCandidate(Guid candidateId, CandidateMessageTypes messageType, IList<KeyValuePair<CommunicationTokens, string>> tokens)
        {
            var templateName = GetTemplateName(messageType);

            switch (messageType)
            {
                case CandidateMessageTypes.SendActivationCode:
                    var candidate = _candidateReadRepository.Get(candidateId);
                    var user = _userReadRepository.Get(candidate.RegistrationDetails.EmailAddress);
                    var activationCode = user.ActivationCode;

                    _sendActivationCodeStrategy.Send(templateName, candidate, activationCode);
                    break;

                case CandidateMessageTypes.SendPasswordResetCode:
                    // TODO: NOTIMPL: get candidate, invoke strategy to send forgotten password / reset code email to candidate
                    //var passwordResetCode = user.PasswordResetCode;
                    //_sendPasswordResetCodeStrategy.Send();
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
                    // TODO: NOTIMPL: get candidate, invoke strategy to send password changed email to candidate
                    //_sendPasswordChangedStrategy.Send();
                    break;

                default:
                    throw new ArgumentOutOfRangeException("messageType");
            }
        }

        #region Helpers
        private static string GetTemplateName(Enum messageType)
        {
            var enumType = messageType.GetType();

            return string.Format("{0}.{1}", enumType.Name, Enum.GetName(enumType, messageType));
        }
        #endregion
    }
}
