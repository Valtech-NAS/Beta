using System;

namespace SFA.Apprenticeships.Domain.Entities.Locations
{
    public class Location
    {
        public string Name { get; set; }
        public GeoPoint GeoPoint { get; set; }
    }
}
