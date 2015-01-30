namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.IoC
{
    using Application.ApplicationUpdate;
    using Application.Interfaces.Communications;
    using Microsoft.WindowsAzure;
    using Consumers;
    using StructureMap.Configuration.DSL;

    public class AsyncProcessorRegistry : Registry
    {
        public AsyncProcessorRegistry()
        {
            var emailDispatcher = CloudConfigurationManager.GetSetting("EmailDispatcher");
            var smsDispatcher = CloudConfigurationManager.GetSetting("SmsDispatcher");

            For<EmailRequestConsumerAsync>().Use<EmailRequestConsumerAsync>().Ctor<IEmailDispatcher>().Named(emailDispatcher);
            For<SmsRequestConsumerAsync>().Use<SmsRequestConsumerAsync>().Ctor<ISmsDispatcher>().Named(smsDispatcher);
            For<SubmitApprenticeshipApplicationRequestConsumerAsync>().Use<SubmitApprenticeshipApplicationRequestConsumerAsync>();
            For<SubmitTraineeshipApplicationRequestConsumerAsync>().Use<SubmitTraineeshipApplicationRequestConsumerAsync>();
            For<CreateCandidateRequestConsumerAsync>().Use<CreateCandidateRequestConsumerAsync>();
            For<CommunicationRequestConsumerAsync>().Use<CommunicationRequestConsumerAsync>();
            For<IApplicationStatusProcessor>().Use<ApplicationStatusProcessor>();
        }
    }
}
