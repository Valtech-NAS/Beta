namespace SFA.Apprenticeships.Infrastructure.RabbitMq.IoC
{
    using Domain.Interfaces.Messaging;
    using EasyNetQ;
    using Configuration;
    using Interfaces;
    using RabbitMQ;
    using StructureMap;
    using StructureMap.Configuration.DSL;

    public class RabbitMqRegistry : Registry
    {
        public RabbitMqRegistry()
        {
            var rabbitBuses = RabbitMqHostsConfiguration.Instance;
            For<IBus>().Singleton().Use(BusFactory.CreateBus(rabbitBuses.DefaultHost));
            For<IBootstrapSubcribers>().Singleton().Use<BootstrapSubcribers>();
            For<IMessageBus>().Singleton().Use<RabbitMessageBus>();
        }

        /// <summary>
        /// Added to prevent the need for a EasyNetQ references in dependant apps
        /// and to allow disposal of the EasyNetQ bus.
        /// </summary>
        public static void DisposeResources()
        {
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            ObjectFactory.GetInstance<IBus>().Advanced.Dispose();
#pragma warning restore 0618
        }
    }
}
