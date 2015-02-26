namespace SFA.Apprenticeships.Web.Employer.Ioc
{
    using System.Web;
    using Application.Interfaces;
    using Application.Interfaces.Communications;
    using Application.Services.CommunicationService;
    using Application.Services.ConfigReferenceDataService;
    using Application.Services.LocationSearchService;
    using Common.AppSettings;
    using Domain.Entities;
    using Infrastructure.Communication.Email;
    using Mappers;
    using Mappers.Interfaces;
    using Mediators.EmployerEnquiry;
    using Mediators.Interfaces;
    using Mediators.Location;
    using Providers;
    using Providers.Interfaces;
    using StructureMap.Configuration.DSL;
    using ViewModels;

    public class EmployerWebRegistry : Registry
    {
        public EmployerWebRegistry()
        {
            For<HttpContextBase>().Use(ctx => new HttpContextWrapper(HttpContext.Current));

            RegisterServices();

            RegisterMappers();

            RegisterProviders();

            RegisterMediators();
        }

        private void RegisterServices()
        {
            For<IEmailDispatcher>().Use<SendGridEmailDispatcher>().Name = "SendGridEmailDispatcher";
            For<IEmailDispatcher>().Use<VoidEmailDispatcher>().Name = "VoidEmailDispatcher";
            For<ILocationSearchService>().Use<LocationSearchService>();
            For<ICommunciationService>().Use<CommunciationService>().Ctor<IEmailDispatcher>().Named(BaseAppSettingValues.EmailDispatcher);
            For<IReferenceDataService>().Use<ConfigReferenceDataService>();
        }

        private void RegisterProviders()
        {
            For<IEmployerEnquiryProvider>().Use<EmployerEnquiryProvider>();
            For<ILocationProvider>().Use<LocationProvider>();
        }

        private void RegisterMediators()
        {
            For<IEmployerEnquiryMediator>().Use<EmployerEnquiryMediator>();
            For<ILocationMediator>().Use<LocationMediator>();
        }

        private void RegisterMappers()
        {
            For<IDomainToViewModelMapper<Address, AddressViewModel>>().Use<AddressMapper>();
            For<IViewModelToDomainMapper<AddressViewModel, Address>>().Use<AddressMapper>();

            For<IDomainToViewModelMapper<EmployerEnquiry, EmployerEnquiryViewModel>>().Use<EmployerEnquiryMapper>();
            For<IViewModelToDomainMapper<EmployerEnquiryViewModel, EmployerEnquiry>>().Use<EmployerEnquiryMapper>();

            For<IDomainToViewModelMapper<Location, LocationViewModel>>().Use<LocationMapper>();
            For<IViewModelToDomainMapper<LocationViewModel, Location>>().Use<LocationMapper>();

            For<IDomainToViewModelMapper<ReferenceData, ReferenceDataViewModel>>().Use<ReferenceDataMapper>();
            For<IViewModelToDomainMapper<ReferenceDataViewModel, ReferenceData>>().Use<ReferenceDataMapper>();
        }
    }
}
