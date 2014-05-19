namespace SFA.Apprenticeships.Common.IoC
{
    using SFA.Apprenticeships.Common.Caching;
    using SFA.Apprenticeships.Common.Configuration;
    using StructureMap.Configuration.DSL;

    public class CommonRegistry : Registry
    {
        public CommonRegistry()
        {
            For<ICacheClient>().Singleton().Use<MemoryCacheClient>();
            For<IConfigurationManager>().Singleton().Use<ConfigurationManager>()
                .Ctor<string>("configFileAppSettingKey").Is(ConfigurationManager.ConfigurationFileAppSetting);
        }
    }
}
