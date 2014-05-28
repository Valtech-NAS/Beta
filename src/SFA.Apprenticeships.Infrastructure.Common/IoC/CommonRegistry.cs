namespace SFA.Apprenticeships.Infrastructure.Common.IoC
{
    using StructureMap.Configuration.DSL;

    public class CommonRegistry : Registry
    {
        public CommonRegistry()
        {
            //For<ICacheClient>()
            //    .Singleton()
            //    .Use<MemoryCacheClient>();

            //For<IConfigurationManager>()
            //    .Singleton()
            //    .Use<ConfigurationManager>()
            //    .Ctor<string>("configFileAppSettingKey")
            //    .Is(ConfigurationManager.ConfigurationFileAppSetting);

            //For<RabbitMqHostsConfiguration>()
            //    .Singleton().Use(RabbitMqHostsConfiguration.Instance);

            //For<ILegacyServicesConfiguration>()
            //    .Singleton()
            //    .Use(LegacyServicesConfiguration.Instance);

            //For<IAzureCloudConfig>()
            //    .Singleton()
            //    .Use<AzureCloudConfig>();
        }
    }
}
