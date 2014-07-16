namespace SFA.Apprenticeships.Web.Candidate.IoC
{
    using Application.Address;
    using Application.Authentication;
    using Application.Candidate;
    using Application.Candidate.Strategies;
    using Application.Communication;
    using Application.Communication.Strategies;
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Locations;
    using Application.Interfaces.Messaging;
    using Application.Interfaces.Users;
    using Application.Interfaces.Vacancies;
    using Application.Location;
    using Application.Registration;
    using Application.Vacancy;
    using Domain.Interfaces.Mapping;
    using Infrastructure.LegacyWebServices.CreateCandidate;
    using Infrastructure.UserDirectory;
    using Mappers;
    using Providers;
    using StructureMap.Configuration.DSL;

    public class CandidateWebRegistry : Registry
    {
        public CandidateWebRegistry()
        {
            // services (app)
            For<ILocationSearchService>().Use<LocationSearchService>();
            For<IVacancySearchService>().Use<VacancySearchService>();
            For<IVacancyDataService>().Use<VacancyDataService>();
            For<ICandidateService>().Use<CandidateService>();
            For<IActivateCandidateStrategy>().Use<LegacyActivateCandidateStrategy>();
            For<IRegisterCandidateStrategy>().Use<RegisterCandidateStrategy>();
            For<ISubmitApplicationStrategy>().Use<LegacySubmitApplicationStrategy>();
            For<ISendActivationCodeStrategy>().Use<QueueEmailOnlyActivationCodeStrategy>();
            For<ILegacyCandidateProvider>().Use<LegacyCandidateProvider>();
            For<IRegistrationService>().Use<RegistrationService>();
            For<IAddressSearchService>().Use<AddressSearchService>();
            For<IAuthenticationService>().Use<AuthenticationService>();
            For<ICommunicationService>().Use<CommunicationService>();
            For<ICodeGenerator>().Use<CodeGenerator>();

            // providers (web)
            For<IMapper>().Singleton().Use<CandidateWebMappers>().Name = "CandidateWebMappers";
            For<ISearchProvider>().Use<SearchProvider>().Ctor<IMapper>().Named("CandidateWebMappers");
            For<IVacancyDetailProvider>().Use<VacancyDetailProvider>().Ctor<IMapper>().Named("CandidateWebMappers");
            For<IApplicationProvider>().Use<ApplicationProvider>();
            For<ICandidateServiceProvider>().Use<CandidateServiceProvider>().Ctor<IMapper>().Named("CandidateWebMappers");
        }
    }
}