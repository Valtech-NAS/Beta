namespace SFA.Apprenticeships.Web.Common.IoC
{
    using SFA.Apprenticeships.Common.Caching;
    using SFA.Apprenticeships.Services.Common.ActiveDirectory;
    using SFA.Apprenticeships.Web.Common.Providers;
    using StructureMap.Configuration.DSL;

    public class WebCommonRegistry : Registry
    {
        public WebCommonRegistry()
        {
            For<IActiveDirectoryConfiguration>().Singleton().Use(ActiveDirectoryConfigurationSection.Instance);

            // Use cache decorated provider.
            For<IReferenceDataProvider>().Use<LegacyReferenceDataProvider>()
                .DecorateWith((ctx, provider) => new CacheLegacyReferenceDataProvider(ctx.GetInstance<ICacheClient>(), provider));
        }
    }
}
