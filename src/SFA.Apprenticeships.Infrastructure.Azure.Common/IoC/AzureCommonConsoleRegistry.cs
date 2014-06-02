namespace SFA.Apprenticeships.Infrastructure.Azure.Common.IoC
{
    using Configuration;
    using StructureMap.Configuration.DSL;

    public class AzureCommonConsoleRegistry : Registry
    {
        public AzureCommonConsoleRegistry()
        {
            For<IAzureCloudConfig>().Singleton().Use<AzureConsoleConfig>();
            For<IAzureCloudClient>().Use<AzureCloudClient>();
        }
    }
}
