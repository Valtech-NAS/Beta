namespace SFA.Apprenticeships.Infrastructure.RabbitMq.Configuration
{
    using System.Configuration;

    public class RabbitMqHostConfiguration : ConfigurationElement, IRabbitMqHostConfiguration
    {
        private const string NameConst = "Name";
        private const string VirtualHostConst = "VirtualHost";
        private const string UserNameConst = "UserName";
        private const string PasswordConst = "Password";
        private const string PortConst = "Port";
        private const string HostNameConst = "HostName";
        private const string DurableConst = "Durable";
        private const string HeartBeatSecondsConst = "HeartBeatSeconds";
        private const string PreFetchCountConst = "PreFetchCount";
        private const string OutputEasyNetQLogsToNLogInternalConst = "OutputEasyNetQLogsToNLogInternal";
        private const string NodeCountConst = "NodeCount";
        private const string QueueWarningLimitConst = "QueueWarningLimitCount";

        [ConfigurationProperty(NameConst, IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this[NameConst]; }
            set { this[NameConst] = value; }
        }

        [ConfigurationProperty(VirtualHostConst, IsRequired = false, IsKey = false, DefaultValue = "/")]
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

        [ConfigurationProperty(HostNameConst, IsRequired = true, IsKey = false)]
        public string HostName
        {
            get { return (string)this[HostNameConst]; }
            set { this[HostNameConst] = value; }
        }

        [ConfigurationProperty(DurableConst, IsRequired = true, IsKey = false, DefaultValue = true)]
        public bool Durable
        {
            get { return (bool)this[DurableConst]; }
            set { this[DurableConst] = value; }
        }

        [ConfigurationProperty(HeartBeatSecondsConst, IsRequired = false, IsKey = false, DefaultValue = (ushort)580)]
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

        [ConfigurationProperty(OutputEasyNetQLogsToNLogInternalConst, IsRequired = false, IsKey = false, DefaultValue = false)]
        public bool OutputEasyNetQLogsToNLogInternal
        {
            get { return (bool)this[OutputEasyNetQLogsToNLogInternalConst]; }
            set { this[OutputEasyNetQLogsToNLogInternalConst] = value; }
        }

        [ConfigurationProperty(NodeCountConst, IsRequired = false, IsKey = false, DefaultValue = 1)]
        public int NodeCount
        {
            get { return (int)this[NodeCountConst]; }
            set { this[NodeCountConst] = value; }
        }

        [ConfigurationProperty(QueueWarningLimitConst, IsRequired = false, IsKey = false, DefaultValue = 500)]
        public int QueueWarningLimit
        {
            get { return (int)this[QueueWarningLimitConst]; }
            set { this[QueueWarningLimitConst] = value; }
        }

        public string ConnectionString
        {
            get
            {
                return
                    string.Format(
                        "host={0};virtualHost={1};username={2};password={3};requestedHeartbeat={4};prefetchcount={5};timeout=30",
                        HostName, VirtualHost, UserName, Password, HeartBeatSeconds, PreFetchCount);
            }
        }
    }
}