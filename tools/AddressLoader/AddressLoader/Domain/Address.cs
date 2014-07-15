namespace AddressLoader.Domain
{
    public class Address
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string Postcode { get; set; }
        public string Uprn { get; set; }
        public Location Location { get; set; }
    }
}
