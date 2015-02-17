namespace SFA.Apprenticeships.Infrastructure.Communication.UnitTests.Email.EmailMessageFormatters
{
    using System.Collections.Generic;
    using Application.Interfaces.Communications;

    public class EmailRequestBuilder
    {
        public const string ToEmail = "test@test.com";

        private MessageTypes _messageType;
        private IEnumerable<CommunicationToken> _tokens = new List<CommunicationToken>(0);

        public EmailRequestBuilder WithMessageType(MessageTypes messageType)
        {
            _messageType = messageType;
            return this;
        }

        public EmailRequestBuilder WithTokens(IEnumerable<CommunicationToken> tokens)
        {
            _tokens = tokens;
            return this;
        }

        public EmailRequest Build()
        {
            var emailRequest = new EmailRequest
            {
                MessageType = _messageType,
                ToEmail = ToEmail,
                Tokens = _tokens
            };
            return emailRequest;
        }
    }
}