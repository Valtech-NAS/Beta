namespace SFA.Apprenticeships.Web.Candidate.Mediators.Traineeships
{
    using System;
    using System.Globalization;
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
        private readonly TraineeshipSearchViewModelClientValidator _searchRequestValidator;
        private readonly TraineeshipSearchViewModelLocationValidator _searchLocationValidator;

        public TraineeshipSearchMediator(
            IConfigurationManager configManager,
            ISearchProvider searchProvider,
            ITraineeshipVacancyDetailProvider traineeshipVacancyDetailProvider,
            IUserDataProvider userDataProvider,
            TraineeshipSearchViewModelClientValidator searchRequestValidator,
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
            throw new NotImplementedException();
        }

        public MediatorResponse<VacancyDetailViewModel> Details(int vacancyId, Guid? candidateId, string searchReturnUrl)
        {
            var vacancyDetailViewModel = _traineeshipVacancyDetailProvider.GetVacancyDetailViewModel(candidateId, vacancyId);

            if (vacancyDetailViewModel == null)
            {
                return GetMediatorResponse<VacancyDetailViewModel>(Codes.TraineeshipSearch.Details.VacancyNotFound, null);
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