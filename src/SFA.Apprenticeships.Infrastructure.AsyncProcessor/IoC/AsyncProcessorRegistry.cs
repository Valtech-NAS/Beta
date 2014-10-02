namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.IoC
{
    using Application.Interfaces.Messaging;
    using Consumers;
    using Properties;
    using StructureMap.Configuration.DSL;

    public class AsyncProcessorRegistry : Registry
    {
        public AsyncProcessorRegistry()
        {
            var emailDispatcher = Settings.Default.EmailDispatcher;

            For<EmailRequestConsumerAsync>().Use<EmailRequestConsumerAsync>().
                Ctor<IEmailDispatcher>().Named(emailDispatcher);
            For<SubmitApplicationRequestConsumerAsync>().Use<SubmitApplicationRequestConsumerAsync>();
        }
    }
}
