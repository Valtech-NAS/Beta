namespace SFA.Apprenticeships.Infrastructure.Common.IoC
{
    using ActiveDirectory;
    using Configuration;
    using StructureMap.Configuration.DSL;

    public class CommonRegistry : Registry
    {
        public CommonRegistry()
        {
            For<IConfigurationManager>()
                .Singleton()
                .Use<ConfigurationManager>()
                .Ctor<string>("configFileAppSettingKey")
                .Is(ConfigurationManager.ConfigurationFileAppSetting);

            For<IActiveDirectoryConfiguration>().Singleton().Use(ActiveDirectoryConfiguration.Instance);
        }
    }
}
