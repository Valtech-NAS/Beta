namespace SFA.Apprenticeships.Infrastructure.RabbitMq.IoC
{
    using EasyNetQ;
    using SFA.Apprenticeships.Infrastructure.RabbitMq.Configuration;
    using SFA.Apprenticeships.Infrastructure.RabbitMq.Interfaces;
    using SFA.Apprenticeships.Infrastructure.RabbitMq.RabbitMQ;
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
