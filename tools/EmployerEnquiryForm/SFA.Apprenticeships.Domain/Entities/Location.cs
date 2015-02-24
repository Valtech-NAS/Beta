namespace SFA.Apprenticeships.Domain.Entities
{
    public class Location
    {
        public Address Address { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}