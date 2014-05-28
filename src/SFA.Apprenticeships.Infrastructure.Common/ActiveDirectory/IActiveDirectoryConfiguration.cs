namespace SFA.Apprenticeships.Infrastructure.Common.ActiveDirectory
{
    public interface IActiveDirectoryConfiguration
    {
        string Server { get; }
        string DistinguishedName { get; }
        string Username { get; }
        string Password { get; }
        int Port { get; }
        int SslPort { get; }
    }
}
