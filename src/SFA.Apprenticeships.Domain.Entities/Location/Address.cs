namespace SFA.Apprenticeships.Domain.Entities.Location
{
    using System;

    public class Address
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string Postcode { get; set; }
        public GeoPoint GeoPoint { get; set; }
    }
}
