namespace SFA.Apprenticeships.Common.Messaging.IoC
{
    using EasyNetQ;
    using SFA.Apprenticeships.Common.Messaging.Interfaces;
    using SFA.Apprenticeships.Common.Messaging.RabbitMQ;
    using StructureMap.Configuration.DSL;

    public class CommonMessagingRegistry : Registry
    {
        public CommonMessagingRegistry()
        {
            For<IBus>().Singleton().Use(Transport.CreateBus());
            For<IBootstrapSubcribers>().Singleton().Use<BootstrapSubcribers>();
        }
    }
}
