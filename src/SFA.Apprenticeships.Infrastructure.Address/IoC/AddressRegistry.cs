namespace SFA.Apprenticeships.Infrastructure.Address.IoC
{
    using System;
    using Application.Interfaces.Locations;
    using StructureMap.Configuration.DSL;

    public class AddressRegistry : Registry
    {
        public AddressRegistry()
        {
            For<IAddressSearchProvider>().Use<AddressSearchProvider>();
        }
    }
}
