namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Configuration
{
    using System;
    using System.Configuration;
    using Common.Configuration;

    //todo: replace once new NAS Gateway operations available
    public class LegacyServicesConfiguration : SecureConfigurationSection<LegacyServicesConfiguration>, ILegacyServicesConfiguration
    {
        private const string SystemIdConst = "SystemId";
        private const string PublicKeyConst = "PublicKey";

        public LegacyServicesConfiguration() : base("LegacyServicesConfiguration")
        {
        }

        [ConfigurationProperty(SystemIdConst, IsRequired = true, IsKey = true)]
        public Guid SystemId
        {
            get { return (Guid)this[SystemIdConst]; }
            set { this[SystemIdConst] = value; }
        }

        [ConfigurationProperty(PublicKeyConst, IsRequired = true, IsKey = false)]
        public string PublicKey
        {
            get { return (string)this[PublicKeyConst]; }
            set { this[PublicKeyConst] = value; }
        }
    }
}