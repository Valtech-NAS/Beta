namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Globalization;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ActionResults;
    using Attributes;
    using Common.Constants;
    using Constants;
    using FluentValidation.Mvc;
    using Mediators;
    using ViewModels.VacancySearch;

    public class ApprenticeshipSearchController : CandidateControllerBase
    {
        private readonly IApprenticeshipSearchMediator _apprenticeshipSearchMediator;

        public ApprenticeshipSearchController(IApprenticeshipSearchMediator apprenticeshipSearchMediator)
        {
            _apprenticeshipSearchMediator = apprenticeshipSearchMediator;
        }

        [HttpGet]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [ApplyWebTrends]
        public async Task<ActionResult> Index(ApprenticeshipSearchMode searchMode = ApprenticeshipSearchMode.Keyword)
        {
            return await Task.Run<ActionResult>(() =>
            {
                //Originally done in PopulateSortType
                ModelState.Remove("SortType");

                var response = _apprenticeshipSearchMediator.Index(searchMode);

                return View(response.ViewModel);
            });
        }

        [HttpGet]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [ApplyWebTrends]
        [ClearSearchReturnUrl(ClearSearchReturnUrl = false)]
        public async Task<ActionResult> Results(ApprenticeshipSearchViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                ViewBag.SearchReturnUrl = (Request != null && Request.Url != null) ? Request.Url.PathAndQuery : null;

                var response = _apprenticeshipSearchMediator.Results(model);

                switch (response.Code)
                {
                    case Codes.ApprenticeshipSearch.Results.ValidationError:
                        ModelState.Clear();
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(response.ViewModel);
                    case Codes.ApprenticeshipSearch.Results.HasError:
                        ModelState.Clear();
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View(response.ViewModel);
                    case Codes.ApprenticeshipSearch.Results.ExactMatchFound:
                        ViewBag.SearchReturnUrl = null;
                        return RedirectToAction("Details", response.Parameters);
                    case Codes.ApprenticeshipSearch.Results.Ok:
                        ModelState.Remove("Location");
                        ModelState.Remove("Latitude");
                        ModelState.Remove("Longitude");
                        ModelState.Remove("SortType");
                        return View(response.ViewModel);
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }

        [HttpGet]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [ClearSearchReturnUrl(ClearSearchReturnUrl = false)]
        public async Task<ActionResult> DetailsWithDistance(int id, string distance)
        {
            return await Task.Run<ActionResult>(() =>
            {
                UserData.Push(UserDataItemNames.VacancyDistance, distance);
                UserData.Push(UserDataItemNames.LastViewedVacancyId, id.ToString(CultureInfo.InvariantCulture));

                return RedirectToRoute(CandidateRouteNames.ApprenticeshipDetails, new { id });
            });
        }

        [HttpGet]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [ApplyWebTrends]
        [ClearSearchReturnUrl(ClearSearchReturnUrl = false)]
        public async Task<ActionResult> Details(string id)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var candidateId = GetCandidateId();
                
                var response = _apprenticeshipSearchMediator.Details(id, candidateId);
                
                switch (response.Code)
                {
                    case Codes.ApprenticeshipSearch.Details.VacancyNotFound: 
                        return new ApprenticeshipNotFoundResult();
                    case Codes.ApprenticeshipSearch.Details.VacancyHasError:
                        ModelState.Clear();
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View(response.ViewModel);
                    case Codes.ApprenticeshipSearch.Details.Ok:
                        return View(response.ViewModel);
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }
    }
}