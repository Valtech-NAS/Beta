namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Collections;
    using System.Web.Mvc;
    using SFA.Apprenticeships.Application.Interfaces.Vacancies;
    using SFA.Apprenticeships.Domain.Interfaces.Configuration;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch;
    using SFA.Apprenticeships.Web.Common.Constants;

    public abstract class VacancySearchController : CandidateControllerBase
    {
        private readonly int _vacancyResultsPerPage;

        protected VacancySearchController(IConfigurationManager configManager)
        {
            _vacancyResultsPerPage = configManager.GetAppSetting<int>("VacancyResultsPerPage");
        }

        #region Helpers

        //TODO: DFSW Remove once mediators are in place for all controllers and ViewBag is no longer used
        protected int GetResultsPerPage()
        {
            int resultsPerPage;
            if (!int.TryParse(UserData.Get(UserDataItemNames.ResultsPerPage), out resultsPerPage))
            {
                resultsPerPage = _vacancyResultsPerPage;
            }

            return resultsPerPage;
        }

        //TODO: DFSW Remove once mediators are in place for all controllers and ViewBag is no longer used
        protected void PopulateResultsPerPage(VacancySearchViewModel viewModel, int selectedValue)
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

            viewModel.ResultsPerPageSelectList = resultsPerPage;
        }

        //TODO: DFSW Remove once mediators are in place for all controllers and ViewBag is no longer used
        protected void PopulateDistances(VacancySearchViewModel viewModel, int selectedValue = 2)
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

            viewModel.Distances = distances;
        }

        //TODO: DFSW Remove once mediators are in place for all controllers and ViewBag is no longer used
        protected void PopulateSortType(VacancySearchViewModel viewModel, VacancySortType selectedSortType = VacancySortType.Distance,
            string keywords = null, bool isLocalLocationType = true)
        {
            var sortTypeOptions = new ArrayList();

            if (!string.IsNullOrWhiteSpace(keywords))
            {
                sortTypeOptions.Add(new {SortType = VacancySortType.Relevancy, Name = "Best Match"});
            }

            sortTypeOptions.Add(new {SortType = VacancySortType.ClosingDate, Name = "Closing Date"});

            if (isLocalLocationType)
            {
                sortTypeOptions.Add(new {SortType = VacancySortType.Distance, Name = "Distance"});
            }

            ModelState.Remove("SortType");

            var sortTypes = new SelectList(
                sortTypeOptions,
                "SortType",
                "Name",
                selectedSortType
                );

            viewModel.SortTypes = sortTypes;
        }

        //TODO: DFSW Remove once mediators are in place for all controllers and ViewBag is no longer used
        protected static bool HasGeoPoint(VacancySearchViewModel searchViewModel)
        {
            searchViewModel.CheckLatLonLocHash();

            return searchViewModel.Latitude.HasValue && searchViewModel.Longitude.HasValue;
        }

        //TODO: DFSW Remove once mediators are in place for all controllers and ViewBag is no longer used
        protected static bool HasToPopulateDistance(int id, string distance, string lastVacancyId)
        {
            return !string.IsNullOrWhiteSpace(distance)
                   && !string.IsNullOrWhiteSpace(lastVacancyId)
                   && int.Parse(lastVacancyId) == id;
        }

        #endregion
    }
}