namespace SFA.Apprenticeships.Infrastructure.UserDirectory.IoC
{
    using Configuration;
    using StructureMap.Configuration.DSL;
    using ActiveDirectory;

    public class UserDirectoryRegistry : Registry
    {
        public UserDirectoryRegistry()
        {
            For<ActiveDirectoryServer>().Use<ActiveDirectoryServer>();
            For<ActiveDirectoryChangePassword>().Use<ActiveDirectoryChangePassword>();
            For<ActiveDirectoryConfiguration>().Singleton().Use(ActiveDirectoryConfiguration.Instance);
        }
    }
}
