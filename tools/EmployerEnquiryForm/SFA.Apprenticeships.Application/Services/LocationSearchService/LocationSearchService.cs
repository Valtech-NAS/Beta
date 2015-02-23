namespace SFA.Apprenticeships.Application.Services.LocationSearchService
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities;
    using Interfaces;

    public class LocationSearchService : ILocationSearchService 
    {
        public IEnumerable<Location> FindAddress(string postcode)
        {
            //todo: hook up with the service to search addresses by postcode 
            //return FindAddress(postcode);
            return Enumerable.Empty<Location>();
        }
    }
}