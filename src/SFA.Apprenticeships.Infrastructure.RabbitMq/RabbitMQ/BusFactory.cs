namespace SFA.Apprenticeships.Infrastructure.RabbitMq.RabbitMQ
{
    using EasyNetQ;
    using SFA.Apprenticeships.Infrastructure.RabbitMq.Configuration;
    using SFA.Apprenticeships.Infrastructure.RabbitMq.Interfaces;
    using SFA.Apprenticeships.Infrastructure.RabbitMq.ServiceOverrides;

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
