namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.Consumers.Commands
{
    using Application.Interfaces.Communications;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;

    public class CandidateCommunicationCommand : CommunicationCommand
    {
        private readonly ICandidateReadRepository _candidateReadRepository;

        public CandidateCommunicationCommand(IMessageBus messageBus, ICandidateReadRepository candidateReadRepository)
            : base(messageBus)
        {
            _candidateReadRepository = candidateReadRepository;
        }

        public override bool CanHandle(CommunicationRequest message)
        {
            return message.MessageType != MessageTypes.CandidateContactMessage;
        }

        public override void Handle(CommunicationRequest message)
        {
            var candidateId = message.EntityId.Value;

            var candidate = _candidateReadRepository.Get(candidateId);

            // note, some messages are mandatory - determined by type
            var isOptionalMessageType = message.MessageType == MessageTypes.TraineeshipApplicationSubmitted ||
                                        message.MessageType == MessageTypes.ApprenticeshipApplicationSubmitted;

            // note, some messages are channel specific
            var isSmsOnly = message.MessageType == MessageTypes.SendMobileVerificationCode;

            if ((!isOptionalMessageType || candidate.CommunicationPreferences.AllowEmail) && !isSmsOnly)
            {
                SendEmailMessage(message);
            }

            if (!isOptionalMessageType || candidate.CommunicationPreferences.AllowMobile)
            {
                SendSmsMessage(message);
            }
        }
    }
}
