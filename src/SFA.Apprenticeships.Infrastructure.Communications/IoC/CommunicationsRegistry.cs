namespace SFA.Apprenticeships.Infrastructure.Communications.IoC
{
    using Application.Communications;
    using Consumers;
    using StructureMap.Configuration.DSL;

    public class CommunicationsRegistry : Registry
    {
        public CommunicationsRegistry()
        {
            For<ICommunicationProcessor>().Use<CommunicationProcessor>();
            For<CommunicationsControlQueueConsumer>().Use<CommunicationsControlQueueConsumer>();
        }
    }
}
