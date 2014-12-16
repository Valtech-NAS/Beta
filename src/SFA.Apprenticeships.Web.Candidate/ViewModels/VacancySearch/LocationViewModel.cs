namespace SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch
{
    using System;

    [Serializable]
    public class LocationViewModel
    {
        public LocationViewModel() { }

        public LocationViewModel(ApprenticeshipSearchViewModel model)
        {
            if (model != null)
            {
                Name = model.Location;
                Longitude = model.Longitude.GetValueOrDefault();
                Latitude = model.Latitude.GetValueOrDefault();
            }
        }

        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}