namespace SFA.Apprenticeships.Infrastructure.ApplicationEtl.IoC
{
    using System;
    using Consumers;
    using StructureMap.Configuration.DSL;

    public class ApplicationEtlRegistry : Registry
    {
        public ApplicationEtlRegistry()
        {
            For<ApplicationEtlControlQueueConsumer>().Use<ApplicationEtlControlQueueConsumer>();
            For<ApplicationStatusSummaryConsumerAsync>().Use<ApplicationStatusSummaryConsumerAsync>();
        }
    }
}
