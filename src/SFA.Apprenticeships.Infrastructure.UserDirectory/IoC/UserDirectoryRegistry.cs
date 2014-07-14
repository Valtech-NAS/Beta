namespace SFA.Apprenticeships.Infrastructure.UserDirectory.IoC
{
    using Configuration;
    using StructureMap.Configuration.DSL;

    public class UserDirectoryRegistry : Registry
    {
        public UserDirectoryRegistry()
        {
            For<ActiveDirectoryConfiguration>().Singleton().Use(ActiveDirectoryConfiguration.Instance);
        }
    }
}
