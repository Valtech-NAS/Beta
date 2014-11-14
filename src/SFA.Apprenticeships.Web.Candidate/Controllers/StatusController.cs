namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Application.Interfaces.Locations;
    using Domain.Interfaces.Repositories;

    public class StatusController : CandidateControllerBase
    {
        private readonly IApplicationReadRepository _applicationReadRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ILocationLookupProvider _locationLookupProvider;

        public StatusController(ILocationLookupProvider locationLookupProvider,
            IApplicationReadRepository applicationReadRepository,
            ICandidateReadRepository candidateReadRepository)
        {
            _locationLookupProvider = locationLookupProvider;
            _applicationReadRepository = applicationReadRepository;
            _candidateReadRepository = candidateReadRepository;
        }

        public ActionResult Index()
        {
            var statusTasks = new[]
            {
                new Task(() => _applicationReadRepository.Get(Guid.NewGuid())),
                new Task(() => _candidateReadRepository.Get(Guid.NewGuid())),
                new Task(() => _locationLookupProvider.FindLocation("London"))
            };

            Parallel.ForEach(statusTasks, task => task.Start());

            Task.WaitAll(statusTasks);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}