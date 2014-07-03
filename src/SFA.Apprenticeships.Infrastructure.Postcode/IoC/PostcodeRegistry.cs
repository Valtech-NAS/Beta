namespace SFA.Apprenticeships.Infrastructure.Postcode.IoC
{
    using System;
    using Application.Interfaces.Locations;
    using StructureMap.Configuration.DSL;

    public class PostcodeRegistry : Registry
    {
        public PostcodeRegistry()
        {
            For<IPostcodeLookupProvider>().Use<PostcodeService>();
        }
    }
}
