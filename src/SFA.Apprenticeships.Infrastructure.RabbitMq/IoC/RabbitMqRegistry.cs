namespace SFA.Apprenticeships.Infrastructure.RabbitMq.IoC
{
    using EasyNetQ;
    using SFA.Apprenticeships.Application.Interfaces.Messaging;
    using SFA.Apprenticeships.Infrastructure.RabbitMq.Configuration;
    using SFA.Apprenticeships.Infrastructure.RabbitMq.Interfaces;
    using SFA.Apprenticeships.Infrastructure.RabbitMq.RabbitMQ;
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
