namespace SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch
{
    using Application.Interfaces.Search;
    using FluentValidation.Attributes;
    using SFA.Apprenticeships.Web.Candidate.Validators;

    [Validator(typeof(VacancySearchValidator))]
    public class VacancySearchViewModel
    {
        public VacancySearchViewModel() { }

        public VacancySearchViewModel(VacancySearchViewModel viewModel)
        {
            Keywords = viewModel.Keywords;
            Location = viewModel.Location;
            Longitude = viewModel.Longitude;
            Latitude = viewModel.Latitude;
            WithinDistance = viewModel.WithinDistance;
            PageNumber = viewModel.PageNumber;
            SortType = viewModel.SortType;
        }

        private int _pageNumber = 1;

        public string Keywords { get; set; }

        public string Location { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }

        public int WithinDistance { get; set; }

        public VacancySortType SortType { get; set; }

        public int PageNumber
        {
            get { return _pageNumber; }
            set { _pageNumber = value; }
        }
    }
}