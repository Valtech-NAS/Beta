namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using Domain.Interfaces.Repositories;

    public class ArchiveApplicationStrategy : IArchiveApplicationStrategy
    {
        private readonly IApplicationReadRepository _applicationReadRepository;
        private readonly IApplicationWriteRepository _applicationWriteRepository;

        public ArchiveApplicationStrategy(IApplicationReadRepository applicationReadRepository,
            IApplicationWriteRepository applicationWriteRepository)
        {
            _applicationReadRepository = applicationReadRepository;
            _applicationWriteRepository = applicationWriteRepository;
        }

        public void ArchiveApplication(Guid candidateId, int vacancyId)
        {
            var applicationDetail = _applicationReadRepository
                .GetForCandidate(candidateId, applicationdDetail => applicationdDetail.Vacancy.Id == vacancyId);

            if (applicationDetail != null && !applicationDetail.IsArchived)
            {
                applicationDetail.IsArchived = true;
                _applicationWriteRepository.Save(applicationDetail);
            }
        }
    }
}
