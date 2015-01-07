namespace SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class TraineeshipSearchResponseViewModel : ViewModelBase
    {
        public TraineeshipSearchResponseViewModel(string message) : base(message)
        {
        }

        public TraineeshipSearchResponseViewModel()
        {
        }

        public long TotalHits { get; set; }

        public int PageSize { get; set; }

        public IEnumerable<TraineeshipVacancySummaryViewModel> Vacancies { get; set; }

        public TraineeshipSearchViewModel VacancySearch { get; set; }

        public int PrevPage
        {
            get
            {
                if (VacancySearch == null) return 1;

                return VacancySearch.PageNumber == 1 ? 0 : VacancySearch.PageNumber - 1;
            }
        }

        public int NextPage
        {
            get
            {
                if (VacancySearch == null) return 1;

                var pages = Pages;
                return VacancySearch.PageNumber == pages ? pages : VacancySearch.PageNumber + 1;
            }
        }

        public int Pages
        {
            get
            {
                var pages = 1;
                if (PageSize <= 0) {return pages;}
                pages = (int)TotalHits / PageSize;
                if (TotalHits % PageSize > 0) { pages++; }
                return pages;
            }
        }

        // TODO: AG: MEDIATORS: consider renaming to ResultsPerPage (might collide with another property).
        public SelectList ResultsPerPageSelectList
        {
            get { return VacancySearch.ResultsPerPageSelectList; }
        }

        public SelectList Distances
        {
            get { return VacancySearch.Distances; }
        }

        public SelectList SortTypes
        {
            get { return VacancySearch.SortTypes; }
        }

        public TraineeshipSearchViewModel[] LocationSearches
        {
            get { return VacancySearch.LocationSearches; }
        }
    }
}