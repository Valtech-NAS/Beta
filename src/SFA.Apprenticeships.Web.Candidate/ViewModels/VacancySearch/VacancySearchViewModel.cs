namespace SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch
{
    public class VacancySearchViewModel
    {
        private int _pageNumber = 1;

        public string JobTitle { get; set; }

        public string Keywords { get; set; }

        public string Location { get; set; }

        public int WithinDistance { get; set; }

        public int PageNumber
        {
            get { return _pageNumber; }
            set { _pageNumber = value; }
        }
    }
}