namespace SFA.Apprenticeships.Web.Common.IoC
{
    using Providers;
    using StructureMap.Configuration.DSL;
    using Application.Interfaces.ReferenceData;
    using Infrastructure.LegacyWebServices.ReferenceData;

    public class WebCommonRegistry : Registry
    {
        public WebCommonRegistry()
        {
            For<IWebReferenceDataProvider>().Use<WebReferenceDataProvider>();
            For<IReferenceDataProvider>().Use<LegacyReferenceDataProvider>();
        }
    }
}
