namespace SFA.Apprenticeships.Infrastructure.Communication.IoC
{
    using Application.Interfaces.Messaging;
    using Configuration;
    using StructureMap.Configuration.DSL;

    public class CommunicationRegistry : Registry
    {
        public CommunicationRegistry()
        {
            For<IEmailDispatcher>().Use<SendGridEmailDispatcher>().Name = "SendGridEmailDispatcher";
            For<IEmailDispatcher>().Use<VoidEmailDispatcher>().Name = "VoidEmailDispatcher";
            For<ILoggerEmailDispatcher>().Use<SendLoggerEmailDispatcher>();
            For<ISmsDispatcher>().Use<TwilioSmsDispatcher>();
            For<SendGridConfiguration>().Singleton().Use(SendGridConfiguration.Instance);
        }
    }
}