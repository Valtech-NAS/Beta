namespace SFA.Apprenticeships.Web.Common.IoC
{
    using SFA.Apprenticeships.Web.Common.Providers;
    using StructureMap.Configuration.DSL;

    public class WebCommonRegistry : Registry
    {
        public WebCommonRegistry()
        {
            For<IReferenceDataProvider>().Use<LegacyReferenceDataProvider>();
        }
    }
}
