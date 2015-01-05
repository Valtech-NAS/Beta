namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Application.Interfaces.Vacancies;
    using Common.Constants;
    using Common.Providers;
    using Domain.Entities.Vacancies.Apprenticeships;
    using FluentValidation.Mvc;
    using Microsoft.Ajax.Utilities;
    using Providers;
    using Validators;
    using ViewModels;
    using ViewModels.VacancySearch;

    public class ApprenticeshipSearchMediator : IApprenticeshipSearchMediator
    {
        private readonly ISearchProvider _searchProvider;
        private readonly IApprenticeshipVacancyDetailProvider _apprenticeshipVacancyDetailProvider;
        private readonly IUserDataProvider _userDataProvider;
        private readonly ApprenticeshipSearchViewModelClientValidator _searchRequestValidator;
        private readonly ApprenticeshipSearchViewModelLocationValidator _searchLocationValidator;

        public ApprenticeshipSearchMediator(
            ISearchProvider searchProvider,
            IApprenticeshipVacancyDetailProvider apprenticeshipVacancyDetailProvider,
            IUserDataProvider userDataProvider,
            ApprenticeshipSearchViewModelClientValidator searchRequestValidator,
            ApprenticeshipSearchViewModelLocationValidator searchLocationValidator)
        {
            _searchProvider = searchProvider;
            _apprenticeshipVacancyDetailProvider = apprenticeshipVacancyDetailProvider;
            _userDataProvider = userDataProvider;
            _searchRequestValidator = searchRequestValidator;
            _searchLocationValidator = searchLocationValidator;
        }

        public MediatorResponse<ApprenticeshipSearchViewModel> Index()
        {
            throw new NotImplementedException();
        }

        public MediatorResponse<ApprenticeshipSearchResponseViewModel> Search(ApprenticeshipSearchViewModel model, ModelStateDictionary modelState)
        {
            if (model.SearchAction == SearchAction.Search && model.LocationType != ApprenticeshipLocationType.NonNational)
            {
                model.LocationType = ApprenticeshipLocationType.NonNational;
            }

            if (model.LocationType == ApprenticeshipLocationType.NonNational && model.SortType == VacancySortType.Relevancy &&
                string.IsNullOrWhiteSpace(model.Keywords))
            {
                modelState.Remove("SortType");
                model.SortType = VacancySortType.Distance;
            }

            if (model.LocationType == ApprenticeshipLocationType.National && string.IsNullOrWhiteSpace(model.Keywords) &&
                model.SortType != VacancySortType.ClosingDate)
            {
                modelState.Remove("SortType");
                model.SortType = VacancySortType.ClosingDate;
            }

            var clientResult = _searchRequestValidator.Validate(model);

            if (!clientResult.IsValid)
            {
                modelState.Clear();
                clientResult.AddToModelState(modelState, string.Empty);

                return GetMediatorResponse("results", new ApprenticeshipSearchResponseViewModel { VacancySearch = model });
            }

            if (!HasGeoPoint(model))
            {
                // User did not select a location from the dropdown list, provide suggested locations.
                var suggestedLocations = _searchProvider.FindLocation(model.Location.Trim());

                if (suggestedLocations.HasError())
                {
                    modelState.Clear();
                    return GetMediatorResponse("results", new ApprenticeshipSearchResponseViewModel { VacancySearch = model }, suggestedLocations.ViewModelMessage, UserMessageLevel.Warning);
                }

                if (suggestedLocations.Locations.Any())
                {
                    var location = suggestedLocations.Locations.First();

                    modelState.Remove("Location");
                    modelState.Remove("Latitude");
                    modelState.Remove("Longitude");

                    model.Location = location.Name;
                    model.Latitude = location.Latitude;
                    model.Longitude = location.Longitude;

                    //TODO: Add to view model
                    var locationSearches = suggestedLocations.Locations.Skip(1).Select(each =>
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
                modelState.Clear();
                return GetMediatorResponse("results", new ApprenticeshipSearchResponseViewModel { VacancySearch = model });
            }

            var results = _searchProvider.FindVacancies(model);

            if (results.HasError())
            {
                modelState.Clear();
                return GetMediatorResponse("results", new ApprenticeshipSearchResponseViewModel { VacancySearch = model }, results.ViewModelMessage, UserMessageLevel.Warning);
            }

            if (model.SearchAction == SearchAction.Search && results.TotalLocalHits > 0)
            {
                results.VacancySearch.LocationType = ApprenticeshipLocationType.NonNational;
            }

            if (results.VacancySearch.LocationType == ApprenticeshipLocationType.National)
            {
                //TODO: Uncomment and make compatible with new pattern
                //PopulateSortType(model.SortType, model.Keywords, false);
            }
            else
            {
                //TODO: Uncomment and make compatible with new pattern
                //PopulateSortType(model.SortType, model.Keywords);
            }

            return GetMediatorResponse("results", results);
        }

        public MediatorResponse<VacancyDetailViewModel> Details(int id, Guid? candidateId, string searchReturnUrl)
        {
            var vacancyDetailViewModel = _apprenticeshipVacancyDetailProvider.GetVacancyDetailViewModel(candidateId, id);

            if (vacancyDetailViewModel == null)
            {
                return GetMediatorResponse<VacancyDetailViewModel>(Codes.ApprenticeshipSearch.Details.VacancyNotFound, null);
            }

            if (vacancyDetailViewModel.HasError())
            {
                return GetMediatorResponse(Codes.ApprenticeshipSearch.Details.VacancyHasError, vacancyDetailViewModel, vacancyDetailViewModel.ViewModelMessage, UserMessageLevel.Warning);
            }

            var distance = _userDataProvider.Pop(UserDataItemNames.VacancyDistance);
            var lastVacancyId = _userDataProvider.Pop(UserDataItemNames.LastViewedVacancyId);

            if (HasToPopulateDistance(id, distance, lastVacancyId))
            {
                vacancyDetailViewModel.Distance = distance;
                _userDataProvider.Push(UserDataItemNames.VacancyDistance, distance);
            }

            vacancyDetailViewModel.SearchReturnUrl = searchReturnUrl;

            _userDataProvider.Push(UserDataItemNames.LastViewedVacancyId, id.ToStringInvariant());

            return GetMediatorResponse(Codes.ApprenticeshipSearch.Details.Ok, vacancyDetailViewModel);
        }

        private static bool HasGeoPoint(VacancySearchViewModel searchViewModel)
        {
            searchViewModel.CheckLatLonLocHash();

            return searchViewModel.Latitude.HasValue && searchViewModel.Longitude.HasValue;
        }

        private static bool HasToPopulateDistance(int id, string distance, string lastVacancyId)
        {
            return !string.IsNullOrWhiteSpace(distance)
                   && !string.IsNullOrWhiteSpace(lastVacancyId)
                   && int.Parse(lastVacancyId) == id;
        }

        private static MediatorResponse<T> GetMediatorResponse<T>(string code, T viewModel) where T : ViewModelBase
        {
            var response = new MediatorResponse<T>
            {
                Code = code,
                ViewModel = viewModel
            };

            return response;
        }

        private static MediatorResponse<T> GetMediatorResponse<T>(string code, T viewModel, string message, UserMessageLevel level) where T : ViewModelBase
        {
            var response = new MediatorResponse<T>
            {
                Code = code,
                ViewModel = viewModel,
                Message = new MediatorResponseMessage
                {
                    Message = message,
                    Level = level
                }
            };

            return response;
        }
    }
}