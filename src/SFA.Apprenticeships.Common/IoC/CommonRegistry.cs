namespace SFA.Apprenticeships.Common.IoC
{
    using SFA.Apprenticeships.Common.Caching;
    using SFA.Apprenticeships.Common.Configuration;
    using SFA.Apprenticeships.Common.Configuration.Azure;
    using SFA.Apprenticeships.Common.Configuration.LegacyServices;
    using SFA.Apprenticeships.Common.Configuration.Messaging;
    using StructureMap.Configuration.DSL;

    public class CommonRegistry : Registry
    {
        public CommonRegistry()
        {
            For<ICacheClient>()
                .Singleton()
                .Use<MemoryCacheClient>();

            For<IConfigurationManager>()
                .Singleton()
                .Use<ConfigurationManager>()
                .Ctor<string>("configFileAppSettingKey")
                .Is(ConfigurationManager.ConfigurationFileAppSetting);

            For<IRabbitMQConfiguration>()
                .Singleton().Use(RabbitMQConfigurationSection.Instance);

            For<ILegacyServicesConfiguration>()
                .Singleton()
                .Use(LegacyServicesConfigurationSection.Instance);

            For<IAzureCloudConfig>()
                .Singleton()
                .Use<AzureCloudConfig>();
        }
    }
}
