using SFA.Apprenticeships.Domain.Entities.Vacancies.Apprenticeships;

namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using AutoMapper;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Repositories;
    using SFA.Apprenticeships.Domain.Entities.Vacancies.Traineeships;
    using Vacancy;

    public class CreateTraineeshipApplicationStrategy : ICreateTraineeshipApplicationStrategy
    {
        //private readonly IApplicationReadRepository _applicationReadRepository;
        //private readonly IApplicationWriteRepository _applicationWriteRepository;
        // TODO: change it for TraineeshipRepositories
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IVacancyDataProvider _vacancyDataProvider;

        public CreateTraineeshipApplicationStrategy(
            IVacancyDataProvider vacancyDataProvider,
            //IApplicationReadRepository applicationReadRepository,
            //IApplicationWriteRepository applicationWriteRepository,
            ICandidateReadRepository candidateReadRepository)
        {
            _vacancyDataProvider = vacancyDataProvider;
            //_applicationWriteRepository = applicationWriteRepository;
            //_applicationReadRepository = applicationReadRepository;
            _candidateReadRepository = candidateReadRepository;
        }

        public TraineeshipApplicationDetail CreateApplication(Guid candidateId, int vacancyId)
        {
            //var applicationDetail = _applicationReadRepository.GetForCandidate(
            //    candidateId, applicationdDetail => applicationdDetail.Vacancy.Id == vacancyId);

            TraineeshipApplicationDetail applicationDetail = null;

            if (applicationDetail == null)
            {
                // Candidate has not previously applied for this vacancy.
                return CreateNewApplication(candidateId, vacancyId);
            }

            /*
            applicationDetail.AssertState("Create apprenticeshipApplication", ApplicationStatuses.Draft);

            var vacancyDetails = _vacancyDataProvider.GetVacancyDetails(vacancyId);

            if (vacancyDetails == null)
            {
                // Update apprenticeshipApplication status.
                _applicationWriteRepository.ExpireOrWithdrawForCandidate(candidateId, vacancyId);

                return _applicationReadRepository.Get(applicationDetail.EntityId);
            }

            if (applicationDetail.IsArchived)
            {
                return UnarchiveApplication(applicationDetail);
            }
            */
            return applicationDetail;
        }

        /*
        private ApprenticeshipApplicationDetail UnarchiveApplication(ApprenticeshipApplicationDetail apprenticeshipApplicationDetail)
        {
            apprenticeshipApplicationDetail.IsArchived = false;
            _applicationWriteRepository.Save(apprenticeshipApplicationDetail);

            return _applicationReadRepository.Get(apprenticeshipApplicationDetail.EntityId);
        }*/

        private TraineeshipApplicationDetail CreateNewApplication(Guid candidateId, int vacancyId)
        {
            var vacancyDetails = _vacancyDataProvider.GetVacancyDetails(vacancyId);

            if (vacancyDetails == null)
            {
                return new TraineeshipApplicationDetail
                {
                    Status = ApplicationStatuses.ExpiredOrWithdrawn
                };
            }

            var candidate = _candidateReadRepository.Get(candidateId);
            var applicationDetail = CreateApplicationDetail(candidate, vacancyDetails);

            // TODO: use new repository
            // _applicationWriteRepository.Save(applicationDetail);

            return applicationDetail;
        }

        private static TraineeshipApplicationDetail CreateApplicationDetail(Candidate candidate, VacancyDetail vacancyDetails)
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
                    EmployerName = vacancyDetails.IsEmployerAnonymous ?
                        vacancyDetails.AnonymousEmployerName : vacancyDetails.EmployerName,
                    ClosingDate = vacancyDetails.ClosingDate,
                    Description = vacancyDetails.Description,
                    Location = null, // NOTE: no equivalent in legacy vacancy details.
                    VacancyLocationType = vacancyDetails.VacancyLocationType
                },
                // Populate traineeshipApplication template with candidate's most recent information.
                // TODO: a traineeship doesn't have about you information
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
