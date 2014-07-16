namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.IoC
{
    using System;
    using Consumers;
    using StructureMap.Configuration.DSL;

    public class AsyncProcessorRegistry : Registry
    {
        public AsyncProcessorRegistry()
        {
            For<EmailConsumerAsync>().Use<EmailConsumerAsync>();
        }
    }
}
