namespace SFA.Apprenticeships.Application.Communication
{
    using System;
    using System.Collections.Generic;
    using Interfaces.Messaging;
    using NLog;
    using Strategies;

    public class CommunicationService : ICommunicationService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ISendAccountUnlockCodeStrategy _sendAccountUnlockCodeStrategy;
        private readonly ISendActivationCodeStrategy _sendActivationCodeStrategy;
        private readonly ISendApplicationSubmittedStrategy _sendApplicationSubmittedStrategy;
        private readonly ISendTraineeshipApplicationSubmittedStrategy _sendTraineeshipApplicationSubmittedStrategy;
        private readonly ISendPasswordChangedStrategy _sendPasswordChangedStrategy;
        private readonly ISendPasswordResetCodeStrategy _sendPasswordResetCodeStrategy;

        public CommunicationService(ISendActivationCodeStrategy sendActivationCodeStrategy,
            ISendPasswordResetCodeStrategy sendPasswordResetCodeStrategy,
            ISendApplicationSubmittedStrategy sendApplicationSubmittedStrategy,
            ISendTraineeshipApplicationSubmittedStrategy sendTraineeshipApplicationSubmittedStrategy,
            ISendPasswordChangedStrategy sendPasswordChangedStrategy,
            ISendAccountUnlockCodeStrategy sendAccountUnlockCodeStrategy)
        {
            _sendActivationCodeStrategy = sendActivationCodeStrategy;
            _sendPasswordResetCodeStrategy = sendPasswordResetCodeStrategy;
            _sendApplicationSubmittedStrategy = sendApplicationSubmittedStrategy;
            _sendTraineeshipApplicationSubmittedStrategy = sendTraineeshipApplicationSubmittedStrategy;
            _sendPasswordChangedStrategy = sendPasswordChangedStrategy;
            _sendAccountUnlockCodeStrategy = sendAccountUnlockCodeStrategy;
        }

        public void SendMessageToCandidate(Guid candidateId, MessageTypes messageType, IList<KeyValuePair<CommunicationTokens, string>> tokens)
        {
            Logger.Debug("CommunicationService called to send a message of type {0} to candidate with Id={1}", messageType, candidateId);

            //TODO: MG: design breaks OCP - may refactor as all signatures are same
            switch (messageType)
            {
                case MessageTypes.SendActivationCode:
                    _sendActivationCodeStrategy.Send(candidateId, tokens);
                    break;

                case MessageTypes.SendPasswordResetCode:
                    _sendPasswordResetCodeStrategy.Send(candidateId, tokens);
                    break;

                case MessageTypes.SendAccountUnlockCode:
                    _sendAccountUnlockCodeStrategy.Send(candidateId, tokens);
                    break;

                case MessageTypes.ApprenticeshipApplicationSubmitted:
                    _sendApplicationSubmittedStrategy.Send(candidateId, tokens);
                    break;

                case MessageTypes.TraineeshipApplicationSubmitted:
                    _sendTraineeshipApplicationSubmittedStrategy.Send(candidateId, tokens);
                    break;

                case MessageTypes.PasswordChanged:
                    _sendPasswordChangedStrategy.Send(candidateId, tokens);
                    break;

                default:
                    throw new ArgumentOutOfRangeException("messageType");
            }
        }
    }
}
