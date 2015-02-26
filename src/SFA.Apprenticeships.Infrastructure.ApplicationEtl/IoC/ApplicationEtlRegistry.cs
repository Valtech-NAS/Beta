namespace SFA.Apprenticeships.Infrastructure.ApplicationEtl.IoC
{
    using Application.ApplicationUpdate;
    using Application.ApplicationUpdate.Strategies;
    using Consumers;
    using StructureMap.Configuration.DSL;

    public class ApplicationEtlRegistry : Registry
    {
        public ApplicationEtlRegistry()
        {
            For<ApplicationEtlControlQueueConsumer>().Use<ApplicationEtlControlQueueConsumer>();
            For<ApplicationStatusSummaryConsumerAsync>().Use<ApplicationStatusSummaryConsumerAsync>();
            For<IApplicationStatusProcessor>().Use<ApplicationStatusProcessor>();
            For<IApplicationStatusUpdateStrategy>().Use<ApplicationStatusUpdateStrategy>();
            For<IApplicationStatusChangedStrategy>().Use<ApplicationStatusChangedStrategy>();
        }
    }
}
