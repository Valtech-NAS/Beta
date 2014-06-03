namespace SFA.Apprenticeships.Infrastructure.RabbitMq.RabbitMQ
{
    using System;
    using System.Reflection;
    using EasyNetQ;
    using EasyNetQ.AutoSubscribe;
    using CuttingEdge.Conditions;
    using StructureMap;
    using Interfaces;

    internal class BootstrapSubcribers : IBootstrapSubcribers
    {
        private readonly IBus _bus;

        public BootstrapSubcribers(IBus bus)
        {
            Condition.Requires(bus, "bus").IsNotNull();

            _bus = bus;
        }

        public void LoadSubscribers(Assembly assembly, string subscriptionId)
        {
            var autosubscriber = new AutoSubscriber(_bus, subscriptionId)
            {
                AutoSubscriberMessageDispatcher = new StructureMapMessageDispatcher(ObjectFactory.Container),
                GenerateSubscriptionId = c => c.ConcreteType.Name
            };

            autosubscriber.Subscribe(assembly);
            autosubscriber.SubscribeAsync(assembly);
        }
    }
}
