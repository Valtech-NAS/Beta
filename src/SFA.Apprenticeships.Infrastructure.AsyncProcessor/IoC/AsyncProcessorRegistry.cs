namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.IoC
{
    using Microsoft.WindowsAzure;
    using Application.Interfaces.Messaging;
    using Consumers;
    using StructureMap.Configuration.DSL;

    public class AsyncProcessorRegistry : Registry
    {
        public AsyncProcessorRegistry()
        {
            var emailDispatcher = CloudConfigurationManager.GetSetting("EmailDispatcher");

            For<EmailRequestConsumerAsync>().Use<EmailRequestConsumerAsync>().Ctor<IEmailDispatcher>().Named(emailDispatcher);
            For<SubmitApprenticeshipApplicationRequestConsumerAsync>().Use<SubmitApprenticeshipApplicationRequestConsumerAsync>();
            For<SubmitTraineeshipApplicationRequestConsumerAsync>().Use<SubmitTraineeshipApplicationRequestConsumerAsync>();
            For<CreateCandidateRequestConsumerAsync>().Use<CreateCandidateRequestConsumerAsync>();
            For<CommunicationRequestConsumerAsync>().Use<CommunicationRequestConsumerAsync>();
        }
    }
}
