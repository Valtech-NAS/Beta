namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ActionResults;
    using Application.Interfaces.Vacancies;
    using Attributes;
    using Common.Constants;
    using Constants;
    using Domain.Interfaces.Configuration;
    using FluentValidation.Mvc;
    using Microsoft.Ajax.Utilities;
    using Providers;
    using Validators;
    using ViewModels.VacancySearch;

    [TraineeshipsToggle]
    public class TraineeshipSearchController : VacancySearchController
    {
        private readonly TraineeshipSearchViewModelLocationValidator _searchLocationValidator;
        private readonly ISearchProvider _searchProvider;
        private readonly TraineeshipSearchViewModelClientValidator _searchRequestValidator;
        private readonly ITraineeshipVacancyDetailProvider _traineeshipVacancyDetailProvider;
        
        public TraineeshipSearchController(IConfigurationManager configManager,
            ISearchProvider searchProvider,
            TraineeshipSearchViewModelClientValidator searchRequestValidator,
            TraineeshipSearchViewModelLocationValidator searchLocationValidator,
            ITraineeshipVacancyDetailProvider traineeshipVacancyDetailProvider)
            : base(configManager)
        {
            _searchProvider = searchProvider;
            _searchRequestValidator = searchRequestValidator;
            _searchLocationValidator = searchLocationValidator;
            _traineeshipVacancyDetailProvider = traineeshipVacancyDetailProvider;
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
                var resultsPerPage = GetResultsPerPage();

                var viewModel = new TraineeshipSearchViewModel
                {
                    WithinDistance = 40,
                    ResultsPerPage = resultsPerPage,
                };

                PopulateDistances(viewModel);
                PopulateSortType(viewModel);
                
                return View(viewModel);
            });
        }

        [HttpGet]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [ApplyWebTrends]
        public async Task<ActionResult> Results(TraineeshipSearchViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                UserData.Pop(UserDataItemNames.VacancyDistance);

                if (model.ResultsPerPage == 0)
                {
                    model.ResultsPerPage = GetResultsPerPage();
                }

                UserData.Push(UserDataItemNames.ResultsPerPage,
                    model.ResultsPerPage.ToString(CultureInfo.InvariantCulture));

                PopulateDistances(model, model.WithinDistance);
                PopulateResultsPerPage(model, model.ResultsPerPage);
                PopulateSortType(model, model.SortType);

                var clientResult = _searchRequestValidator.Validate(model);

                if (!clientResult.IsValid)
                {
                    ModelState.Clear();
                    clientResult.AddToModelState(ModelState, string.Empty);

                    return View("results", new TraineeshipSearchResponseViewModel { VacancySearch = model });
                }

                if (!HasGeoPoint(model))
                {
                    // User did not select a location from the dropdown list, provide suggested locations.
                    var suggestedLocations = _searchProvider.FindLocation(model.Location.Trim());

                    if (suggestedLocations.HasError())
                    {
                        ModelState.Clear();
                        SetUserMessage(suggestedLocations.ViewModelMessage, UserMessageLevel.Warning);

                        return View("results", new TraineeshipSearchResponseViewModel { VacancySearch = model });
                    }

                    if (suggestedLocations.Locations.Any())
                    {
                        var location = suggestedLocations.Locations.First();

                        ModelState.Remove("Location");
                        ModelState.Remove("Latitude");
                        ModelState.Remove("Longitude");

                        model.Location = location.Name;
                        model.Latitude = location.Latitude;
                        model.Longitude = location.Longitude;

                        model.LocationSearches = suggestedLocations.Locations.Skip(1).Select(each =>
                        {
                            var vsvm = new TraineeshipSearchViewModel
                            {
                                Location = each.Name,
                                Latitude = each.Latitude,
                                Longitude = each.Longitude,
                                PageNumber = model.PageNumber,
                                SortType = model.SortType,
                                WithinDistance = model.WithinDistance,
                                ResultsPerPage = model.ResultsPerPage
                            };

                            vsvm.Hash = vsvm.LatLonLocHash();

                            return vsvm;
                        }).ToArray();
                    }
                }

                var locationResult = _searchLocationValidator.Validate(model);

                if (!locationResult.IsValid)
                {
                    ModelState.Clear();
                    return View("results", new TraineeshipSearchResponseViewModel { VacancySearch = model });
                }

                var results = _searchProvider.FindVacancies(model);

                if (results.HasError())
                {
                    ModelState.Clear();
                    SetUserMessage(results.ViewModelMessage, UserMessageLevel.Warning);
                    return View("results", new TraineeshipSearchResponseViewModel { VacancySearch = model });
                }
                
                return View("results", results);
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
                Guid? candidateId = null;

                if (Request.IsAuthenticated && UserContext != null)
                {
                    candidateId = UserContext.CandidateId;
                }

                var vacancy = _traineeshipVacancyDetailProvider.GetVacancyDetailViewModel(candidateId, id);

                if (vacancy == null)
                {
                    return new VacancyNotFoundResult();
                }

                if (vacancy.HasError())
                {
                    ModelState.Clear();
                    SetUserMessage(vacancy.ViewModelMessage, UserMessageLevel.Warning);

                    return View(vacancy);
                }

                var distance = UserData.Pop(UserDataItemNames.VacancyDistance);
                var lastVacancyId = UserData.Pop(UserDataItemNames.LastViewedVacancyId);

                if (HasToPopulateDistance(id, distance, lastVacancyId))
                {
                    ViewBag.Distance = distance;
                    UserData.Push(UserDataItemNames.VacancyDistance, distance);
                }

                if (HasToPopulateReturnUrl())
                {
// ReSharper disable once PossibleNullReferenceException
                    ViewBag.SearchReturnUrl = Request.UrlReferrer.PathAndQuery;
                }

                UserData.Push(UserDataItemNames.LastViewedVacancyId, id.ToStringInvariant());

                return View(vacancy);
            });
        }
        protected bool HasToPopulateReturnUrl()
        {
            var urlHelper = new UrlHelper(ControllerContext.RequestContext);
            var url = urlHelper.RouteUrl(CandidateRouteNames.TraineeshipResults, null);
            return Request != null && Request.UrlReferrer != null && Request.UrlReferrer.AbsolutePath == url;
        }
    }
}
