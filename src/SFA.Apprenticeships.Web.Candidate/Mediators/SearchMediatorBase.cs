namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    using System.Collections;
    using System.Web.Mvc;
    using Application.Interfaces.Vacancies;
    using Common.Constants;
    using Common.Providers;
    using Domain.Interfaces.Configuration;
    using ViewModels.VacancySearch;

    public abstract class SearchMediatorBase : MediatorBase
    {
        private readonly int _vacancyResultsPerPage;

        protected readonly IUserDataProvider UserDataProvider;

        protected SearchMediatorBase(IConfigurationManager configManager, IUserDataProvider userDataProvider)
        {
            _vacancyResultsPerPage = configManager.GetAppSetting<int>("VacancyResultsPerPage");

            UserDataProvider = userDataProvider;
        }

        protected static SelectList GetDistances(int selectedValue = 5)
        {
            var distances = new SelectList(
                new[]
                {
                    new {WithinDistance = 2, Name = "2 miles"},
                    new {WithinDistance = 5, Name = "5 miles"},
                    new {WithinDistance = 10, Name = "10 miles"},
                    new {WithinDistance = 15, Name = "15 miles"},
                    new {WithinDistance = 20, Name = "20 miles"},
                    new {WithinDistance = 30, Name = "30 miles"},
                    new {WithinDistance = 40, Name = "40 miles"}
                },
                "WithinDistance",
                "Name",
                selectedValue
                );

            return distances;
        }

        protected SelectList GetSortTypes(VacancySortType selectedSortType = VacancySortType.Distance, string keywords = null, bool isLocalLocationType = true)
        {
            var sortTypeOptions = new ArrayList();

            if (!string.IsNullOrWhiteSpace(keywords))
            {
                sortTypeOptions.Add(new { SortType = VacancySortType.Relevancy, Name = "Best Match" });
            }

            sortTypeOptions.Add(new { SortType = VacancySortType.ClosingDate, Name = "Closing Date" });

            if (isLocalLocationType)
            {
                sortTypeOptions.Add(new { SortType = VacancySortType.Distance, Name = "Distance" });
            }

            var sortTypes = new SelectList(
                sortTypeOptions,
                "SortType",
                "Name",
                selectedSortType
                );

            return sortTypes;
        }

        protected static SelectList GetResultsPerPageSelectList(int selectedValue)
        {
            var resultsPerPage = new SelectList(
                new[]
                {
                    new {ResultsPerPage = 5, Name = "5 per page"},
                    new {ResultsPerPage = 10, Name = "10 per page"},
                    new {ResultsPerPage = 25, Name = "25 per page"},
                    new {ResultsPerPage = 50, Name = "50 per page"}
                },
                "ResultsPerPage",
                "Name",
                selectedValue
                );

            return resultsPerPage;
        }

        protected int GetResultsPerPage()
        {
            int resultsPerPage;
            if (!int.TryParse(UserDataProvider.Get(UserDataItemNames.ResultsPerPage), out resultsPerPage))
            {
                resultsPerPage = _vacancyResultsPerPage;
            }

            return resultsPerPage;
        }

        protected static bool HasGeoPoint(VacancySearchViewModel searchViewModel)
        {
            searchViewModel.CheckLatLonLocHash();

            return searchViewModel.Latitude.HasValue && searchViewModel.Longitude.HasValue;
        }

        protected static bool HasToPopulateDistance(int id, string distance, string lastVacancyId)
        {
            return !string.IsNullOrWhiteSpace(distance)
                   && !string.IsNullOrWhiteSpace(lastVacancyId)
                   && int.Parse(lastVacancyId) == id;
        }
    }
}