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
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Configuration;
    using FluentValidation.Mvc;
    using Mediators;
    using Providers;
    using Validators;
    using ViewModels.VacancySearch;

    public class ApprenticeshipSearchController : VacancySearchController
    {
        private readonly ApprenticeshipSearchViewModelLocationValidator _searchLocationValidator;
        private readonly IApprenticeshipSearchMediator _apprenticeshipSearchMediator;
        private readonly ISearchProvider _searchProvider;
        private readonly ApprenticeshipSearchViewModelClientValidator _searchRequestValidator;

        public ApprenticeshipSearchController(IConfigurationManager configManager,
            IApprenticeshipSearchMediator apprenticeshipSearchMediator,
            ISearchProvider searchProvider,
            ApprenticeshipSearchViewModelClientValidator searchRequestValidator,
            ApprenticeshipSearchViewModelLocationValidator searchLocationValidator)
            : base(configManager)
        {
            _apprenticeshipSearchMediator = apprenticeshipSearchMediator;
            _searchProvider = searchProvider;
            _searchRequestValidator = searchRequestValidator;
            _searchLocationValidator = searchLocationValidator;
        }

        [HttpGet]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [ApplyWebTrends]
        public async Task<ActionResult> Index()
        {
            return await Task.Run<ActionResult>(() =>
            {
                //Originally done in PopulateSortType
                ModelState.Remove("SortType");

                var response = _apprenticeshipSearchMediator.Index();

                return View(response.ViewModel);
            });
        }

        [HttpGet]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [ApplyWebTrends]
        public async Task<ActionResult> Results(ApprenticeshipSearchViewModel model)
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

                if (model.SearchAction == SearchAction.Search && model.LocationType != ApprenticeshipLocationType.NonNational)
                {
                    model.LocationType = ApprenticeshipLocationType.NonNational;
                }

                if (model.LocationType == ApprenticeshipLocationType.NonNational && model.SortType == VacancySortType.Relevancy &&
                    string.IsNullOrWhiteSpace(model.Keywords))
                {
                    ModelState.Remove("SortType");
                    model.SortType = VacancySortType.Distance;
                }

                if (model.LocationType == ApprenticeshipLocationType.National && string.IsNullOrWhiteSpace(model.Keywords) &&
                    model.SortType != VacancySortType.ClosingDate)
                {
                    ModelState.Remove("SortType");
                    model.SortType = VacancySortType.ClosingDate;
                }

                PopulateDistances(model, model.WithinDistance);
                PopulateResultsPerPage(model.ResultsPerPage);

                var clientResult = _searchRequestValidator.Validate(model);

                if (!clientResult.IsValid)
                {
                    ModelState.Clear();
                    clientResult.AddToModelState(ModelState, string.Empty);

                    return View("results", new ApprenticeshipSearchResponseViewModel {VacancySearch = model});
                }

                if (!HasGeoPoint(model))
                {
                    // User did not select a location from the dropdown list, provide suggested locations.
                    var suggestedLocations = _searchProvider.FindLocation(model.Location.Trim());

                    if (suggestedLocations.HasError())
                    {
                        ModelState.Clear();
                        SetUserMessage(suggestedLocations.ViewModelMessage, UserMessageLevel.Warning);

                        return View("results", new ApprenticeshipSearchResponseViewModel { VacancySearch = model });
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
                            var vsvm = new ApprenticeshipSearchViewModel
                            {
                                Keywords = model.Keywords,
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
                    return View("results", new ApprenticeshipSearchResponseViewModel {VacancySearch = model});
                }

                var results = _searchProvider.FindVacancies(model);

                if (results.HasError())
                {
                    ModelState.Clear();
                    SetUserMessage(results.ViewModelMessage, UserMessageLevel.Warning);
                    return View("results", new ApprenticeshipSearchResponseViewModel {VacancySearch = model});
                }

                if (model.SearchAction == SearchAction.Search && results.TotalLocalHits > 0)
                {
                    results.VacancySearch.LocationType = ApprenticeshipLocationType.NonNational;
                }

                if (results.VacancySearch.LocationType == ApprenticeshipLocationType.National)
                {
                    PopulateSortType(model, model.SortType, model.Keywords, false);
                }
                else
                {
                    PopulateSortType(model, model.SortType, model.Keywords);
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

                return RedirectToRoute(CandidateRouteNames.ApprenticeshipDetails, new { id });
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

                var searchReturnUrl = GetSearchReturnUrl();
                
                var response = _apprenticeshipSearchMediator.Details(id, candidateId, searchReturnUrl);
                
                switch (response.Code)
                {
                    case Codes.ApprenticeshipSearch.Details.VacancyNotFound: 
                        return new VacancyNotFoundResult();
                    case Codes.ApprenticeshipSearch.Details.VacancyHasError:
                        ModelState.Clear();
                        SetUserMessage(response.Message.Message, response.Message.Level);
                        return View(response.ViewModel);
                    case Codes.ApprenticeshipSearch.Details.Ok:
                        return View(response.ViewModel);
                }

                throw new ArgumentException(string.Format("Mediator returned unrecognised code: {0}", response.Code));
            });
        }

        private Guid? GetCandidateId()
        {
            Guid? candidateId = null;

            if (Request.IsAuthenticated && UserContext != null)
            {
                candidateId = UserContext.CandidateId;
            }

            return candidateId;
        }

        private string GetSearchReturnUrl()
        {
            var urlHelper = new UrlHelper(ControllerContext.RequestContext);
            var url = urlHelper.RouteUrl(CandidateRouteNames.ApprenticeshipResults, null);
            if (Request != null && Request.UrlReferrer != null && Request.UrlReferrer.AbsolutePath == url)
                return Request.UrlReferrer.PathAndQuery;
            return null;
        }
    }
}