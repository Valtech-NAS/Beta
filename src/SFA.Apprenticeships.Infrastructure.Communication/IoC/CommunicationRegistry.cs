namespace SFA.Apprenticeships.Infrastructure.Communication.IoC
{
    using System;
    using Application.Interfaces.Messaging;
    using StructureMap.Configuration.DSL;

    public class CommunicationRegistry : Registry
    {
        public CommunicationRegistry()
        {
            For<IEmailDispatcher>().Use<SendGridEmailDispatcher>();
            For<ISmsDispatcher>().Use<TwilioSmsDispatcher>();
        }
    }
}
