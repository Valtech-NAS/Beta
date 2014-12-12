namespace SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch
{
    using System.Collections.Generic;

    public class LocationsViewModel : ViewModelBase
    {
        public LocationsViewModel()
            : this(new LocationViewModel[] { })
        {
        }

        public LocationsViewModel(string message) : base(message) { }

        public LocationsViewModel(IEnumerable<LocationViewModel> locations)
        {
            Locations = locations;
        }

        public IEnumerable<LocationViewModel> Locations { get; private set; }
    }
}
