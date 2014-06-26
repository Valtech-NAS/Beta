namespace SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch
{
    using System.ComponentModel.DataAnnotations;
    using Application.Interfaces.Search;
    using FluentValidation.Attributes;
    using Validators;

    [Validator(typeof(VacancySearchClientSideValidator))]
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
            Hash = viewModel.Hash;
        }

        private int _pageNumber = 1;

        [Display(Name = "Keywords (optional)", Description = "For example, mechanical engineer, retail, customer service")]
        public string Keywords { get; set; }

        [Display(Name = "Apprenticeship location", Description = "Enter postcode, town or city")]
        //[Required(ErrorMessage = "Sorry, we didn't find a match for the location. Please try again.")]
        public string Location { get; set; }

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
        /// Check to allow non-js browsers to function as expected.
        /// Without this, the lat and lon would always take precedence and updates to
        /// the location field would never be honoured.
        /// </summary>
        public void CheckLatLonLocHash()
        {
            if (Hash != 0 && Hash != LatLonLocHash())
            {
                Longitude = null;
                Latitude = null;
            }
        }
    }
}