namespace SFA.Apprenticeships.Infrastructure.RabbitMq.RabbitMQ
{
    using EasyNetQ;
    using Configuration;
    using Interfaces;
    using ServiceOverrides;

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
