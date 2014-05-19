﻿namespace SFA.Apprenticeships.Common.Messaging
{
    using System.Reflection;
    using EasyNetQ;
    using EasyNetQ.AutoSubscribe;

    public class Bootstrapper
    {
        private IBus _bus;

        public Bootstrapper(IBus bus)
        {
            _bus = bus;
        }


        public void LoadConsumers(Assembly assembly, string subscriptionId)
        {
            var autosubscriber = new AutoSubscriber(_bus, subscriptionId)
            {
                //AutoSubscriberMessageDispatcher = new StructureMapMessageDispatcher(),
                GenerateSubscriptionId = c => c.ConcreteType.Name
            };

            autosubscriber.Subscribe(assembly);
            autosubscriber.SubscribeAsync(assembly);
        }
    }
}
