namespace SFA.Apprenticeships.Common.Messaging
{
    using EasyNetQ;
    using SFA.Apprenticeships.Common.Configuration.Messaging;
    using SFA.Apprenticeships.Common.Messaging.Interfaces;
    using SFA.Apprenticeships.Common.Messaging.ServiceOverrides;

    public class Transport
    {
        private static readonly IRabbitMqServiceProvider CustomServiceProvider;
        private static readonly IRabbitMQConfiguration RabbitMqConfiguration;

        static Transport()
        {
            CustomServiceProvider = new CustomServiceProvider();
            RabbitMqConfiguration = RabbitMQConfigurationSection.Instance;
        }   

        public static IBus CreateBus()
        {
            var rabbitBus = RabbitHutch.CreateBus(RabbitMqConfiguration.ConnectionString, CustomServiceProvider.RegisterCustomServices());
            return rabbitBus;
        }
    }
}
