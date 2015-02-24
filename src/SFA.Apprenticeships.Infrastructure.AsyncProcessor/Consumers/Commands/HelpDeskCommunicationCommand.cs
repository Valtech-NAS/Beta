namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.Consumers.Commands
{
    using Application.Interfaces.Communications;
    using Domain.Interfaces.Messaging;

    public class HelpDeskCommunicationCommand : CommunicationCommand
    {
        public HelpDeskCommunicationCommand(IMessageBus messageBus)
            : base(messageBus)
        {
        }

        public override bool CanHandle(CommunicationRequest message)
        {
            return message.MessageType == MessageTypes.CandidateContactMessage;
        }

        public override void Handle(CommunicationRequest message)
        {
            SendEmailMessage(message);
        }
    }
}