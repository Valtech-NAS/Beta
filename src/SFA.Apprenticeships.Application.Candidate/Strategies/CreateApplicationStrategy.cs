namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using AutoMapper;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Repositories;
    using Vacancy;

    public class CreateApplicationStrategy : ICreateApplicationStrategy
    {
        private readonly IApplicationReadRepository _applicationReadRepository;
        private readonly IApplicationWriteRepository _applicationWriteRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IVacancyDataProvider _vacancyDataProvider;

        public CreateApplicationStrategy(
            IVacancyDataProvider vacancyDataProvider,
            IApplicationReadRepository applicationReadRepository,
            IApplicationWriteRepository applicationWriteRepository,
            ICandidateReadRepository candidateReadRepository)
        {
            _vacancyDataProvider = vacancyDataProvider;
            _applicationWriteRepository = applicationWriteRepository;
            _applicationReadRepository = applicationReadRepository;
            _candidateReadRepository = candidateReadRepository;
        }

        public ApplicationDetail CreateApplication(Guid candidateId, int vacancyId)
        {
            var applicationDetail = _applicationReadRepository.GetForCandidate(
                candidateId, applicationdDetail => applicationdDetail.Vacancy.Id == vacancyId);

            if (applicationDetail == null)
            {
                // Candidate has not previously applied for this vacancy.
                return CreateNewApplication(candidateId, vacancyId);
            }

            applicationDetail.AssertState("Create application", ApplicationStatuses.Draft);

            var vacancyDetails = _vacancyDataProvider.GetVacancyDetails(vacancyId);

            if (vacancyDetails == null)
            {
                // Update application status.
                _applicationWriteRepository.ExpireOrWithdrawForCandidate(candidateId, vacancyId);

                return _applicationReadRepository.Get(applicationDetail.EntityId);
            }

            if (applicationDetail.IsArchived)
            {
                return UnarchiveApplication(applicationDetail);
            }

            return applicationDetail;
        }

        private ApplicationDetail UnarchiveApplication(ApplicationDetail applicationDetail)
        {
            applicationDetail.IsArchived = false;
            _applicationWriteRepository.Save(applicationDetail);

            return _applicationReadRepository.Get(applicationDetail.EntityId);
        }

        private ApplicationDetail CreateNewApplication(Guid candidateId, int vacancyId)
        {
            var vacancyDetails = _vacancyDataProvider.GetVacancyDetails(vacancyId);

            if (vacancyDetails == null)
            {
                return new ApplicationDetail
                {
                    Status = ApplicationStatuses.ExpiredOrWithdrawn
                };
            }

            var candidate = _candidateReadRepository.Get(candidateId);
            var applicationDetail = CreateApplicationDetail(candidate, vacancyDetails);

            _applicationWriteRepository.Save(applicationDetail);

            return applicationDetail;
        }

        private static ApplicationDetail CreateApplicationDetail(Candidate candidate, VacancyDetail vacancyDetails)
        {
            return new ApplicationDetail
            {
                EntityId = Guid.NewGuid(),
                Status = ApplicationStatuses.Draft,
                DateCreated = DateTime.Now,
                CandidateId = candidate.EntityId,
                // TODO: US354: AG: better way to clone? http://stackoverflow.com/questions/5713556/copy-object-to-object-with-automapper
                CandidateDetails = Mapper.Map<RegistrationDetails, RegistrationDetails>(candidate.RegistrationDetails),
                Vacancy = new VacancySummary
                {
                    Id = vacancyDetails.Id,
                    Title = vacancyDetails.Title,
                    EmployerName = vacancyDetails.IsEmployerAnonymous ?
                        vacancyDetails.AnonymousEmployerName : vacancyDetails.EmployerName,
                    ClosingDate = vacancyDetails.ClosingDate,
                    Description = vacancyDetails.Description,
                    Location = null, // NOTE: no equivalent in legacy vacancy details.
                    VacancyLocationType = vacancyDetails.VacancyLocationType
                },
                // Populate application template with candidate's most recent information.
                CandidateInformation = new ApplicationTemplate
                {
                    EducationHistory = candidate.ApplicationTemplate.EducationHistory,
                    Qualifications = candidate.ApplicationTemplate.Qualifications,
                    WorkExperience = candidate.ApplicationTemplate.WorkExperience,
                    AboutYou = candidate.ApplicationTemplate.AboutYou
                },
            };
        }
    }
}
