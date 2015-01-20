namespace SFA.Apprenticeships.Infrastructure.Communications.IoC
{
    using Consumers;
    using StructureMap.Configuration.DSL;

    public class CommunicationsRegistry : Registry
    {
        public CommunicationsRegistry()
        {
            For<CommunicationsControlQueueConsumer>().Use<CommunicationsControlQueueConsumer>();
        }
    }
}
