namespace SFA.Apprenticeships.Infrastructure.Azure.Common.IoC
{
    using Configuration;
    using StructureMap.Configuration.DSL;

    public class AzureCommonConsoleRegistry : Registry
    {
        public AzureCommonConsoleRegistry()
        {
            For<IAzureCloudConfig>().Singleton().Use<AzureConsoleCloudConfig>();
            For<IAzureCloudClient>().Use<AzureCloudClient>();
        }
    }
}
