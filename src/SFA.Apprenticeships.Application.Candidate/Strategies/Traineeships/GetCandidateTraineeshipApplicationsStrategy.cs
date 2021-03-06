﻿namespace SFA.Apprenticeships.Application.Candidate.Strategies.Traineeships
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;

    public class GetCandidateTraineeshipApplicationsStrategy : IGetCandidateTraineeshipApplicationsStrategy
    {
        private readonly ITraineeshipApplicationReadRepository _traineeshipApplicationReadRepository;

        public GetCandidateTraineeshipApplicationsStrategy(ITraineeshipApplicationReadRepository traineeshipApplicationReadRepository)
        {
            _traineeshipApplicationReadRepository = traineeshipApplicationReadRepository;
        }

        public IList<TraineeshipApplicationSummary> GetApplications(Guid candidateId)
        {
            return _traineeshipApplicationReadRepository.GetForCandidate(candidateId);
        }
    }
}
