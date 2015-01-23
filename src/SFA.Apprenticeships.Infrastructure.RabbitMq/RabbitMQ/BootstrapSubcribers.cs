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

        public void LoadSubscribers(Assembly assembly, string subscriptionId)
        {
            var autosubscriber = new AutoSubscriber(_bus, subscriptionId)
            {
                ConfigureSubscriptionConfiguration = configuration => configuration.WithPrefetchCount(_defaultHostConfiguration.PreFetchCount),
#pragma warning disable 0618
                // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
                AutoSubscriberMessageDispatcher = new StructureMapMessageDispatcher(ObjectFactory.Container)
#pragma warning restore 0618
            };

            autosubscriber.Subscribe(assembly);
            autosubscriber.SubscribeAsync(assembly);
        }
    }
}
