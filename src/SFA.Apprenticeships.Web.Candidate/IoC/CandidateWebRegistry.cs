namespace SFA.Apprenticeships.Web.Candidate.IoC
{
    using Application.Address;
    using Application.ApplicationUpdate;
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
    using Application.UserAccount;
    using Application.UserAccount.Strategies;
    using Application.Vacancy;
    using Domain.Interfaces.Mapping;
    using Infrastructure.LegacyWebServices.GetCandidateApplicationStatuses;
    using Mappers;
    using Providers;
    using StructureMap.Configuration.DSL;
    using ISendPasswordResetCodeStrategy = Application.UserAccount.Strategies.ISendPasswordResetCodeStrategy;

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
            For<IRegisterUserStrategy>().Use<RegisterUserStrategy>();
            For<IActivateUserStrategy>().Use<ActivateUserStrategy>();

            For<IResetForgottenPasswordStrategy>()
                .Use<ResetForgottenPasswordStrategy>()
                .Name = "ResetForgottenPasswordStrategy";

            For<IResetForgottenPasswordStrategy>()
                .Use<LegacyResetForgottenPasswordStrategy>()
                .Ctor<IResetForgottenPasswordStrategy>()
                .Named("ResetForgottenPasswordStrategy")
                .Name = "LegacyResetForgottenPasswordStrategy";

            For<ISendPasswordResetCodeStrategy>().Use<SendPasswordResetCodeStrategy>();
            For<Application.Communication.Strategies.ISendPasswordResetCodeStrategy>()
                .Use<QueueEmailOnlyPasswordResetCodeStrategy>();
            For<ISubmitApplicationStrategy>().Use<LegacySubmitApplicationStrategy>();
            For<ISendActivationCodeStrategy>().Use<QueueEmailOnlyActivationCodeStrategy>();
            For<ISendApplicationSubmittedStrategy>().Use<LegacyQueueApplicationSubmittedStrategy>();
            For<ISendPasswordChangedStrategy>().Use<QueueEmailOnlyPasswordChangedStrategy>();
            For<Application.Communication.Strategies.ISendAccountUnlockCodeStrategy>().Use<QueueEmailOnlyAccountUnlockCodeStrategy>();
            For<IResendActivationCodeStrategy>().Use<ResendActivationCodeStrategy>();
            For<Application.UserAccount.Strategies.ISendAccountUnlockCodeStrategy>().Use<SendAccountUnlockCodeStrategy>();

            For<IUnlockAccountStrategy>()
                .Use<UnlockAccountStrategy>()
                .Name = "UnlockAccountStrategy";

            For<IUnlockAccountStrategy>()
               .Use<LegacyUnlockAccountStrategy>()
               .Ctor<IUnlockAccountStrategy>()
               .Named("UnlockAccountStrategy")
               .Name = "LegacyUnlockAccountStrategy";

            For<ILockAccountStrategy>().Use<LockAccountStrategy>();
            For<ILockUserStrategy>().Use<LockUserStrategy>();
            For<ICreateApplicationStrategy>().Use<CreateApplicationStrategy>();
            For<ISaveApplicationStrategy>().Use<SaveApplicationStrategy>();
            For<IAuthenticateCandidateStrategy>().Use<AuthenticateCandidateStrategy>();
            For<IUserAccountService>().Use<UserAccountService>();
            For<IAddressSearchService>().Use<AddressSearchService>();
            For<IAuthenticationService>().Use<AuthenticationService>();
            For<ICommunicationService>().Use<CommunicationService>();
            For<ICodeGenerator>().Use<CodeGenerator>();
            For<IGetCandidateApplicationsStrategy>().Use<LegacyGetCandidateApplicationsStrategy>();
            For<ILegacyApplicationStatusesProvider>().Use<LegacyCandidateApplicationStatusesProvider>();
            For<IApplicationStatusUpdater>().Use<ApplicationStatusUpdater>();

            // providers (web)
            For<IMapper>().Singleton().Use<CandidateWebMappers>().Name = "CandidateWebMappers";
            For<ISearchProvider>().Use<SearchProvider>().Ctor<IMapper>().Named("CandidateWebMappers");
            For<IVacancyDetailProvider>().Use<VacancyDetailProvider>().Ctor<IMapper>().Named("CandidateWebMappers");
            For<IApplicationProvider>().Use<ApplicationProvider>();
            For<ICandidateServiceProvider>()
                .Use<CandidateServiceProvider>()
                .Ctor<IMapper>()
                .Named("CandidateWebMappers");
        }
    }
}