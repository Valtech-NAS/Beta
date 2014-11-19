using SFA.Apprenticeships.Application.Authentication;

namespace SFA.Apprenticeships.Infrastructure.UserDirectory.IoC
{
    using Configuration;
    using Hash;
    using StructureMap.Configuration.DSL;
    using ActiveDirectory;

    public class UserDirectoryRegistry : Registry
    {
        public UserDirectoryRegistry()
        {
            /*For<ActiveDirectoryServer>().Use<ActiveDirectoryServer>();
            For<ActiveDirectoryChangePassword>().Use<ActiveDirectoryChangePassword>();
            For<ActiveDirectoryConfiguration>().Singleton().Use(ActiveDirectoryConfiguration.Instance);
            For<IUserDirectoryProvider>().Use<ActiveDirectoryUserDirectoryProvider>();*/
            For<IUserDirectoryProvider>().Use<UserDirectoryProvider>();
            For<IPasswordHash>().Use<PasswordHash>();
        }
    }
}
