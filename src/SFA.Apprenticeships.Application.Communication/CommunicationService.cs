namespace SFA.Apprenticeships.Application.Communication
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Communication;
    using Interfaces.Communications;
    using Interfaces.Logging;
    using Strategies;

    public class CommunicationService : ICommunicationService
    {
        private readonly ILogService _logger;

        private readonly ISendApplicationSubmittedStrategy _sendApplicationSubmittedStrategy;
        private readonly ISendTraineeshipApplicationSubmittedStrategy _sendTraineeshipApplicationSubmittedStrategy;
        private readonly ISendCandidateCommunicationStrategy _sendCandidateCommunicationStrategy;

        public CommunicationService(ISendApplicationSubmittedStrategy sendApplicationSubmittedStrategy,
            ISendTraineeshipApplicationSubmittedStrategy sendTraineeshipApplicationSubmittedStrategy, 
            ISendCandidateCommunicationStrategy sendCandidateCommunicationStrategy, ILogService logger)
        {
            _sendApplicationSubmittedStrategy = sendApplicationSubmittedStrategy;
            _sendTraineeshipApplicationSubmittedStrategy = sendTraineeshipApplicationSubmittedStrategy;
            _sendCandidateCommunicationStrategy = sendCandidateCommunicationStrategy;
            _logger = logger;
        }

        public void SendMessageToCandidate(Guid candidateId, MessageTypes messageType, IEnumerable<CommunicationToken> tokens)
        {
            _logger.Debug("CommunicationService called to send a message of type {0} to candidate with Id={1}", messageType, candidateId);

            switch (messageType)
            {
                case MessageTypes.ApprenticeshipApplicationSubmitted:
                    _sendApplicationSubmittedStrategy.Send(candidateId, tokens);
                    break;

                case MessageTypes.TraineeshipApplicationSubmitted:
                    _sendTraineeshipApplicationSubmittedStrategy.Send(candidateId, tokens);
                    break;

                case MessageTypes.SendActivationCode:
                case MessageTypes.SendPasswordResetCode:
                case MessageTypes.SendAccountUnlockCode:
                case MessageTypes.PasswordChanged:
                case MessageTypes.SendMobileVerificationCode:
                    _sendCandidateCommunicationStrategy.Send(candidateId, messageType, tokens);
                    break;

                default:
                    throw new ArgumentOutOfRangeException("messageType");
            }
        }

        public void SendContactMessage(Guid? userId, MessageTypes messageType, IEnumerable<CommunicationToken> tokens)
        {
            //todo: 1.6: send contact message to helpdesk. user id could be a candidate user (if logged in) or another user type (future)
            //delegate to strategy to read candidate info (if necessary) so tokens can be added from user's account info
            //messageType == MessageTypes.CandidateContactMessage
        }
    }
}
