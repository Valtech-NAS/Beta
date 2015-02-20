namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Repositories;
    using Interfaces.Communications;
    using Vacancy;
    using MessagingErrorCodes = Interfaces.Messaging.ErrorCodes;

    public class LegacyQueueApprenticeshipApplicationSubmittedStrategy : ISendApplicationSubmittedStrategy
    {
        private readonly IVacancyDataProvider<ApprenticeshipVacancyDetail> _vacancyDataProvider;
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ISendCandidateCommunicationStrategy _sendCandidateCommunicationStrategy;

        public LegacyQueueApprenticeshipApplicationSubmittedStrategy(IVacancyDataProvider<ApprenticeshipVacancyDetail> vacancyDataProvider, IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository, ICandidateReadRepository candidateReadRepository, ISendCandidateCommunicationStrategy queueCommunicationRequestStrategy)
        {
            _vacancyDataProvider = vacancyDataProvider;
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _candidateReadRepository = candidateReadRepository;
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
                new CommunicationToken(CommunicationTokens.ApplicationVacancyReference, vacancy.VacancyReference)
            };

            _sendCandidateCommunicationStrategy.Send(candidateId, MessageTypes.ApprenticeshipApplicationSubmitted, applicationTokens);
        }

        private ApprenticeshipApplicationDetail GetApplication(IEnumerable<CommunicationToken> tokens)
        {
            var applicationId = Guid.Parse(tokens.First(m => m.Key == CommunicationTokens.ApplicationId).Value);

            return _apprenticeshipApplicationReadRepository.Get(applicationId, true);
        }
    }
}
