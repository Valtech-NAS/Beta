namespace SFA.Apprenticeships.Common.Messaging.RabbitMQ
{
    using System;
    using System.Reflection;
    using EasyNetQ;
    using EasyNetQ.AutoSubscribe;
    using StructureMap;

    public class BootstrapSubcribers
    {
        private readonly IBus _bus;

        public BootstrapSubcribers(IBus bus)
        {
            if (bus == null)
            {
                throw new ArgumentNullException("bus");
            }

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
