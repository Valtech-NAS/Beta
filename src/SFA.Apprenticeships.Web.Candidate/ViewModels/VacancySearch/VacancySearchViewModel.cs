namespace SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch
{
    using System.Web.Mvc;
    using Application.Interfaces.Vacancies;
    
    public enum SearchAction
    {
        Search,
        Sort
    }

    public abstract class VacancySearchViewModel
    {
        private int _pageNumber = 1;

        protected VacancySearchViewModel() 
        {
        }

        protected VacancySearchViewModel(VacancySearchViewModel viewModel)
        {
            Location = viewModel.Location;
            Longitude = viewModel.Longitude;
            Latitude = viewModel.Latitude;
            WithinDistance = viewModel.WithinDistance;
            PageNumber = viewModel.PageNumber;
            SortType = viewModel.SortType;
            Hash = viewModel.Hash;
            ResultsPerPage = viewModel.ResultsPerPage;
        }

        public abstract string Location { get; set; }

        public double? Longitude { get; set; }

        public double? Latitude { get; set; }

        public int Hash { get; set; }

        public int WithinDistance { get; set; }

        public VacancySortType SortType { get; set; }

        public int PageNumber
        {
            get { return _pageNumber; }
            set { _pageNumber = value; }
        }

        public int LatLonLocHash()
        {
            if (!Latitude.HasValue && !Longitude.HasValue && Location == null)
            {
                return 0;
            }
            return string.Format("{0}{1}{2}", Longitude, Latitude, Location).GetHashCode();
        }

        /// <summary>
        ///     Check to allow non-js browsers to function as expected.
        ///     Without this, the lat and lon would always take precedence and updates to
        ///     the location field would never be honoured.
        /// </summary>
        public void CheckLatLonLocHash()
        {
            if (Hash != 0 && Hash != LatLonLocHash())
            {
                Longitude = null;
                Latitude = null;
            }
        }

        public SearchAction SearchAction { get; set; }

        public int ResultsPerPage { get; set; }

        public SelectList ResultsPerPageSelectList { get; set; }

        public SelectList Distances { get; set; }

        public SelectList SortTypes { get; set; }
    }
}
