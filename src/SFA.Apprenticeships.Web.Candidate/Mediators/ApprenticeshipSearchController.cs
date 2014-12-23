/*namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Attributes;
    using Constants;
    using Controllers;
    using ViewModels.VacancySearch;

    public class ApprenticeshipSearchController : CandidateControllerBase
    {
        private readonly IApprenticeshipSearchMediator _mediator;

        public ApprenticeshipSearchController(IApprenticeshipSearchMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [ApplyWebTrends]
        public async Task<ActionResult> Results(ApprenticeshipSearchViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _mediator.Search(model);
                switch (response.Code)
                {
                    case "details":
                        return RedirectToRoute("Details", response.Parameters["id"]);
                    case "results":
                        return View("results", response.ViewModel);
                    default:
                        throw new Exception(string.Format("Response code {0} was not recognized", response.Code));
                }
            });
        }
    }
}*/