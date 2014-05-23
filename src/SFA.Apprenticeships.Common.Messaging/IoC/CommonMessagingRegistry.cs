namespace SFA.Apprenticeships.Common.Messaging.IoC
{
    using EasyNetQ;
    using SFA.Apprenticeships.Common.Configuration.Messaging;
    using SFA.Apprenticeships.Common.Messaging.Interfaces;
    using SFA.Apprenticeships.Common.Messaging.RabbitMQ;
    using StructureMap.Configuration.DSL;

    public class CommonMessagingRegistry : Registry
    {
        public CommonMessagingRegistry()
        {
            var rabbitBuses = RabbitMqHostsConfiguration.Instance;
            For<IBus>().Singleton().Use(BusFactory.CreateBus(rabbitBuses.DefaultHost));
            For<IBootstrapSubcribers>().Singleton().Use<BootstrapSubcribers>();
        }
    }
}
