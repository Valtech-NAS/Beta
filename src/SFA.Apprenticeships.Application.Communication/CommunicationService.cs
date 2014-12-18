namespace SFA.Apprenticeships.Application.Communication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;
    using Interfaces.Messaging;
    using NLog;
    using Strategies;

    //TODO: MG: design breaks OCP - may refactor
    public class CommunicationService : ICommunicationService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly ISendAccountUnlockCodeStrategy _sendAccountUnlockCodeStrategy;
        private readonly ISendActivationCodeStrategy _sendActivationCodeStrategy;
        private readonly ISendApplicationSubmittedStrategy _sendApplicationSubmittedStrategy;
        private readonly ISendTraineeshipApplicationSubmittedStrategy _sendTraineeshipApplicationSubmittedStrategy;
        private readonly ISendPasswordChangedStrategy _sendPasswordChangedStrategy;
        private readonly ISendPasswordResetCodeStrategy _sendPasswordResetCodeStrategy;

        private readonly IApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly ITraineeshipApplicationReadRepository _traineeshipApplicationReadRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;

        public CommunicationService(ISendActivationCodeStrategy sendActivationCodeStrategy,
            ISendPasswordResetCodeStrategy sendPasswordResetCodeStrategy,
            ISendApplicationSubmittedStrategy sendApplicationSubmittedStrategy,
            ISendTraineeshipApplicationSubmittedStrategy sendTraineeshipApplicationSubmittedStrategy,
            ISendPasswordChangedStrategy sendPasswordChangedStrategy,
            ISendAccountUnlockCodeStrategy sendAccountUnlockCodeStrategy,
            ICandidateReadRepository candidateReadRepository,
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository)
            ITraineeshipApplicationReadRepository traineeshipApplicationReadRepository)
        {
            _sendActivationCodeStrategy = sendActivationCodeStrategy;
            _sendPasswordResetCodeStrategy = sendPasswordResetCodeStrategy;
            _sendApplicationSubmittedStrategy = sendApplicationSubmittedStrategy;
            _sendTraineeshipApplicationSubmittedStrategy = sendTraineeshipApplicationSubmittedStrategy;
            _sendPasswordChangedStrategy = sendPasswordChangedStrategy;
            _sendAccountUnlockCodeStrategy = sendAccountUnlockCodeStrategy;
            _candidateReadRepository = candidateReadRepository;
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _traineeshipApplicationReadRepository = traineeshipApplicationReadRepository;
        }

        public void SendMessageToCandidate(Guid candidateId, CandidateMessageTypes messageType,
            IList<KeyValuePair<CommunicationTokens, string>> tokens)
        {
            var sentEmail = true;

            Logger.Debug("Calling CommunicationService to send a message of type {0} to candidate with Id={1}",
                messageType, candidateId);

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

                case CandidateMessageTypes.ApprenticeshipApplicationSubmitted:
                    if ((sentEmail = candidate.CommunicationPreferences.AllowEmail) == true)
                    {
                        var application = GetApprenticeshipApplicationDetail(tokens);

                        _sendApplicationSubmittedStrategy.Send(candidate, application, messageType, tokens);
                    }
                    break;

                case CandidateMessageTypes.TraineeshipApplicationSubmitted:
                    if ((sentEmail = candidate.CommunicationPreferences.AllowEmail) == true)
                    {
                        var application = GetTraineeshipApplicationDetail(tokens);

                        _sendTraineeshipApplicationSubmittedStrategy.Send(candidate, application, messageType, tokens);
                    }
                    break;

                case CandidateMessageTypes.PasswordChanged:
                    _sendPasswordChangedStrategy.Send(candidate, messageType, tokens);
                    break;

                default:
                    throw new ArgumentOutOfRangeException("messageType");
            }

            if (!sentEmail)
            {
                Logger.Debug("NOT calling CommunicationService to send a message of type {0} to candidate with Id={1} (candidate has AllowEmail = false set)", messageType, candidateId);
            }
        }

        private ApprenticeshipApplicationDetail GetApprenticeshipApplicationDetail(IEnumerable<KeyValuePair<CommunicationTokens, string>> tokens)
        {
            var applicationId = Guid.Parse(tokens.First(m => m.Key == CommunicationTokens.ApplicationId).Value);

            return _apprenticeshipApplicationReadRepository.Get(applicationId);
        }

        private TraineeshipApplicationDetail GetTraineeshipApplicationDetail(IEnumerable<KeyValuePair<CommunicationTokens, string>> tokens)
        {
            var applicationId = Guid.Parse(tokens.First(m => m.Key == CommunicationTokens.ApplicationId).Value);

            return _traineeshipApplicationReadRepository.Get(applicationId);
        }
    }
}
