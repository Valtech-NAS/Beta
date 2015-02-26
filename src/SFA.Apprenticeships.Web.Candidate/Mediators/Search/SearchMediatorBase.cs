namespace SFA.Apprenticeships.Web.Candidate.Mediators.Search
{
    using System.Collections;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using Apprenticeships.Application.Interfaces.Vacancies;
    using Common.Constants;
    using Common.Providers;
    using Domain.Interfaces.Configuration;
    using ViewModels.VacancySearch;

    public abstract class SearchMediatorBase : MediatorBase
    {
        private static readonly Regex VacancyIdRegex = new Regex(@"(^\d+).*");

        private readonly int _vacancyResultsPerPage;

        protected readonly IUserDataProvider UserDataProvider;

        protected SearchMediatorBase(IConfigurationManager configManager, IUserDataProvider userDataProvider)
        {
            _vacancyResultsPerPage = configManager.GetAppSetting<int>("VacancyResultsPerPage");

            UserDataProvider = userDataProvider;
        }

        protected static SelectList GetDistances()
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
                    new {WithinDistance = 40, Name = "40 miles"},
                    new {WithinDistance = 0, Name = "England"}
                },
                "WithinDistance",
                "Name"
                );

            return distances;
        }

        public static SelectList GetSortTypes(VacancySearchSortType selectedSortType = VacancySearchSortType.Distance, string keywords = null, bool isLocalLocationType = true)
        {
            var sortTypeOptions = new ArrayList();

            if (!string.IsNullOrWhiteSpace(keywords))
            {
                sortTypeOptions.Add(new { SortType = VacancySearchSortType.Relevancy, Name = "Best match" });
            }

            sortTypeOptions.Add(new { SortType = VacancySearchSortType.ClosingDate, Name = "Closing date" });

            if (isLocalLocationType)
            {
                sortTypeOptions.Add(new { SortType = VacancySearchSortType.Distance, Name = "Distance" });
            }

            sortTypeOptions.Add(new {SortType = VacancySearchSortType.RecentlyAdded, Name = "Recently added"});

            var sortTypes = new SelectList(
                sortTypeOptions,
                "SortType",
                "Name",
                selectedSortType
                );

            return sortTypes;
        }

        public static SelectList GetResultsPerPageSelectList(int selectedValue)
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

        protected static bool TryParseVacancyId(string vacancyIdString, out int vacancyId)
        {
            vacancyId = 0;

            if (string.IsNullOrWhiteSpace(vacancyIdString)) return false;

            var match = VacancyIdRegex.Match(vacancyIdString);

            if (!match.Success) return false;
            
            vacancyIdString = match.Groups[1].Value;
            
            return int.TryParse(vacancyIdString, out vacancyId);
        }
    }
}