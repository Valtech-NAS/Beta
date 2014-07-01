namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Locations
{
    using System;

    [Serializable]
    public class AddressViewModel
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string Postcode { get; set; }
        public GeoPointViewModel GeoPoint { get; set; }
    }
}