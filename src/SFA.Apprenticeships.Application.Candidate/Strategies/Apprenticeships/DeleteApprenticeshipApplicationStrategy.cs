namespace SFA.Apprenticeships.Application.Candidate.Strategies.Apprenticeships
{
    using System;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;

    public class DeleteApprenticeshipApplicationStrategy : IDeleteApplicationStrategy
    {
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly IApprenticeshipApplicationWriteRepository _apprenticeshipApplicationWriteRepository;

        public DeleteApprenticeshipApplicationStrategy(IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            IApprenticeshipApplicationWriteRepository apprenticeshipApplicationWriteRepository)
        {
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _apprenticeshipApplicationWriteRepository = apprenticeshipApplicationWriteRepository;
        }

        public void DeleteApplication(Guid candidateId, int vacancyId)
        {
            var applicationDetail = _apprenticeshipApplicationReadRepository.GetForCandidate(candidateId, vacancyId);

            if (applicationDetail != null)
            {
                applicationDetail.AssertState("Delete application", ApplicationStatuses.Draft, ApplicationStatuses.ExpiredOrWithdrawn);

                _apprenticeshipApplicationWriteRepository.Delete(applicationDetail.EntityId);
            }
        }
    }
}