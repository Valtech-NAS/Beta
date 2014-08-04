namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using AutoMapper;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Users;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Repositories;
    using Interfaces.Vacancies;

    public class CreateApplicationStrategy : ICreateApplicationStrategy
    {
        private readonly IVacancyDataProvider _vacancyDataProvider;
        private readonly IApplicationReadRepository _applicationReadRepository;
        private readonly IApplicationWriteRepository _applicationWriteRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;

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
                // New application.
                applicationDetail = CreateNewApplication(candidateId, vacancyId);
                _applicationWriteRepository.Save(applicationDetail);
            }

            return applicationDetail;
        }

        private ApplicationDetail CreateNewApplication(Guid candidateId, int vacancyId)
        {
            var candidate = GetCandidate(candidateId);
            var vacancyDetails = GetVacancyDetails(vacancyId);

            var applicationDetail = new ApplicationDetail
            {
                Status = ApplicationStatuses.Draft,
                DateCreated = DateTime.Now,
                CandidateId = candidateId,
                // TODO: US354: AG: better way to clone? http://stackoverflow.com/questions/5713556/copy-object-to-object-with-automapper
                CandidateDetails = Mapper.Map<RegistrationDetails, RegistrationDetails>(candidate.RegistrationDetails),
                Vacancy = new VacancySummary
                {
                    Id = vacancyDetails.Id,
                    Title = vacancyDetails.Title,
                    EmployerName = vacancyDetails.EmployerName,
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

            // TODO: US354: AG: Existing application tests.
            // TODO: US354: AG: if exists and status = submitting/submitted (or any other "post submission" state) then cannot create new one
            // TODO: US354: AG: if exists and vacancy has expired then return it so user can view it
            // TODO: US354: AG: if exists and status = draft then return it so user can continue with the application form

            return applicationDetail;
        }

        private Candidate GetCandidate(Guid candidateId)
        {
            var candidate = _candidateReadRepository.Get(candidateId);

            if (candidate == null)
            {
                throw new CustomException(
                    "Candidate not found with ID {0}.", ErrorCodes.UnknownCandidateError, candidateId);
            }

            return candidate;
        }

        private VacancyDetail GetVacancyDetails(int vacancyId)
        {
            var vacancyDetails = _vacancyDataProvider.GetVacancyDetails(vacancyId);

            if (vacancyDetails == null)
            {
                throw new CustomException(
                    "Vacancy not found with ID {0}.", ErrorCodes.VacancyNotFoundError, vacancyId);
            }

            return vacancyDetails;
        }
    }
}
