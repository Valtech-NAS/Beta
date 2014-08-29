namespace SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch
{
    using System.Collections.Generic;
    using Constants.ViewModels;

    public class LocationsViewModel
    {
        public LocationsViewModel()
            : this(new LocationViewModel[] { })
        {
        }

        public LocationsViewModel(IEnumerable<LocationViewModel> locations)
        {
            Locations = locations;
        }
        
        public LocationsViewModel(string message)
        {
            ViewModelMessage = message;
        }

        public IEnumerable<LocationViewModel> Locations { get; private set; }

        // TODO: AG: US333: refactor into an object on new ViewModelBase class.
        public string ViewModelMessage { get; private set; }
    }
}
