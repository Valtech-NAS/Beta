namespace SFA.Apprenticeships.Domain.Entities
{
    public class Location
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string Street { get; set; }
        public string County { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Postcode { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}