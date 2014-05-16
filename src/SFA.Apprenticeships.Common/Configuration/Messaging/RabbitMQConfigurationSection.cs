namespace SFA.Apprenticeships.Common.Configuration.Messaging
{
    using System.Configuration;
    using SFA.Apprenticeships.Common.Configuration;

    public class RabbitMQConfigurationSection : SecureConfigurationSection<RabbitMQConfigurationSection>, IRabbitMQConfiguration
    {
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
        private const string PreFetchCountConst = "PreFetchCount";

        private const string OutputEasyNetQLogsToNLogInternalConst = "OutputEasyNetQLogsToNLogInternal";

        public RabbitMQConfigurationSection() : base("RabbitMQConfiguration")
        {
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

        [ConfigurationProperty(PortConst, IsRequired = false, IsKey = false, DefaultValue = (ushort)5672)]
        public ushort Port
        {
            get { return (ushort)this[PortConst]; }
            set { this[PortConst] = value; }
        }

        [ConfigurationProperty(RoutingKeyConst, IsRequired = false, IsKey = false, DefaultValue = "{0}")]
        public string RoutingKey
        {
            get { return (string)this[RoutingKeyConst]; }
            set { this[RoutingKeyConst] = value; }
        }

        [ConfigurationProperty(HostNameConst, IsRequired = true, IsKey = false)]
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

        [ConfigurationProperty(HeartBeatSecondsConst, IsRequired = false, IsKey = false, DefaultValue = (ushort)3)]
        public ushort HeartBeatSeconds
        {
            get { return (ushort)this[HeartBeatSecondsConst]; }
            set { this[HeartBeatSecondsConst] = value; }
        }

        [ConfigurationProperty(PreFetchCountConst, IsRequired = false, IsKey = false, DefaultValue = (ushort)10)]
        public ushort PreFetchCount
        {
            get { return (ushort)this[PreFetchCountConst]; }
            set { this[PreFetchCountConst] = value; }
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

        public string ConnectionString
        {
            get
            {
                return
                    string.Format(
                        "host={0};virtualHost={1};username={2};password={3};requestedHeartbeat={4};prefetchcount={5};persistentMessages={6}",
                        HostName, VirtualHost, UserName, Password, HeartBeatSeconds, PreFetchCount, true);
            }
        }
    }
}