namespace SFA.Apprenticeships.Web.Common.IoC
{
    using SFA.Apprenticeships.Services.Common.ActiveDirectory;
    using SFA.Apprenticeships.Web.Common.Providers;
    using StructureMap.Configuration.DSL;

    public class WebCommonRegistry : Registry
    {
        public WebCommonRegistry()
        {
            For<IActiveDirectoryConfiguration>().Singleton().Use(ActiveDirectoryConfigurationSection.Instance);
            For<IReferenceDataProvider>().Use<LegacyReferenceDataProvider>();
        }
    }
}
