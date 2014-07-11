using SFA.Apprenticeships.Application.Communication.Strategies;

namespace SFA.Apprenticeships.Web.Candidate.IoC
{
    using Application.Interfaces.Locations;
    using Application.Interfaces.Vacancies;
    using Application.Location;
    using Application.Vacancy;
    using Domain.Interfaces.Mapping;
    using Mappers;
    using Providers;
    using StructureMap.Configuration.DSL;
    using Application.Candidate;
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Users;
    using Application.Registration;
    using Application.Address;
    using Application.Authentication;
    using Application.Candidate.Strategies;
    using Application.Communication;
    using Application.Interfaces.Messaging;
    using Infrastructure.LegacyWebServices.CreateCandidate;

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
            For<IAddressSearchServiceProvider>().Use<AddressSearchServiceProvider>().Ctor<IMapper>().Named("CandidateWebMappers");
        }
    }
}
