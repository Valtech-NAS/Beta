namespace SFA.Apprenticeships.Web.Candidate.Mediators.Traineeships
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Application.Interfaces.Vacancies;
    using Common.Constants;
    using Common.Providers;
    using Domain.Interfaces.Configuration;
    using Providers;
    using Validators;
    using ViewModels.VacancySearch;

    public class TraineeshipSearchMediator : SearchMediatorBase, ITraineeshipSearchMediator
    {
        private readonly ISearchProvider _searchProvider;
        private readonly ITraineeshipVacancyDetailProvider _traineeshipVacancyDetailProvider;
        private readonly TraineeshipSearchViewModelServerValidator _searchRequestValidator;
        private readonly TraineeshipSearchViewModelLocationValidator _searchLocationValidator;

        public TraineeshipSearchMediator(
            IConfigurationManager configManager,
            ISearchProvider searchProvider,
            ITraineeshipVacancyDetailProvider traineeshipVacancyDetailProvider,
            IUserDataProvider userDataProvider,
            TraineeshipSearchViewModelServerValidator searchRequestValidator,
            TraineeshipSearchViewModelLocationValidator searchLocationValidator)
            : base(configManager, userDataProvider)
        {
            _searchProvider = searchProvider;
            _traineeshipVacancyDetailProvider = traineeshipVacancyDetailProvider;
            _searchRequestValidator = searchRequestValidator;
            _searchLocationValidator = searchLocationValidator;
        }

        public MediatorResponse<TraineeshipSearchViewModel> Index()
        {
            var traineeshipSearchViewModel = new TraineeshipSearchViewModel
            {
                WithinDistance = 40,
                Distances = GetDistances(),
                SortTypes = GetSortTypes(),
                SortType = VacancySortType.Distance,
                ResultsPerPage = GetResultsPerPage()
            };

            return GetMediatorResponse(Codes.TraineeshipSearch.Index.Ok, traineeshipSearchViewModel);
        }

        public MediatorResponse<TraineeshipSearchResponseViewModel> Results(TraineeshipSearchViewModel model)
        {
            UserDataProvider.Pop(UserDataItemNames.VacancyDistance);

            if (model.ResultsPerPage == 0)
            {
                model.ResultsPerPage = GetResultsPerPage();
            }

            UserDataProvider.Push(UserDataItemNames.ResultsPerPage, model.ResultsPerPage.ToString(CultureInfo.InvariantCulture));

            model.Distances = GetDistances(model.WithinDistance);
            model.ResultsPerPageSelectList = GetResultsPerPageSelectList(model.ResultsPerPage);

            var clientResult = _searchRequestValidator.Validate(model);

            if (!clientResult.IsValid)
            {
                return GetMediatorResponse(Codes.TraineeshipSearch.Results.ValidationError, new TraineeshipSearchResponseViewModel { VacancySearch = model }, clientResult);
            }

            if (!HasGeoPoint(model))
            {
                // User did not select a location from the dropdown list, provide suggested locations.
                var suggestedLocations = _searchProvider.FindLocation(model.Location.Trim());

                if (suggestedLocations.HasError())
                {
                    return GetMediatorResponse(Codes.TraineeshipSearch.Results.HasError, new TraineeshipSearchResponseViewModel { VacancySearch = model }, suggestedLocations.ViewModelMessage, UserMessageLevel.Warning);
                }

                if (suggestedLocations.Locations.Any())
                {
                    var location = suggestedLocations.Locations.First();

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

            // TODO: AG: MEDIATORS: test location result validation.
            var locationResult = _searchLocationValidator.Validate(model);

            if (!locationResult.IsValid)
            {
                return GetMediatorResponse(Codes.TraineeshipSearch.Results.Ok, new TraineeshipSearchResponseViewModel { VacancySearch = model });
            }

            var traineeshipSearchResponseViewModel = _searchProvider.FindVacancies(model);

            traineeshipSearchResponseViewModel.VacancySearch.SortTypes = GetSortTypes(model.SortType);

            return GetMediatorResponse(Codes.TraineeshipSearch.Results.Ok, traineeshipSearchResponseViewModel);
        }

        public MediatorResponse<VacancyDetailViewModel> Details(string vacancyIdString, Guid? candidateId, string searchReturnUrl)
        {
            int vacancyId;
            if (!TryParseVacancyId(vacancyIdString, out vacancyId))
            {
                return GetMediatorResponse<VacancyDetailViewModel>(Codes.TraineeshipSearch.Details.VacancyNotFound);
            }

            var vacancyDetailViewModel = _traineeshipVacancyDetailProvider.GetVacancyDetailViewModel(candidateId, vacancyId);

            if (vacancyDetailViewModel == null)
            {
                return GetMediatorResponse<VacancyDetailViewModel>(Codes.TraineeshipSearch.Details.VacancyNotFound);
            }

            if (vacancyDetailViewModel.HasError())
            {
                return GetMediatorResponse(Codes.TraineeshipSearch.Details.VacancyHasError, vacancyDetailViewModel, vacancyDetailViewModel.ViewModelMessage, UserMessageLevel.Warning);
            }

            var distance = UserDataProvider.Pop(UserDataItemNames.VacancyDistance);
            var lastVacancyId = UserDataProvider.Pop(UserDataItemNames.LastViewedVacancyId);

            if (HasToPopulateDistance(vacancyId, distance, lastVacancyId))
            {
                vacancyDetailViewModel.Distance = distance;
                UserDataProvider.Push(UserDataItemNames.VacancyDistance, distance);
            }

            vacancyDetailViewModel.SearchReturnUrl = searchReturnUrl;

            UserDataProvider.Push(UserDataItemNames.LastViewedVacancyId, vacancyId.ToString(CultureInfo.InvariantCulture));

            return GetMediatorResponse(Codes.TraineeshipSearch.Details.Ok, vacancyDetailViewModel);
        }
    }
}