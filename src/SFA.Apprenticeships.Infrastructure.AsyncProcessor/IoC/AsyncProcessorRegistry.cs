namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.IoC
{
    using AsyncProcessor.Consumers;
    using StructureMap.Configuration.DSL;

    public class AsyncProcessorRegistry : Registry
    {
        public AsyncProcessorRegistry()
        {
            For<EmailConsumerAsync>().Use<EmailConsumerAsync>();
        }
    }
}
