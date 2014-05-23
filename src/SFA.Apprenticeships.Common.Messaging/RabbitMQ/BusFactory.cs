namespace SFA.Apprenticeships.Common.Messaging.RabbitMQ
{
    using EasyNetQ;
    using SFA.Apprenticeships.Common.Configuration.Messaging;
    using SFA.Apprenticeships.Common.Messaging.Interfaces;
    using SFA.Apprenticeships.Common.Messaging.ServiceOverrides;

    internal class BusFactory
    {
        protected static readonly IRabbitMqServiceProvider CustomServiceProvider;

        static BusFactory()
        {
            CustomServiceProvider = new CustomServiceProvider();
        }

        public static IBus CreateBus(string rabbitHost)
        {
            var rabbitMqHostConfiguration = RabbitMqHostsConfiguration.Instance.RabbitHosts[rabbitHost];

            var rabbitBus = RabbitHutch.CreateBus(
                                    rabbitMqHostConfiguration.ConnectionString, 
                                    CustomServiceProvider.RegisterCustomServices());
            return rabbitBus;
        }
    }
}
