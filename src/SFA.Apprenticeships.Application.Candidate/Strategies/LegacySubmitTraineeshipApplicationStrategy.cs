namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using NLog;
    using SFA.Apprenticeships.Application.Interfaces.Messaging;
    using SFA.Apprenticeships.Application.Vacancy;
    using SFA.Apprenticeships.Domain.Entities.Applications;
    using SFA.Apprenticeships.Domain.Entities.Candidates;
    using SFA.Apprenticeships.Domain.Entities.Exceptions;
    using SFA.Apprenticeships.Domain.Entities.Users;
    using SFA.Apprenticeships.Domain.Entities.Vacancies;
    using SFA.Apprenticeships.Domain.Entities.Vacancies.Traineeships;
    using SFA.Apprenticeships.Domain.Interfaces.Messaging;
    using SFA.Apprenticeships.Domain.Interfaces.Repositories;

    public class LegacySubmitTraineeshipApplicationStrategy : ISubmitTraineeshipApplicationStrategy
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ICommunicationService _communicationService;
        private readonly IMessageBus _messageBus;
        private readonly IVacancyDataProvider _vacancyDataProvider;
        private readonly ITraineeshipApplicationWriteRepository _traineeshipApplicationWriteRepository;
        private readonly ITraineeshipApplicationReadRepository _traineeshipApplicationReadRepository;

        public LegacySubmitTraineeshipApplicationStrategy(IMessageBus messageBus,
            ICommunicationService communicationService,
            IVacancyDataProvider vacancyDataProvider,
            ICandidateReadRepository candidateReadRepository, 
            ITraineeshipApplicationWriteRepository traineeshipApplicationWriteRepository, 
            ITraineeshipApplicationReadRepository traineeshipApplicationReadRepository)
        {
            _messageBus = messageBus;
            _communicationService = communicationService;
            _vacancyDataProvider = vacancyDataProvider;
            _candidateReadRepository = candidateReadRepository;
            _traineeshipApplicationWriteRepository = traineeshipApplicationWriteRepository;
            _traineeshipApplicationReadRepository = traineeshipApplicationReadRepository;
        }

        public void SubmitApplication(Guid candidateId, int vacancyId,
            TraineeshipApplicationDetail traineeshipApplicationDetail)
        {
            var vacancyDetails = _vacancyDataProvider.GetVacancyDetails(vacancyId);

            if (vacancyDetails == null)
            {
                // TODO: is this possible?
            }

            var candidate = _candidateReadRepository.Get(candidateId);
            var applicationDetail = CreateApplicationDetail(candidate, vacancyDetails);

            _traineeshipApplicationWriteRepository.Save(applicationDetail);
        }

        public void SubmitApplication(Guid applicationId)
        {
            // TODO: use the new repos
            var applicationDetail = _traineeshipApplicationReadRepository.Get(applicationId, true);

            try
            {
                PublishMessage(applicationDetail);
                // NotifyCandidate(applicationDetail.CandidateId, applicationDetail.EntityId.ToString());
            }
            catch (Exception ex)
            {
                Logger.Debug("SubmitApplicationRequest could not be queued for ApplicationId={0}", applicationId);

                throw new CustomException("SubmitApplicationRequest could not be queued", ex,
                    ErrorCodes.ApplicationQueuingError);
            }
        }

        private void PublishMessage(TraineeshipApplicationDetail traineeshipApplicationDetail)
        {
            //TODO: use a new message?
            var message = new SubmitTraineeshipApplicationRequest
            {
                ApplicationId = traineeshipApplicationDetail.EntityId
            };

            _messageBus.PublishMessage(message);
        }

        private void NotifyCandidate(Guid candidateId, string applicationId)
        {
            _communicationService.SendMessageToCandidate(candidateId, CandidateMessageTypes.ApplicationSubmitted,
                new[]
                {
                    new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.ApplicationId, applicationId)
                });
        }

        private static TraineeshipApplicationDetail CreateApplicationDetail(Candidate candidate,
            VacancyDetail vacancyDetails)
        {
            return new TraineeshipApplicationDetail
            {
                EntityId = Guid.NewGuid(),
                Status = ApplicationStatuses.Draft,
                DateCreated = DateTime.Now,
                CandidateId = candidate.EntityId,
                // TODO: US354: AG: better way to clone? http://stackoverflow.com/questions/5713556/copy-object-to-object-with-automapper
                CandidateDetails = Mapper.Map<RegistrationDetails, RegistrationDetails>(candidate.RegistrationDetails),
                Vacancy = new TraineeshipSummary
                {
                    Id = vacancyDetails.Id,
                    Title = vacancyDetails.Title,
                    EmployerName = vacancyDetails.IsEmployerAnonymous
                        ? vacancyDetails.AnonymousEmployerName
                        : vacancyDetails.EmployerName,
                    ClosingDate = vacancyDetails.ClosingDate,
                    Description = vacancyDetails.Description,
                    Location = null // NOTE: no equivalent in legacy vacancy details.
                },
                // Populate traineeshipApplication template with candidate's most recent information.
                CandidateInformation = new ApplicationTemplate
                {
                    Qualifications = candidate.ApplicationTemplate.Qualifications,
                    WorkExperience = candidate.ApplicationTemplate.WorkExperience
                },
            };
        }
    }
}