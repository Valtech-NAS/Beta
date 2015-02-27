namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies.Traineeships;
    using Domain.Interfaces.Repositories;
    using Interfaces.Communications;
    using Vacancy;
    using MessagingErrorCodes = Interfaces.Messaging.ErrorCodes;

    public class LegacyQueueTraineeshipApplicationSubmittedStrategy : ISendTraineeshipApplicationSubmittedStrategy
    {
        private readonly IVacancyDataProvider<TraineeshipVacancyDetail> _vacancyDataProvider;
        private readonly ITraineeshipApplicationReadRepository _traineeshipApplicationReadRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ISendCandidateCommunicationStrategy _sendCandidateCommunicationStrategy;

        public LegacyQueueTraineeshipApplicationSubmittedStrategy(IVacancyDataProvider<TraineeshipVacancyDetail> vacancyDataProvider, ICandidateReadRepository candidateReadRepository, ITraineeshipApplicationReadRepository traineeshipApplicationReadRepository, ISendCandidateCommunicationStrategy queueCommunicationRequestStrategy)
        {
            _vacancyDataProvider = vacancyDataProvider;
            _candidateReadRepository = candidateReadRepository;
            _traineeshipApplicationReadRepository = traineeshipApplicationReadRepository;
            _sendCandidateCommunicationStrategy = queueCommunicationRequestStrategy;
        }

        public void Send(Guid candidateId, IEnumerable<CommunicationToken> tokens)
        {
            var candidate = _candidateReadRepository.Get(candidateId);

            var application = GetApplication(tokens);
            var vacancy = _vacancyDataProvider.GetVacancyDetails(application.Vacancy.Id, true);

            var applicationTokens = new[]
            {
                new CommunicationToken(CommunicationTokens.CandidateFirstName, candidate.RegistrationDetails.FirstName), 
                new CommunicationToken(CommunicationTokens.ApplicationVacancyTitle, vacancy.Title),
                new CommunicationToken(CommunicationTokens.ApplicationVacancyReference, vacancy.VacancyReference),
                new CommunicationToken(CommunicationTokens.ApplicationVacancyEmployerName, vacancy.EmployerName),
                new CommunicationToken(CommunicationTokens.ProviderContact, vacancy.Contact)
            };
            
            _sendCandidateCommunicationStrategy.Send(candidateId, MessageTypes.TraineeshipApplicationSubmitted, applicationTokens);
        }

        private TraineeshipApplicationDetail GetApplication(IEnumerable<CommunicationToken> tokens)
        {
            var applicationId = Guid.Parse(tokens.First(m => m.Key == CommunicationTokens.ApplicationId).Value);

            return _traineeshipApplicationReadRepository.Get(applicationId, true);
        }
    }
}
