namespace SFA.Apprenticeships.Web.Employer.Ioc
{
    using System.Web;
    using Application.Interfaces;
    using Application.Interfaces.Communications;
    using Application.Services.CommunicationService;
    using Application.Services.ConfigReferenceDataService;
    using Application.Services.LocationSearchService;
    using Mediators;
    using Mediators.Interfaces;
    using Providers;
    using Providers.Interfaces;
    using StructureMap.Configuration.DSL;

    public class EmployerWebRegistry : Registry
    {
        public EmployerWebRegistry()
        {
            For<HttpContextBase>().Use(ctx => new HttpContextWrapper(HttpContext.Current));

            RegisterServices();

            RegisterProviders();

            RegisterMediators();
        }
        
        private void RegisterServices()
        {
            For<ILocationSearchService>().Use<LocationSearchService>();
            For<ICommunciationService>().Use<CommunciationService>();
            For<IReferenceDataService>().Use<ConfigReferenceDataService>();
        }

        private void RegisterProviders()
        {
            For<IEmployerEnquiryProvider>().Use<EmployerEnquiryProvider>();
        }

        private void RegisterMediators()
        {
            For<IEmployerEnquiryMediator>().Use<EmployerEnquiryMediator>();
        }
    }
}
