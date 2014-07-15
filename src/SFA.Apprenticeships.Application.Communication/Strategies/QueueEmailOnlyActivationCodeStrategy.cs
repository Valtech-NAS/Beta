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
            var request = new EmailRequest
            {
                ToEmail = candidate.RegistrationDetails.EmailAddress,
                TemplateName = templateName,
                Tokens = new[]
                {
                    new KeyValuePair<string, string>(
                        "Candidate.ActivationCode", activationCode)
                },
            };

            _bus.PublishMessage(request);
        }
    }
}
