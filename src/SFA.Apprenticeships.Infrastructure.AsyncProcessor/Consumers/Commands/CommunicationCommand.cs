namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.Consumers.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Communications;
    using Domain.Interfaces.Messaging;

    public abstract class CommunicationCommand
    {
        private readonly IMessageBus _messageBus;

        protected CommunicationCommand(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        protected void SendEmailMessage(CommunicationRequest message)
        {
            var toEmail = message.Tokens.First(t => t.Key == CommunicationTokens.RecipientEmailAddress).Value;
            var request = new EmailRequest
            {
                ToEmail = toEmail,
                MessageType = message.MessageType,
                Tokens = GetMessageTokens(message),
            };

            _messageBus.PublishMessage(request);
        }

        protected void SendSmsMessage(CommunicationRequest message)
        {
            var toMobile = message.Tokens.First(t => t.Key == CommunicationTokens.CandidateMobileNumber).Value;
            var request = new SmsRequest
            {
                ToNumber = toMobile,
                MessageType = message.MessageType,
                Tokens = GetMessageTokens(message),
            };

            _messageBus.PublishMessage(request);
        }

        private static IEnumerable<CommunicationToken> GetMessageTokens(CommunicationRequest message)
        {
            return message.Tokens
                .Where(t => t.Key != CommunicationTokens.RecipientEmailAddress && t.Key != CommunicationTokens.CandidateMobileNumber);
        }

        public abstract bool CanHandle(CommunicationRequest message);

        public abstract void Handle(CommunicationRequest message);
    }
}
