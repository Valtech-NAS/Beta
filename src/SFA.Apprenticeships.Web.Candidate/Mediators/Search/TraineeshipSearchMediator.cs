namespace SFA.Apprenticeships.Web.Candidate.Mediators.Search
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Apprenticeships.Application.Interfaces.Vacancies;
    using Common.Constants;
    using Common.Providers;
    using Constants;
    using Domain.Entities.Vacancies;
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
                SortType = VacancySearchSortType.Distance,
                ResultsPerPage = GetResultsPerPage()
            };

            return GetMediatorResponse(TraineeshipSearchMediatorCodes.Index.Ok, traineeshipSearchViewModel);
        }

        public MediatorResponse<TraineeshipSearchResponseViewModel> Results(TraineeshipSearchViewModel model)
        {
            UserDataProvider.Pop(CandidateDataItemNames.VacancyDistance);

            if (model.ResultsPerPage == 0)
            {
                model.ResultsPerPage = GetResultsPerPage();
            }

            UserDataProvider.Push(UserDataItemNames.ResultsPerPage, model.ResultsPerPage.ToString(CultureInfo.InvariantCulture));

            model.Distances = GetDistances();
            model.ResultsPerPageSelectList = GetResultsPerPageSelectList(model.ResultsPerPage);

            var clientResult = _searchRequestValidator.Validate(model);

            if (!clientResult.IsValid)
            {
                return GetMediatorResponse(TraineeshipSearchMediatorCodes.Results.ValidationError, new TraineeshipSearchResponseViewModel { VacancySearch = model }, clientResult);
            }

            if (!HasGeoPoint(model))
            {
                // User did not select a location from the dropdown list, provide suggested locations.
                var suggestedLocations = _searchProvider.FindLocation(model.Location.Trim());

                if (suggestedLocations.HasError())
                {
                    return GetMediatorResponse(TraineeshipSearchMediatorCodes.Results.HasError, new TraineeshipSearchResponseViewModel { VacancySearch = model }, suggestedLocations.ViewModelMessage, UserMessageLevel.Warning);
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

            var locationResult = _searchLocationValidator.Validate(model);

            if (!locationResult.IsValid)
            {
                return GetMediatorResponse(TraineeshipSearchMediatorCodes.Results.Ok, new TraineeshipSearchResponseViewModel { VacancySearch = model });
            }

            var traineeshipSearchResponseViewModel = _searchProvider.FindVacancies(model);

            traineeshipSearchResponseViewModel.VacancySearch.SortTypes = GetSortTypes(model.SortType);

            return GetMediatorResponse(TraineeshipSearchMediatorCodes.Results.Ok, traineeshipSearchResponseViewModel);
        }

        public MediatorResponse<VacancyDetailViewModel> Details(string vacancyIdString, Guid? candidateId, string searchReturnUrl)
        {
            int vacancyId;

            if (!TryParseVacancyId(vacancyIdString, out vacancyId))
            {
                return GetMediatorResponse<VacancyDetailViewModel>(TraineeshipSearchMediatorCodes.Details.VacancyNotFound);
            }

            var vacancyDetailViewModel = _traineeshipVacancyDetailProvider.GetVacancyDetailViewModel(candidateId, vacancyId);

            if (vacancyDetailViewModel == null || vacancyDetailViewModel.VacancyStatus == VacancyStatuses.Unavailable)
            {
                return GetMediatorResponse<VacancyDetailViewModel>(TraineeshipSearchMediatorCodes.Details.VacancyNotFound);
            }

            if (vacancyDetailViewModel.HasError())
            {
                return GetMediatorResponse(TraineeshipSearchMediatorCodes.Details.VacancyHasError, vacancyDetailViewModel, vacancyDetailViewModel.ViewModelMessage, UserMessageLevel.Warning);
            }

            var distance = UserDataProvider.Pop(CandidateDataItemNames.VacancyDistance);
            var lastVacancyId = UserDataProvider.Pop(CandidateDataItemNames.LastViewedVacancyId);

            if (HasToPopulateDistance(vacancyId, distance, lastVacancyId))
            {
                vacancyDetailViewModel.Distance = distance;
                UserDataProvider.Push(CandidateDataItemNames.VacancyDistance, distance);
            }

            vacancyDetailViewModel.SearchReturnUrl = searchReturnUrl;

            UserDataProvider.Push(CandidateDataItemNames.LastViewedVacancyId, vacancyId.ToString(CultureInfo.InvariantCulture));

            return GetMediatorResponse(TraineeshipSearchMediatorCodes.Details.Ok, vacancyDetailViewModel);
        }
    }
}