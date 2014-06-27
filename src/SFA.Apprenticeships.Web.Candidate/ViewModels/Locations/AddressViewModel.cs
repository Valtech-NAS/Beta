namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Locations
{
    using Domain.Entities.Locations;

    public class AddressViewModel
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string Postcode { get; set; }
        public GeoPoint GeoPoint { get; set; }
    }
}