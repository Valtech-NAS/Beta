namespace SFA.Apprenticeships.Application.Interfaces
{
    using System.Collections.Generic;
    using Domain.Entities;

    public interface ILocationSearchService
    {
        IEnumerable<Location> FindAddress(string postcode);
    }

    
}