namespace SFA.Apprenticeships.Services.Common.ActiveDirectory
{
    using System.Configuration;
    using SFA.Apprenticeships.Common.Configuration;

    public class ActiveDirectoryConfiguration : SecureConfigurationSection<ActiveDirectoryConfiguration>, IActiveDirectoryConfiguration
    {
        private const string ServerConstant = "Server";
        private const string DistinguishedNameConstant = "DistinguishedName";
        private const string AdminUsernameConst = "Username";
        private const string AdminPasswordConst = "Password";
        private const string PortConstant = "Port";
        private const string SslPortConstant = "SslPort";
        private const string DefaultValueConstant = "";

        public ActiveDirectoryConfiguration(): base("ActiveDirectoryConfiguration")
        {
        }

        [ConfigurationProperty(ServerConstant, IsRequired = true, IsKey = true, DefaultValue = DefaultValueConstant)]
        public string Server
        {
            get { return (string) this[ServerConstant]; }
            set { this[ServerConstant] = value; }
        }

        [ConfigurationProperty(DistinguishedNameConstant, IsRequired = true, DefaultValue = DefaultValueConstant)]
        public string DistinguishedName
        {
            get { return (string)this[DistinguishedNameConstant]; }
            set { this[DistinguishedNameConstant] = value; }
        }

        [ConfigurationProperty(AdminUsernameConst, IsRequired = false, DefaultValue = DefaultValueConstant)]
        public string Username
        {
            get { return (string)this[AdminUsernameConst]; }
            set { this[AdminUsernameConst] = value; }
        }

        [ConfigurationProperty(AdminPasswordConst, IsRequired = false, DefaultValue = DefaultValueConstant)]
        public string Password
        {
            get { return (string)this[AdminPasswordConst]; }
            set { this[AdminPasswordConst] = value; }
        }

        [ConfigurationProperty(PortConstant, IsRequired = false, DefaultValue = 389)]
        public int Port
        {
            get { return (int)this[PortConstant]; }
            set { this[PortConstant] = value; }
        }

        [ConfigurationProperty(SslPortConstant, IsRequired = false, DefaultValue = 636)]
        public int SslPort
        {
            get { return (int)this[SslPortConstant]; }
            set { this[SslPortConstant] = value; }
        }
    }
}