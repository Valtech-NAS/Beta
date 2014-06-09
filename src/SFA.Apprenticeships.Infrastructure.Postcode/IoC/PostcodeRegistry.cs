namespace SFA.Apprenticeships.Infrastructure.Postcode.IoC
{
    using SFA.Apprenticeships.Application.Interfaces.Location;
    using StructureMap.Configuration.DSL;

    public class PostcodeRegistry : Registry
    {
        public PostcodeRegistry()
        {
            For<IPostcodeLookupProvider>().Use<PostcodeService>();
        }
    }
}
