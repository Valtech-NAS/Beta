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
        //todo: none of these references are "allowed" - should be calling services
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ILocationLookupProvider _locationLookupProvider;

        public StatusController(ILocationLookupProvider locationLookupProvider,
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            ICandidateReadRepository candidateReadRepository)
        {
            _locationLookupProvider = locationLookupProvider;
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _candidateReadRepository = candidateReadRepository;
        }

        public ActionResult Index()
        {
            var statusTasks = new[]
            {
                new Task(() => _apprenticeshipApplicationReadRepository.Get(Guid.NewGuid())),
                new Task(() => _candidateReadRepository.Get(Guid.NewGuid())),
                new Task(() => _locationLookupProvider.FindLocation("London"))
            };

            Parallel.ForEach(statusTasks, task => task.Start());

            Task.WaitAll(statusTasks);

            // GDS Performance platform (Pingdom) is configured to look for the following text
            // in a 200 response. Anything other than this will register the site as down.
            return Content("System healthy.");
        }
    }
}