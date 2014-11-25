namespace SFA.Apprenticeships.Infrastructure.UserDirectory.IoC
{
    using Application.Authentication;
    using Hash;
    using StructureMap.Configuration.DSL;

    public class UserDirectoryRegistry : Registry
    {
        public UserDirectoryRegistry()
        {
            For<IUserDirectoryProvider>().Use<UserDirectoryProvider>();
            For<IPasswordHash>().Use<PasswordHash>();
        }
    }
}