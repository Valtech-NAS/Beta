namespace SFA.Apprenticeships.Domain.Entities.Locations
{
    public class GeoPoint
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public override string ToString()
        {
            return string.Format("Latitude:{0}, Longitude:{1}", Latitude, Longitude);
        }
    }
}
