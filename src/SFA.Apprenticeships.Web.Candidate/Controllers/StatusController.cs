namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using SFA.Apprenticeships.Application.Interfaces.Locations;
    using SFA.Apprenticeships.Domain.Interfaces.Repositories;

    public class StatusController : CandidateControllerBase
    {
        private readonly IApplicationReadRepository _applicationReadRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ILocationLookupProvider _locationLookupProvider;

        private readonly Task[] _statusTasks;

        public StatusController(ILocationLookupProvider locationLookupProvider,
            IApplicationReadRepository applicationReadRepository,
            ICandidateReadRepository candidateReadRepository)
        {
            _locationLookupProvider = locationLookupProvider;
            _applicationReadRepository = applicationReadRepository;
            _candidateReadRepository = candidateReadRepository;

            _statusTasks = new[]
            {
                new Task(() => _applicationReadRepository.Get(Guid.NewGuid())),
                new Task(() => _candidateReadRepository.Get(Guid.NewGuid())),
                new Task(() => _locationLookupProvider.FindLocation("London"))
            };
        }

        public ActionResult Index()
        {
            Parallel.ForEach(_statusTasks, task => task.Start());

            Task.WaitAll(_statusTasks);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}