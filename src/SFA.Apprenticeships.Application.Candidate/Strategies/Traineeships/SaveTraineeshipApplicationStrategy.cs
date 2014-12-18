namespace SFA.Apprenticeships.Application.Candidate.Strategies.Traineeships
{
    using System;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;

    public class SaveTraineeshipApplicationStrategy : ISaveTraineeshipApplicationStrategy
    {
        private readonly ITraineeshipApplicationWriteRepository _traineeshipApplicationWriteRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ICandidateWriteRepository _candidateWriteRepository;

        public SaveTraineeshipApplicationStrategy(ITraineeshipApplicationWriteRepository traineeshipApplicationWriteRepository, 
            ICandidateReadRepository candidateReadRepository,
            ICandidateWriteRepository candidateWriteRepository)
        {
            _traineeshipApplicationWriteRepository = traineeshipApplicationWriteRepository;
            _candidateReadRepository = candidateReadRepository;
            _candidateWriteRepository = candidateWriteRepository;
        }

        public TraineeshipApplicationDetail SaveApplication(TraineeshipApplicationDetail traineeshipApplicationDetail)
        {
            traineeshipApplicationDetail.DateApplied = DateTime.Now;
            var savedApplication = _traineeshipApplicationWriteRepository.Save(traineeshipApplicationDetail);

            SyncToCandidatesApplicationTemplate(savedApplication);

            return savedApplication;
        }

        private void SyncToCandidatesApplicationTemplate(TraineeshipApplicationDetail apprenticeshipApplicationDetail)
        {
            var candidate = _candidateReadRepository.Get(apprenticeshipApplicationDetail.CandidateId);

            candidate.ApplicationTemplate = new ApplicationTemplate
            {
                Qualifications = apprenticeshipApplicationDetail.CandidateInformation.Qualifications,
                WorkExperience = apprenticeshipApplicationDetail.CandidateInformation.WorkExperience
            };

            _candidateWriteRepository.Save(candidate);
        }
    }
}