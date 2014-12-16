namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Application.Interfaces.Vacancies;
    using Attributes;
    using Common.Constants;
    using Constants;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Configuration;
    using FluentValidation.Mvc;
    using Providers;
    using Validators;
    using ViewModels.VacancySearch;

    public class TraineeshipSearchController : VacancySearchController
    {
        private readonly TraineeshipSearchViewModelLocationValidator _searchLocationValidator;
        private readonly ISearchProvider _searchProvider;
        private readonly TraineeshipSearchViewModelClientValidator _searchRequestValidator;
        private readonly IVacancyDetailProvider _vacancyDetailProvider;

        public TraineeshipSearchController(IConfigurationManager configManager,
            ISearchProvider searchProvider,
            TraineeshipSearchViewModelClientValidator searchRequestValidator,
            TraineeshipSearchViewModelLocationValidator searchLocationValidator,
            IVacancyDetailProvider vacancyDetailProvider)
            : base(configManager)
        {
            _searchProvider = searchProvider;
            _searchRequestValidator = searchRequestValidator;
            _searchLocationValidator = searchLocationValidator;
            _vacancyDetailProvider = vacancyDetailProvider;
        }

        [HttpGet]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [ApplyWebTrends]
        public async Task<ActionResult> Index()
        {
            return await Task.Run<ActionResult>(() =>
            {
                PopulateDistances();
                PopulateSortType();

                var resultsPerPage = GetResultsPerPage();
                return View(new TraineeshipSearchViewModel {
                        WithinDistance = 40,
                        ResultsPerPage = resultsPerPage,
                        SortType = VacancySortType.Distance
                    });
            });
        }

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

                PopulateDistances(model.WithinDistance);
                PopulateResultsPerPage(model.ResultsPerPage);

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

                        ViewBag.LocationSearches = suggestedLocations.Locations.Skip(1).Select(each =>
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

                PopulateSortType(model.SortType);
                return View("results", results);
            });

        }

        public ActionResult Details()
        {
            return RedirectToAction("Index");
        }
    }
}
