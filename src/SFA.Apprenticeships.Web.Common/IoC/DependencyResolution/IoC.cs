using SFA.Apprenticeships.Services.Common.ActiveDirectory;
using SFA.Apprenticeships.Services.Common.Configuration;
using SFA.Apprenticeships.Web.Common.Providers;
using StructureMap;
using StructureMap.Graph;

namespace SFA.Apprenticeships.Web.Common.IoC.DependencyResolution
{
    /// <summary>
    /// IoC Container
    /// </summary>
    public static class IoC
    {
        private const string PrivateConfigurationFile = @"";

        public static IContainer Initialize()
        {
            var container = ObjectFactory.Container;
            ObjectFactory.Initialize(
                x => x.Scan(
                    scan =>
                    {
                        scan.TheCallingAssembly();
                        scan.WithDefaultConventions();
                    }));

            return container
                .LoadCommonConfiguration()
                .LoadWebConfiguration();
        }

        private static IContainer LoadWebConfiguration(this IContainer container)
        {
            container.Configure(
                x =>
                {
                    x.For<IActiveDirectoryConfiguration>().Singleton().Use(ActiveDirectoryConfigurationSection.ConfigurationSectionDetails);
                    // more entries here
                });

            return container;
        }

        private static IContainer LoadCommonConfiguration(this IContainer container)
        {
            container.Configure(
                x =>
                {
                    x.For<IReferenceDataProvider>().Use<ConfigReferenceDataProvider>();
                    x.For<IConfigurationManager>()
                        .Singleton()
                        .Use<ConfigurationManager>()
                        .Ctor<string>("configFile")
                        .Is(System.Configuration.ConfigurationManager.AppSettings[ConfigurationManager.ConfigurationFileAppSetting]);
                });

            return container;
        }
    }
}