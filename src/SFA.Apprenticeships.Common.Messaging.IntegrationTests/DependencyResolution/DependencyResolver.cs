namespace SFA.Apprenticeships.Common.Messaging.IntegrationTests.DependencyResolution
{
    using SFA.Apprenticeships.Common.Messaging.IntegrationTests.Consumers;
    using StructureMap;

    public static class DependencyResolver
    {
        public static IContainer LoadConfiguration(this IContainer container)
        {
            container.Configure(
                x =>
                {
                    x.For<TestConsumerSync>().Use<TestConsumerSync>();
                    x.For<TestConsumerAsync>().Use<TestConsumerAsync>();
                });

            return container;
        }
    }
}
