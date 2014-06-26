namespace SFA.Apprenticeships.Infrastructure.LocationLookup.IoC
{
    using System;
    using Application.Interfaces.Locations;
    using StructureMap.Configuration.DSL;

    public class LocationLookupRegistry : Registry
    {
        public LocationLookupRegistry()
        {
            For<ILocationLookupProvider>().Use<LocationLookupProvider>();
        }
    }
}
