namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.IoC
{
    using Application.Candidate.Strategies;
    using Consumers;
    using LegacyWebServices.CreateApplication;
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
