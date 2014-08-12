﻿namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using AutoMapper;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
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
                return CreateNewApplication(candidateId, vacancyId);
            }

            if (DateTime.Now > applicationDetail.Vacancy.ClosingDate)
            {
                applicationDetail.Status = ApplicationStatuses.ExpiredOrWithdrawn;
                _applicationWriteRepository.Save(applicationDetail);

                throw new CustomException("Vacancy has expired", ErrorCodes.VacancyExpired);
            }

            applicationDetail.AssertState("Application should be in draft", ApplicationStatuses.Draft);

            return applicationDetail;
        }

        private ApplicationDetail CreateNewApplication(Guid candidateId, int vacancyId)
        {
            var vacancyDetails = GetVacancyDetails(vacancyId);

            if (DateTime.Now > vacancyDetails.ClosingDate)
            {
                throw new CustomException("Vacancy has expired can't create a new application",
                    ErrorCodes.VacancyExpired);
            }

            var candidate = _candidateReadRepository.Get(candidateId);

            var applicationDetail = new ApplicationDetail
            {
                EntityId = Guid.NewGuid(),
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

            _applicationWriteRepository.Save(applicationDetail);

            return applicationDetail;
        }

        private VacancyDetail GetVacancyDetails(int vacancyId)
        {
            var vacancyDetails = _vacancyDataProvider.GetVacancyDetails(vacancyId);

            if (vacancyDetails == null)
            {
                // TODO: move null check to data provider (same as User and Candidate repositories).
                throw new CustomException(
                    "Vacancy not found with ID {0}.", ErrorCodes.VacancyNotFoundError, vacancyId);
            }

            return vacancyDetails;
        }
    }
}