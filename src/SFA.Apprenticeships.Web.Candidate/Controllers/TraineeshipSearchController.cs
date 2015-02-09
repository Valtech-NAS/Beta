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

    public class TraineeshipSearchController : CandidateControllerBase
    {
        private readonly ITraineeshipSearchMediator _traineeshipSearchMediator;

        public TraineeshipSearchController(ITraineeshipSearchMediator traineeshipSearchMediator)
        {
            _traineeshipSearchMediator = traineeshipSearchMediator;
        }

        [HttpGet]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [ApplyWebTrends]
        public async Task<ActionResult> Overview()
        {
            return await Task.Run<ActionResult>(() => View());
        }

        [HttpGet]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [ApplyWebTrends]
        public async Task<ActionResult> Index()
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _traineeshipSearchMediator.Index();

                return View(response.ViewModel);
            });
        }

        [HttpGet]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [ApplyWebTrends]
        [ClearSearchReturnUrl(ClearSearchReturnUrl = false)]
        public async Task<ActionResult> Results(TraineeshipSearchViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                ViewBag.SearchReturnUrl = (Request != null && Request.Url != null) ? Request.Url.PathAndQuery : null;

                var response = _traineeshipSearchMediator.Results(model);

                switch (response.Code)
                {
                    case TraineeshipSearchMediatorCodes.Results.ValidationError:
                        ModelState.Clear();
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(response.ViewModel);

                    case TraineeshipSearchMediatorCodes.Results.HasError:
                        ModelState.Clear();
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View(response.ViewModel);

                    case TraineeshipSearchMediatorCodes.Results.Ok:
                        ModelState.Remove("Location");
                        ModelState.Remove("Latitude");
                        ModelState.Remove("Longitude");

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

                return RedirectToRoute(CandidateRouteNames.TraineeshipDetails, new { id });
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

                string searchReturnUrl = ViewBag.SearchReturnUrl != null ? ViewBag.SearchReturnUrl.ToString() : null;

                var response = _traineeshipSearchMediator.Details(id, candidateId, searchReturnUrl);

                switch (response.Code)
                {
                    case TraineeshipSearchMediatorCodes.Details.VacancyNotFound:
                        return new TraineeshipNotFoundResult();

                    case TraineeshipSearchMediatorCodes.Details.VacancyHasError:
                        ModelState.Clear();
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View(response.ViewModel);

                    case TraineeshipSearchMediatorCodes.Details.Ok:
                        return View(response.ViewModel);
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }
    }
}
