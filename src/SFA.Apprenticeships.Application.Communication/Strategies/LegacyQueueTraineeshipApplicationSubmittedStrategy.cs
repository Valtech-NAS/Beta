namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Vacancies.Traineeships;
    using Domain.Interfaces.Repositories;
    using Interfaces.Messaging;
    using Vacancy;
    using MessagingErrorCodes = Interfaces.Messaging.ErrorCodes;

    public class LegacyQueueTraineeshipApplicationSubmittedStrategy : ISendTraineeshipApplicationSubmittedStrategy
    {
        private readonly IVacancyDataProvider<TraineeshipVacancyDetail> _vacancyDataProvider;
        private readonly ITraineeshipApplicationReadRepository _traineeshipApplicationReadRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IQueueCommunicationRequestStrategy _queueCommunicationRequestStrategy;

        public LegacyQueueTraineeshipApplicationSubmittedStrategy(IVacancyDataProvider<TraineeshipVacancyDetail> vacancyDataProvider, ICandidateReadRepository candidateReadRepository, ITraineeshipApplicationReadRepository traineeshipApplicationReadRepository, IQueueCommunicationRequestStrategy queueCommunicationRequestStrategy)
        {
            _vacancyDataProvider = vacancyDataProvider;
            _candidateReadRepository = candidateReadRepository;
            _traineeshipApplicationReadRepository = traineeshipApplicationReadRepository;
            _queueCommunicationRequestStrategy = queueCommunicationRequestStrategy;
        }

        public void Send(Guid candidateId, IEnumerable<KeyValuePair<CommunicationTokens, string>> tokens)
        {
            var candidate = _candidateReadRepository.Get(candidateId);

            var application = GetApplication(tokens);
            var vacancy = _vacancyDataProvider.GetVacancyDetails(application.Vacancy.Id, true);

            var applicationTokens = new[]
            {
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.CandidateFirstName, candidate.RegistrationDetails.FirstName), 
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.ApplicationVacancyTitle, vacancy.Title),
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.ApplicationVacancyReference, vacancy.VacancyReference),
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.ProviderContact, vacancy.Contact)
            };
            
            _queueCommunicationRequestStrategy.Queue(candidateId, MessageTypes.TraineeshipApplicationSubmitted, applicationTokens);
        }

        private TraineeshipApplicationDetail GetApplication(IEnumerable<KeyValuePair<CommunicationTokens, string>> tokens)
        {
            var applicationId = Guid.Parse(tokens.First(m => m.Key == CommunicationTokens.ApplicationId).Value);

            return _traineeshipApplicationReadRepository.Get(applicationId, true);
        }
    }
}
