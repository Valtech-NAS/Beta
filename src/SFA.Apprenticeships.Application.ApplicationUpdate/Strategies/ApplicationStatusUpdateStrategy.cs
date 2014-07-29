namespace SFA.Apprenticeships.Application.ApplicationUpdate.Strategies
{
    using System;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;

    public class ApplicationStatusUpdateStrategy : IApplicationStatusUpdateStrategy
    {
        private readonly IApplicationWriteRepository _applicationWriteRepository;

        public ApplicationStatusUpdateStrategy(IApplicationWriteRepository applicationWriteRepository)
        {
            _applicationWriteRepository = applicationWriteRepository;
        }

        public void Update(ApplicationStatusSummary applicationStatus)
        {
            //todo: update status of application in app repo.
            // note, will be extended to include a call to outbound communication later
            // log any changes

            throw new NotImplementedException();
        }
    }
}
