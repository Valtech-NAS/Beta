namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
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

        public ApplicationDetail SaveApplication(ApplicationDetail application)
        {
            var entity = _applicationReadRepository.Get(application.EntityId);

            if (entity == null)
            {
                throw new CustomException("Application was not found", ErrorCodes.ApplicationNotFoundError);
            }

            entity.AssertState("Application should not be submitted", ApplicationStatuses.Draft);

            entity.CandidateInformation = application.CandidateInformation;
            entity.AdditionalQuestion1Answer = application.AdditionalQuestion1Answer;
            entity.AdditionalQuestion2Answer = application.AdditionalQuestion2Answer;

            var savedApplication = _applicationWriteRepository.Save(entity);

            SyncToCandidatesApplicationTemplate(savedApplication);

            return savedApplication;
        }

        private void SyncToCandidatesApplicationTemplate(ApplicationDetail applicationDetail)
        {
            var candidate = _candidateReadRepository.Get(applicationDetail.CandidateId);

            if (candidate == null)
            {
                throw new CustomException("Candidate not found", ErrorCodes.UnknownCandidateError);
            }

            candidate.ApplicationTemplate = new ApplicationTemplate
            {
                AboutYou = applicationDetail.CandidateInformation.AboutYou,
                EducationHistory = applicationDetail.CandidateInformation.EducationHistory,
                Qualifications = applicationDetail.CandidateInformation.Qualifications,
                WorkExperience = applicationDetail.CandidateInformation.WorkExperience
            };

            _candidateWriteRepository.Save(candidate);
        }
    }
}