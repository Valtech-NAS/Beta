namespace SFA.Apprenticeships.Domain.Entities.Location
{
    using System;

    public class Location
    {
        public string Name { get; set; }
        public GeoPoint GeoPoint { get; set; }
    }
}
