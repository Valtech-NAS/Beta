namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System.Collections.Generic;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Messaging;
    using Interfaces.Messaging;
    using Vacancy;

    public class LegacyQueueTraineeshipApplicationSubmittedStrategy : ISendTraineeshipApplicationSubmittedStrategy
    {
        private readonly IMessageBus _messageBus;
        private readonly IVacancyDataProvider<TraineeshipVacancyDetail> _vacancyDataProvider;

        public LegacyQueueTraineeshipApplicationSubmittedStrategy(IMessageBus messageBus, IVacancyDataProvider<TraineeshipVacancyDetail> vacancyDataProvider)
        {
            _messageBus = messageBus;
            _vacancyDataProvider = vacancyDataProvider;
        }

        public void Send(Candidate candidate, TraineeshipApplicationDetail traineeshipApplicationDetail, CandidateMessageTypes messageType,
            IEnumerable<KeyValuePair<CommunicationTokens, string>> tokens)
        {
            var reference = GetVacancyReference(traineeshipApplicationDetail.Vacancy.Id);

            var applicationTokens = new[]
            {
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.CandidateFirstName, candidate.RegistrationDetails.FirstName), 
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.ApplicationVacancyTitle,
                    traineeshipApplicationDetail.Vacancy.Title),
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.ApplicationVacancyReference, reference)
            };

            var request = new EmailRequest
            {
                ToEmail = candidate.RegistrationDetails.EmailAddress,
                MessageType = messageType,
                Tokens = applicationTokens
            };

            _messageBus.PublishMessage(request);
        }

        private string GetVacancyReference(int id)
        {
            var vacancyDetails = _vacancyDataProvider.GetVacancyDetails(id);

            if (vacancyDetails == null)
            {
                throw new CustomException(
                    "Vacancy not found with ID {0}.", ErrorCodes.VacancyNotFoundError, id);
            }

            return vacancyDetails.VacancyReference;
        }
    }
}