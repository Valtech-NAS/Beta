namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Messaging;
    using Interfaces.Messaging;

    public class QueueEmailOnlyActivationCodeStrategy : ISendActivationCodeStrategy
    {
        private readonly IMessageBus _bus;

        public QueueEmailOnlyActivationCodeStrategy(IMessageBus bus)
        {
            _bus = bus;
        }

        public void Send(string templateName, Candidate candidate, string activationCode)
        {
            var emailAddress = candidate.RegistrationDetails.EmailAddress;

            var request = new EmailRequest
            {
                ToEmail = emailAddress,
                TemplateName = templateName,
                Tokens = CreateTokens(activationCode, emailAddress),
            };

            _bus.PublishMessage(request);
        }

        private static IEnumerable<KeyValuePair<string, string>> CreateTokens(string activationCode, string emailAddress)
        {
            return new[]
            {
                new KeyValuePair<string, string>(
                    "Candidate.ActivationCode", activationCode),
                new KeyValuePair<string, string>(
                    "Candidate.EmailAddress", emailAddress),
            };
        }
    }
}
