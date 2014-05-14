namespace SFA.Apprenticeships.Common.Logging.Configuration
{
    using System.Configuration;
    using SFA.Apprenticeships.Services.Common.ActiveDirectory;
    using ConfigurationManager = SFA.Apprenticeships.Common.Configuration.ConfigurationManager;

    public class RabbitMQLoggingConfigurationSection : ConfigurationSection, IRabbitMQLoggingConfiguration
    {
        public const string ConfigSectionNameConstant = "RabbitMQLoggingConfiguration";
        private const string VirtualHostConst = "VirtualHost";
        private const string UserNameConst = "UserName";
        private const string PasswordConst = "Password";
        private const string PortConst = "Port";
        private const string RoutingKeyConst = "RoutingKey";
        private const string HostNameConst = "HostName";
        private const string ExchangeNameConst = "ExchangeName";
        private const string ExchangeTypeConst = "ExchangeType";
        private const string DurableConst = "Durable";
        private const string AppIdConst = "AppId";
        private const string HeartBeatSecondsConst = "HeartBeatSeconds";
        private const string QueueNameConst = "QueueName";
        private const string OutputEasyNetQLogsToNLogInternalConst = "OutputEasyNetQLogsToNLogInternal";

        private static readonly string ConfigFile;

        static RabbitMQLoggingConfigurationSection()
        {
            // Needs the file (path and name) for the private configuration settings file to be set in web.config.
            ConfigFile = System.Configuration.ConfigurationManager.AppSettings[ConfigurationManager.ConfigurationFileAppSetting];
        }

        public static RabbitMQLoggingConfigurationSection ConfigurationSectionDetails
        {
            get
            {
                var configMap = new ExeConfigurationFileMap {ExeConfigFilename = ConfigFile};
                var config = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
                return config.GetSection(ConfigSectionNameConstant) as RabbitMQLoggingConfigurationSection; 
            }
        }

        [ConfigurationProperty(VirtualHostConst, IsRequired = false, IsKey = true, DefaultValue = "/")]
        public string VirtualHost
        {
            get { return (string)this[VirtualHostConst]; }
            set { this[VirtualHostConst] = value; }
        }

        [ConfigurationProperty(UserNameConst, IsRequired = false, IsKey = false, DefaultValue = "guest")]
        public string UserName
        {
            get { return (string)this[UserNameConst]; }
            set { this[UserNameConst] = value; }
        }

        [ConfigurationProperty(PasswordConst, IsRequired = false, IsKey = false, DefaultValue = "guest")]
        public string Password
        {
            get { return (string)this[PasswordConst]; }
            set { this[PasswordConst] = value; }
        }

        [ConfigurationProperty(PortConst, IsRequired = false, IsKey = false, DefaultValue = 5672)]
        public ushort Port
        {
            get { return (ushort)this[PortConst]; }
            set { this[PortConst] = value; }
        }

        [ConfigurationProperty(PasswordConst, IsRequired = false, IsKey = false, DefaultValue = "{0}")]
        public string RoutingKey
        {
            get { return (string)this[RoutingKeyConst]; }
            set { this[RoutingKeyConst] = value; }
        }

        [ConfigurationProperty(HostNameConst, IsRequired = false, IsKey = false, DefaultValue = "localhost")]
        public string HostName
        {
            get { return (string)this[HostNameConst]; }
            set { this[HostNameConst] = value; }
        }

        [ConfigurationProperty(ExchangeNameConst, IsRequired = true, IsKey = false)]
        public string ExchangeName
        {
            get { return (string)this[ExchangeNameConst]; }
            set { this[ExchangeNameConst] = value; }
        }

        [ConfigurationProperty(ExchangeTypeConst, IsRequired = false, IsKey = false, DefaultValue = EasyNetQ.Topology.ExchangeType.Topic)]
        public string ExchangeType
        {
            get { return (string)this[ExchangeTypeConst]; }
            set
            {
                switch (value)
                {
                    case EasyNetQ.Topology.ExchangeType.Topic:
                    case EasyNetQ.Topology.ExchangeType.Fanout:
                    case EasyNetQ.Topology.ExchangeType.Header:
                    case EasyNetQ.Topology.ExchangeType.Direct:
                        this[ExchangeTypeConst] = value;
                        break;
                    default:
                        throw new ConfigurationErrorsException("ExchangeType not valid ExchangeType, see EasyNetQ.Topology.ExchangeType for valid values");
                }
            }
        }

        [ConfigurationProperty(DurableConst, IsRequired = true, IsKey = false, DefaultValue = true)]
        public bool Durable
        {
            get { return (bool)this[DurableConst]; }
            set { this[DurableConst] = value; }
        }

        [ConfigurationProperty(AppIdConst, IsRequired = true, IsKey = false)]
        public string AppId
        {
            get { return (string)this[AppIdConst]; }
            set { this[AppIdConst] = value; }
        }

        [ConfigurationProperty(HeartBeatSecondsConst, IsRequired = false, IsKey = false, DefaultValue = 3)]
        public ushort HeartBeatSeconds
        {
            get { return (ushort)this[HeartBeatSecondsConst]; }
            set { this[HeartBeatSecondsConst] = value; }
        }

        [ConfigurationProperty(QueueNameConst, IsRequired = true, IsKey = false)]
        public string QueueName
        {
            get { return (string)this[QueueNameConst]; }
            set { this[QueueNameConst] = value; }
        }

        [ConfigurationProperty(OutputEasyNetQLogsToNLogInternalConst, IsRequired = false, IsKey = false, DefaultValue = false)]
        public bool OutputEasyNetQLogsToNLogInternal
        {
            get { return (bool)this[OutputEasyNetQLogsToNLogInternalConst]; }
            set { this[OutputEasyNetQLogsToNLogInternalConst] = value; }
        }
    }
}