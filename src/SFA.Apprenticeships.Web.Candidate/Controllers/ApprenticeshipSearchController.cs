namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Globalization;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ActionResults;
    using Attributes;
    using Constants;
    using FluentValidation.Mvc;
    using Mediators;
    using Mediators.Search;
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
        public async Task<ActionResult> SearchValidation(ApprenticeshipSearchViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                ViewBag.SearchReturnUrl = (Request != null && Request.Url != null) ? Request.Url.PathAndQuery : null;

                var response = _apprenticeshipSearchMediator.SearchValidation(model);

                switch (response.Code)
                {
                    case ApprenticeshipSearchMediatorCodes.SearchValidation.ValidationError:
                        ModelState.Clear();
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View("Index", response.ViewModel);
                    case ApprenticeshipSearchMediatorCodes.SearchValidation.Ok:
                        return RedirectToAction("Results", model);
                }

                throw new InvalidMediatorCodeException(response.Code);
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
                    case ApprenticeshipSearchMediatorCodes.Results.ValidationError:
                        ModelState.Clear();
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(response.ViewModel);
                    case ApprenticeshipSearchMediatorCodes.Results.HasError:
                        ModelState.Clear();
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View(response.ViewModel);
                    case ApprenticeshipSearchMediatorCodes.Results.ExactMatchFound:
                        ViewBag.SearchReturnUrl = null;
                        return RedirectToAction("Details", response.Parameters);
                    case ApprenticeshipSearchMediatorCodes.Results.Ok:
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
                UserData.Push(CandidateDataItemNames.VacancyDistance, distance);
                UserData.Push(CandidateDataItemNames.LastViewedVacancyId, id.ToString(CultureInfo.InvariantCulture));

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
                    case ApprenticeshipSearchMediatorCodes.Details.VacancyNotFound: 
                        return new ApprenticeshipNotFoundResult();
                    case ApprenticeshipSearchMediatorCodes.Details.VacancyHasError:
                        ModelState.Clear();
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View(response.ViewModel);
                    case ApprenticeshipSearchMediatorCodes.Details.Ok:
                        return View(response.ViewModel);
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }
    }
}