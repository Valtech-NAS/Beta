namespace SFA.Apprenticeships.Infrastructure.RabbitMq.RabbitMQ
{
    using System.Reflection;
    using EasyNetQ;
    using EasyNetQ.AutoSubscribe;
    using StructureMap;
    using Interfaces;

    internal class BootstrapSubcribers : IBootstrapSubcribers
    {
        private readonly IBus _bus;

        public BootstrapSubcribers(IBus bus)
        {
            _bus = bus;
        }

        public void LoadSubscribers(Assembly assembly, string subscriptionId)
        {
            var autosubscriber = new AutoSubscriber(_bus, subscriptionId)
            {
#pragma warning disable 0618
                // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
                AutoSubscriberMessageDispatcher = new StructureMapMessageDispatcher(ObjectFactory.Container),
#pragma warning restore 0618
                GenerateSubscriptionId = c => string.Format("{0}_{1}", c.ConcreteType.Name, subscriptionId)
            };

            autosubscriber.Subscribe(assembly);
            autosubscriber.SubscribeAsync(assembly);
        }
    }
}
