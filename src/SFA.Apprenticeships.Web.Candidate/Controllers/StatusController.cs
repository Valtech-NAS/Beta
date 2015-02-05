namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Providers;

    public class StatusController : CandidateControllerBase
    {
        private readonly IApprenticeshipApplicationProvider _apprenticeshipApplicationProvider;
        private readonly ICandidateServiceProvider _candidateServiceProvider;
        private readonly ISearchProvider _searchProvider;

        public StatusController(ISearchProvider searchProvider,
            IApprenticeshipApplicationProvider apprenticeshipApplicationProvider,
            ICandidateServiceProvider candidateServiceProvider)
        {
            _searchProvider = searchProvider;
            _apprenticeshipApplicationProvider = apprenticeshipApplicationProvider;
            _candidateServiceProvider = candidateServiceProvider;
        }

        public ActionResult Index()
        {
            var statusTasks = new[]
            {
                new Task(() => _apprenticeshipApplicationProvider.GetApplicationViewModel(Guid.NewGuid(), 123456)),
                new Task(() => _candidateServiceProvider.GetCandidate(Guid.NewGuid())),
                new Task(() => _searchProvider.FindLocation("London"))
            };

            Parallel.ForEach(statusTasks, task => task.Start());

            Task.WaitAll(statusTasks);

            // GDS Performance platform (Pingdom) is configured to look for the following text
            // in a 200 response. Anything other than this will register the site as down.
            return Content("System healthy.");
        }
    }
}