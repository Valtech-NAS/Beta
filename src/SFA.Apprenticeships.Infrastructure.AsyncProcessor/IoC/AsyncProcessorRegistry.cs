namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.IoC
{
    using Consumers;
    using StructureMap.Configuration.DSL;

    public class AsyncProcessorRegistry : Registry
    {
        public AsyncProcessorRegistry()
        {
            For<EmailRequestConsumerAsync>().Use<EmailRequestConsumerAsync>();
            For<SubmitApplicationRequestConsumerAsync>().Use<SubmitApplicationRequestConsumerAsync>();
        }
    }
}
