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

        public void ArchiveApplication(Guid applicationId)
        {
            var applicationDetail = _applicationReadRepository.Get(applicationId, true);

            if (applicationDetail.IsArchived) return;

            applicationDetail.IsArchived = true;
            _applicationWriteRepository.Save(applicationDetail);
        }
    }
}
