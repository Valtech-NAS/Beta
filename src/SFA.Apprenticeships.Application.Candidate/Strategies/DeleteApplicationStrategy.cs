namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;

    public class DeleteApplicationStrategy : IDeleteApplicationStrategy
    {
        private readonly IApplicationReadRepository _applicationReadRepository;
        private readonly IApplicationWriteRepository _applicationWriteRepository;

        public DeleteApplicationStrategy(IApplicationReadRepository applicationReadRepository,
            IApplicationWriteRepository applicationWriteRepository)
        {
            _applicationReadRepository = applicationReadRepository;
            _applicationWriteRepository = applicationWriteRepository;
        }

        public void DeleteApplication(Guid applicationId)
        {
            var applicationDetail = _applicationReadRepository.Get(applicationId, true);

            applicationDetail.AssertState("Delete application", ApplicationStatuses.Draft, ApplicationStatuses.ExpiredOrWithdrawn);

            _applicationWriteRepository.Delete(applicationDetail.EntityId);
        }
    }
}