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
    using Mediators.Traineeships;
    using ViewModels.VacancySearch;

    [TraineeshipsToggle]
    public class TraineeshipSearchController : VacancySearchController
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
        public async Task<ActionResult> Results(TraineeshipSearchViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _traineeshipSearchMediator.Results(model);

                switch (response.Code)
                {
                    case Codes.TraineeshipSearch.Results.ValidationError:
                        ModelState.Clear();
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(response.ViewModel);

                    case Codes.TraineeshipSearch.Results.HasError:
                        ModelState.Clear();
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View(response.ViewModel);

                    case Codes.TraineeshipSearch.Results.Ok:
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
        public async Task<ActionResult> DetailsWithDistance(int id, string distance)
        {
            return await Task.Run<ActionResult>(() =>
            {
                UserData.Push(UserDataItemNames.VacancyDistance, distance);
                UserData.Push(UserDataItemNames.LastViewedVacancyId, id.ToString(CultureInfo.InvariantCulture));

                return RedirectToRoute(CandidateRouteNames.TraineeshipDetails, new { id });
            });
        }

        [HttpGet]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [ApplyWebTrends]
        public async Task<ActionResult> Details(int id)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var candidateId = GetCandidateId();
                var searchReturnUrl = GetSearchReturnUrl(CandidateRouteNames.TraineeshipResults);
                var response = _traineeshipSearchMediator.Details(id, candidateId, searchReturnUrl);

                switch (response.Code)
                {
                    case Codes.TraineeshipSearch.Details.VacancyNotFound:
                        return new VacancyNotFoundResult();

                    case Codes.TraineeshipSearch.Details.VacancyHasError:
                        ModelState.Clear();
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View(response.ViewModel);

                    case Codes.TraineeshipSearch.Details.Ok:
                        return View(response.ViewModel);
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }
    }
}
