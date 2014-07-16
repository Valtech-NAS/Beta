namespace SFA.Apprenticeships.Infrastructure.Common.IoC
{
    using Configuration;
    using StructureMap.Configuration.DSL;

    //todo: remove this? should be setting up configuration IoC in particular infra projects
    public class CommonRegistry : Registry
    {
        public CommonRegistry()
        {
            For<IConfigurationManager>()
                .Singleton()
                .Use<ConfigurationManager>()
                .Ctor<string>("configFileAppSettingKey")
                .Is(ConfigurationManager.ConfigurationFileAppSetting);
        }
    }
}
