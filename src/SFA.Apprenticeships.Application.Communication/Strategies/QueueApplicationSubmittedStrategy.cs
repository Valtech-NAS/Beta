namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System.Collections.Generic;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Messaging;
    using Interfaces.Messaging;

    public class QueueApplicationSubmittedStrategy : ISendApplicationSubmittedStrategy
    {
        private readonly IMessageBus _bus;

        public QueueApplicationSubmittedStrategy(IMessageBus bus)
        {
            _bus = bus;
        }

        public void Send(Candidate candidate, ApplicationDetail applicationDetail, CandidateMessageTypes messageType,
            IEnumerable<KeyValuePair<CommunicationTokens, string>> tokens)
        {
            var reference = string.Format("Vacancy Id {0}", applicationDetail.Vacancy.Id); //TODO Determine what the vacancy reference is

            var applicationTokens = new[]
            {
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.CandidateFirstName, candidate.RegistrationDetails.FirstName), 
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.ApplicationVacancyTitle,
                    applicationDetail.Vacancy.Title),
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.ApplicationVacancyReference, reference)
            };

            var request = new EmailRequest
            {
                ToEmail = candidate.RegistrationDetails.EmailAddress,
                MessageType = messageType,
                Tokens = applicationTokens,
            };

            _bus.PublishMessage(request);
        }
    }
}