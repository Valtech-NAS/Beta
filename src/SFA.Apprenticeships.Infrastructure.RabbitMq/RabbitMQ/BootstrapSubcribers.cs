namespace SFA.Apprenticeships.Infrastructure.RabbitMq.RabbitMQ
{
    using System.Reflection;
    using EasyNetQ;
    using EasyNetQ.AutoSubscribe;
    using StructureMap;
    using Interfaces;
    using Configuration;

    internal class BootstrapSubcribers : IBootstrapSubcribers
    {
        private readonly IBus _bus;
        private readonly IRabbitMqHostConfiguration _defaultHostConfiguration;

        public BootstrapSubcribers(IBus bus)
        {
            _defaultHostConfiguration = RabbitMqHostsConfiguration.Instance.RabbitHosts[RabbitMqHostsConfiguration.Instance.DefaultHost];
            _bus = bus;
        }

        public void LoadSubscribers(Assembly assembly, string subscriptionId, StructureMap.IContainer container)
        {
            var autosubscriber = new AutoSubscriber(_bus, subscriptionId)
            {
                ConfigureSubscriptionConfiguration = configuration => configuration.WithPrefetchCount(_defaultHostConfiguration.PreFetchCount),
                AutoSubscriberMessageDispatcher = new StructureMapMessageDispatcher(container)
            };

            autosubscriber.Subscribe(assembly);
            autosubscriber.SubscribeAsync(assembly);
        }
    }
}
