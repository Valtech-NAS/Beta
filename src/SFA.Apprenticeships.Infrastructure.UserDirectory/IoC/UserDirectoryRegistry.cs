namespace SFA.Apprenticeships.Infrastructure.UserDirectory.IoC
{
    using SFA.Apprenticeships.Application.Authentication;
    using SFA.Apprenticeships.Infrastructure.UserDirectory.ActiveDirectory;
    using SFA.Apprenticeships.Infrastructure.UserDirectory.Configuration;
    using StructureMap.Configuration.DSL;

    public class UserDirectoryRegistry : Registry
    {
        public UserDirectoryRegistry()
        {
            For<ActiveDirectoryServer>().Use<ActiveDirectoryServer>();
            For<ActiveDirectoryChangePassword>().Use<ActiveDirectoryChangePassword>();
            For<ActiveDirectoryConfiguration>().Singleton().Use(ActiveDirectoryConfiguration.Instance);
            For<IUserDirectoryProvider>().Use<ActiveDirectoryUserDirectoryProvider>();
            //For<IUserDirectoryProvider>().Use<UserDirectoryProvider>();
            //For<IPasswordHash>().Use<PasswordHash>();
        }
    }
}