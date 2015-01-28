namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Repositories;
    using Interfaces.Messaging;
    using Vacancy;
    using MessagingErrorCodes = Interfaces.Messaging.ErrorCodes;

    public class LegacyQueueApprenticeshipApplicationSubmittedStrategy : ISendApplicationSubmittedStrategy
    {
        private readonly IVacancyDataProvider<ApprenticeshipVacancyDetail> _vacancyDataProvider;
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IQueueCommunicationRequestStrategy _queueCommunicationRequestStrategy;

        public LegacyQueueApprenticeshipApplicationSubmittedStrategy(IVacancyDataProvider<ApprenticeshipVacancyDetail> vacancyDataProvider, IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository, ICandidateReadRepository candidateReadRepository, IQueueCommunicationRequestStrategy queueCommunicationRequestStrategy)
        {
            _vacancyDataProvider = vacancyDataProvider;
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _candidateReadRepository = candidateReadRepository;
            _queueCommunicationRequestStrategy = queueCommunicationRequestStrategy;
        }

        public void Send(Guid candidateId, IEnumerable<KeyValuePair<CommunicationTokens, string>> tokens)
        {
            var candidate = _candidateReadRepository.Get(candidateId);

            var application = GetApplication(tokens);
            var vacancy = _vacancyDataProvider.GetVacancyDetails(application.Vacancy.Id);

            if (vacancy == null) //todo: add flag to repo operation
            {
                throw new CustomException("Vacancy not found with ID {0}.",
                    MessagingErrorCodes.VacancyNotFoundError,
                    application.Vacancy.Id);
            }

            var applicationTokens = new[]
            {
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.CandidateFirstName, candidate.RegistrationDetails.FirstName), 
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.ApplicationVacancyTitle, vacancy.Title),
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.ApplicationVacancyReference, vacancy.VacancyReference)
            };

            _queueCommunicationRequestStrategy.Queue(candidateId, MessageTypes.ApprenticeshipApplicationSubmitted, applicationTokens);
        }

        private ApprenticeshipApplicationDetail GetApplication(IEnumerable<KeyValuePair<CommunicationTokens, string>> tokens)
        {
            var applicationId = Guid.Parse(tokens.First(m => m.Key == CommunicationTokens.ApplicationId).Value);

            return _apprenticeshipApplicationReadRepository.Get(applicationId, true);
        }
    }
}
