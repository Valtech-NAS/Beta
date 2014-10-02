using SFA.Apprenticeships.Infrastructure.Communication.Configuration;

namespace SFA.Apprenticeships.Infrastructure.Communication.IoC
{
    using System;
    using Application.Interfaces.Messaging;
    using StructureMap.Configuration.DSL;

    public class CommunicationRegistry : Registry
    {
        public CommunicationRegistry()
        {
            For<IEmailDispatcher>().Use<SendGridEmailDispatcher>().Name = "SendGridEmailDispatcher";
            For<IEmailDispatcher>().Use<VoidEmailDispatcher>().Name = "VoidEmailDispatcher";           

            For<ISmsDispatcher>().Use<TwilioSmsDispatcher>();
            For<SendGridConfiguration>().Singleton().Use(SendGridConfiguration.Instance);
        }
    }
}
