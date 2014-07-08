namespace SFA.Apprenticeships.Infrastructure.RabbitMq.IoC
{
    using Domain.Interfaces.Messaging;
    using EasyNetQ;
    using Configuration;
    using Interfaces;
    using RabbitMQ;
    using StructureMap.Configuration.DSL;

    public class RabbitMqRegistry : Registry
    {
        public RabbitMqRegistry()
        {
            var rabbitBuses = RabbitMqHostsConfiguration.Instance;
            For<IBus>().Singleton().Use(BusFactory.CreateBus(rabbitBuses.DefaultHost));
            For<IBootstrapSubcribers>().Singleton().Use<BootstrapSubcribers>();
            For<IMessageBus>().Singleton().Use<MessageBus>();
        }
    }
}
