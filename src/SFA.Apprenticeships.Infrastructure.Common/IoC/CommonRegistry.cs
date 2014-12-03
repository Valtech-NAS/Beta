namespace SFA.Apprenticeships.Infrastructure.Common.IoC
{
    using SFA.Apprenticeships.Domain.Interfaces.Configuration;
    using SFA.Apprenticeships.Infrastructure.Common.Configuration;
    using StructureMap.Configuration.DSL;

    //TODO: remove this? should be setting up configuration IoC in particular infra projects
    public class CommonRegistry : Registry
    {
        public CommonRegistry()
        {
            For<IConfigurationManager>().Singleton().Use<ConfigurationManager>();
            For<IFeatureToggle>().Use<FeatureToggle>();
        }
    }
}