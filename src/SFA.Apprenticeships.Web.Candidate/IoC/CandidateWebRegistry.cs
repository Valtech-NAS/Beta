namespace SFA.Apprenticeships.Web.Candidate.IoC
{
    using System.Web;
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
    using Mappers;
    using Microsoft.WindowsAzure;
    using Providers;
    using StructureMap.Configuration.DSL;
    using ISendPasswordResetCodeStrategy = Application.UserAccount.Strategies.ISendPasswordResetCodeStrategy;

    public class CandidateWebRegistry : Registry
    {
        public CandidateWebRegistry()
        {
            var codeGenerator = CloudConfigurationManager.GetSetting("CodeGenerator");

            // services (app)
            For<ILocationSearchService>().Use<LocationSearchService>();
            For<IVacancySearchService>().Use<VacancySearchService>();

            For<ICandidateService>().Use<CandidateService>();
            For<IActivateCandidateStrategy>().Use<LegacyActivateCandidateStrategy>();
            //For<IActivateCandidateStrategy>().Use<QueuedLegacyActivateCandidateStrategy>();
            For<IRegisterCandidateStrategy>().Use<RegisterCandidateStrategy>()
                .Ctor<ICodeGenerator>().Named(codeGenerator);
            For<IRegisterUserStrategy>().Use<RegisterUserStrategy>();
            For<IActivateUserStrategy>().Use<ActivateUserStrategy>();

            For<ICodeGenerator>().Use<CodeGenerator>().Name = "RandomCodeGenerator";
            For<ICodeGenerator>().Use<DefaultCodeGenerator>().Name = "DefaultCodeGenerator";

            For<IResetForgottenPasswordStrategy>()
                .Use<ResetForgottenPasswordStrategy>()
                .Name = "ResetForgottenPasswordStrategy";

            For<IResetForgottenPasswordStrategy>()
                .Use<LegacyResetForgottenPasswordStrategy>()
                .Ctor<IResetForgottenPasswordStrategy>()
                .Named("ResetForgottenPasswordStrategy")
                .Name = "LegacyResetForgottenPasswordStrategy";

            For<ISendPasswordResetCodeStrategy>().Use<SendPasswordResetCodeStrategy>()
                .Ctor<ICodeGenerator>().Named(codeGenerator);
            For<Application.Communication.Strategies.ISendPasswordResetCodeStrategy>()
                .Use<QueueEmailOnlyPasswordResetCodeStrategy>();
            For<ISubmitApplicationStrategy>().Use<LegacySubmitApplicationStrategy>();
            For<ISendActivationCodeStrategy>().Use<QueueEmailOnlyActivationCodeStrategy>();
            For<ISendApplicationSubmittedStrategy>().Use<LegacyQueueApplicationSubmittedStrategy>();
            For<ISendPasswordChangedStrategy>().Use<QueueEmailOnlyPasswordChangedStrategy>();
            For<Application.Communication.Strategies.ISendAccountUnlockCodeStrategy>().Use<QueueEmailOnlyAccountUnlockCodeStrategy>();
            For<IResendActivationCodeStrategy>().Use<ResendActivationCodeStrategy>()
                .Ctor<ICodeGenerator>().Named(codeGenerator);
            For<Application.UserAccount.Strategies.ISendAccountUnlockCodeStrategy>().Use<SendAccountUnlockCodeStrategy>();
            For<ISaveCandidateStrategy>().Use<SaveCandidateStrategy>();

            For<IUnlockAccountStrategy>()
                .Use<UnlockAccountStrategy>()
                .Name = "UnlockAccountStrategy";

            For<IUnlockAccountStrategy>()
               .Use<LegacyUnlockAccountStrategy>()
               .Ctor<IUnlockAccountStrategy>()
               .Named("UnlockAccountStrategy")
               .Name = "LegacyUnlockAccountStrategy";
            
            For<ILockAccountStrategy>().Use<LockAccountStrategy>();
            For<ILockUserStrategy>().Use<LockUserStrategy>().Ctor<ICodeGenerator>().Named(codeGenerator);
            For<ICreateApplicationStrategy>().Use<CreateApplicationStrategy>();
            For<ISaveApplicationStrategy>().Use<SaveApplicationStrategy>();
            For<IArchiveApplicationStrategy>().Use<ArchiveApplicationStrategy>();
            For<IDeleteApplicationStrategy>().Use<DeleteApplicationStrategy>();
            For<IAuthenticateCandidateStrategy>().Use<AuthenticateCandidateStrategy>();
            For<IUserAccountService>().Use<UserAccountService>();
            For<IAddressSearchService>().Use<AddressSearchService>();
            For<IAuthenticationService>().Use<AuthenticationService>();
            For<ICommunicationService>().Use<CommunicationService>();
            For<IGetCandidateApplicationsStrategy>().Use<LegacyGetCandidateApplicationsStrategy>();
            For<IApplicationStatusUpdater>().Use<ApplicationStatusUpdater>();

            // providers (web)
            For<IMapper>().Singleton().Use<CandidateWebMappers>().Name = "CandidateWebMappers";
            For<ISearchProvider>().Use<SearchProvider>().Ctor<IMapper>().Named("CandidateWebMappers");
            For<IVacancyDetailProvider>().Use<VacancyDetailProvider>().Ctor<IMapper>().Named("CandidateWebMappers");
            For<IApplicationProvider>().Use<ApplicationProvider>();
            For<IAccountProvider>().Use<AccountProvider>();
            For<ICandidateServiceProvider>()
                .Use<CandidateServiceProvider>()
                .Ctor<IMapper>()
                .Named("CandidateWebMappers");

            For<HttpContextBase>().Use(ctx => new HttpContextWrapper(HttpContext.Current));

            // Would be good if we could do this for the base class CandidateControllerBase rather than each controller
            //ForConcreteType<HomeController>().Configure.Setter<IUserDataProvider>();
            //ForConcreteType<HomeController>().Configure.Setter<IEuCookieDirectiveProvider>();
        }
    }
}
