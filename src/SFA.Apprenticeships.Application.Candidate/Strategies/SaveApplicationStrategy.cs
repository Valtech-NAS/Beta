namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;

    public class SaveApplicationStrategy : ISaveApplicationStrategy
    {
        private readonly IApplicationReadRepository _applicationReadRepository;
        private readonly IApplicationWriteRepository _applicationWriteRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ICandidateWriteRepository _candidateWriteRepository;

        public SaveApplicationStrategy(IApplicationReadRepository applicationReadRepository,
            IApplicationWriteRepository applicationWriteRepository, ICandidateReadRepository candidateReadRepository,
            ICandidateWriteRepository candidateWriteRepository)
        {
            _applicationReadRepository = applicationReadRepository;
            _applicationWriteRepository = applicationWriteRepository;
            _candidateReadRepository = candidateReadRepository;
            _candidateWriteRepository = candidateWriteRepository;
        }

        public ApprenticeshipApplicationDetail SaveApplication(ApprenticeshipApplicationDetail apprenticeshipApplication)
        {
            var applicationDetail = _applicationReadRepository.Get(apprenticeshipApplication.EntityId, true);

            applicationDetail.AssertState("Save apprenticeship application", ApplicationStatuses.Draft);

            applicationDetail.CandidateInformation = apprenticeshipApplication.CandidateInformation;
            applicationDetail.AdditionalQuestion1Answer = apprenticeshipApplication.AdditionalQuestion1Answer;
            applicationDetail.AdditionalQuestion2Answer = apprenticeshipApplication.AdditionalQuestion2Answer;

            var savedApplication = _applicationWriteRepository.Save(applicationDetail);

            SyncToCandidatesApplicationTemplate(savedApplication);

            return savedApplication;
        }

        private void SyncToCandidatesApplicationTemplate(ApprenticeshipApplicationDetail apprenticeshipApplicationDetail)
        {
            var candidate = _candidateReadRepository.Get(apprenticeshipApplicationDetail.CandidateId);

            candidate.ApplicationTemplate = new ApplicationTemplate
            {
                AboutYou = apprenticeshipApplicationDetail.CandidateInformation.AboutYou,
                EducationHistory = apprenticeshipApplicationDetail.CandidateInformation.EducationHistory,
                Qualifications = apprenticeshipApplicationDetail.CandidateInformation.Qualifications,
                WorkExperience = apprenticeshipApplicationDetail.CandidateInformation.WorkExperience
            };

            _candidateWriteRepository.Save(candidate);
        }
    }
}