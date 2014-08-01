namespace SFA.Apprenticeships.Infrastructure.RabbitMq.RabbitMQ
{
    using System;
    using System.Reflection;
    using Application.Interfaces.Messaging;
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
                AutoSubscriberMessageDispatcher = new StructureMapMessageDispatcher(ObjectFactory.Container),
                GenerateSubscriptionId = c => c.ConcreteType.Name
            };

            autosubscriber.Subscribe(assembly);
            autosubscriber.SubscribeAsync(assembly);
        }
    }
}
