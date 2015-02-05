﻿namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Configuration
{
    using System;
    using System.Configuration;
    using Common.Configuration;

    public class LegacyServicesConfiguration : SecureConfigurationSection<LegacyServicesConfiguration>, ILegacyServicesConfiguration
    {
        private const string SystemIdConst = "SystemId";
        private const string PublicKeyConst = "PublicKey";
        private const string ApplicationStatusExtractWindowConst = "ApplicationStatusExtractWindow";

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

        [ConfigurationProperty(ApplicationStatusExtractWindowConst, IsRequired = true, IsKey = false)]
        public int ApplicationStatusExtractWindow
        {
            get { return (int)this[ApplicationStatusExtractWindowConst]; }
            set { this[ApplicationStatusExtractWindowConst] = value; }
        }
    }
}