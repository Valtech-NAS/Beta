namespace SFA.Apprenticeships.Infrastructure.Address.IoC
{
    using System;
    using Application.Address;
    using Domain.Interfaces.Mapping;
    using Mappers;
    using StructureMap.Configuration.DSL;

    public class AddressRegistry : Registry
    {
        public AddressRegistry()
        {
            For<IMapper>().Use<AddressMapper>().Name = "AddressMapper";
            For<IAddressSearchProvider>().Use<AddressSearchProvider>().Ctor<IMapper>().Named("AddressMapper");
        }
    }
}
