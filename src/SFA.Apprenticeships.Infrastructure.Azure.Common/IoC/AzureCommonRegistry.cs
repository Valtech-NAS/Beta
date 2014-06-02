namespace SFA.Apprenticeships.Infrastructure.Azure.Common.IoC
{
    using Configuration;
    using StructureMap.Configuration.DSL;

    public class AzureCommonRegistry : Registry
    {
        public AzureCommonRegistry()
        {
            For<IAzureCloudConfig>().Singleton().Use<AzureCloudConfig>();
            For<IAzureCloudClient>().Use<AzureCloudClient>();
        }
    }
}
