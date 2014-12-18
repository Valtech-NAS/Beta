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
    using Domain.Entities.Vacancies.Apprenticeships;
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
            For<IVacancySearchService<ApprenticeshipSummaryResponse, ApprenticeshipVacancyDetail>>().Use<VacancySearchService<ApprenticeshipSummaryResponse, ApprenticeshipVacancyDetail>>();
            For<IVacancySearchService<TraineeshipSummaryResponse, TraineeshipVacancyDetail>>().Use<VacancySearchService<TraineeshipSummaryResponse, TraineeshipVacancyDetail>>();

            For<ICandidateService>().Use<CandidateService>();
            For<IActivateCandidateStrategy>().Use<QueuedLegacyActivateCandidateStrategy>();
            For<IRegisterCandidateStrategy>().Use<RegisterCandidateStrategy>()
                .Ctor<ICodeGenerator>().Named(codeGenerator);
            For<IRegisterUserStrategy>().Use<RegisterUserStrategy>();
            For<IActivateUserStrategy>().Use<ActivateUserStrategy>();

            For<ICodeGenerator>().Use<RandomCodeGenerator>().Name = "RandomCodeGenerator";
            For<ICodeGenerator>().Use<StaticCodeGenerator>().Name = "StaticCodeGenerator";

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
            For<ISubmitTraineeshipApplicationStrategy>().Use<LegacySubmitTraineeshipApplicationStrategy>();
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
            For<ICreateTraineeshipApplicationStrategy>().Use<CreateTraineeshipApplicationStrategy>();
            For<ISaveApplicationStrategy>().Use<SaveApplicationStrategy>();
            For<ISaveTraineeshipApplicationStrategy>().Use<SaveTraineeshipApplicationStrategy>();
            For<IArchiveApplicationStrategy>().Use<ArchiveApplicationStrategy>();
            For<IDeleteApplicationStrategy>().Use<DeleteApplicationStrategy>();
            For<IAuthenticateCandidateStrategy>().Use<AuthenticateCandidateStrategy>();
            For<IUserAccountService>().Use<UserAccountService>();
            For<IAddressSearchService>().Use<AddressSearchService>();
            For<IAuthenticationService>().Use<AuthenticationService>();
            For<ICommunicationService>().Use<CommunicationService>();
            For<IGetCandidateApplicationsStrategy>().Use<LegacyGetCandidateApplicationsStrategy>();
            For<IApplicationStatusUpdater>().Use<ApplicationStatusUpdater>();
            
            // Apprenticeship providers (web)
            For<IMapper>().Singleton().Use<ApprenticeshipCandidateWebMappers>().Name = "ApprenticeshipCandidateWebMappers";
            For<IMapper>().Singleton().Use<TraineeshipCandidateWebMappers>().Name = "TraineeshipCandidateWebMappers";

            For<ISearchProvider>().Use<SearchProvider>()
                .Ctor<IMapper>("apprenticeshipSearchMapper").Named("ApprenticeshipCandidateWebMappers")
                .Ctor<IMapper>("traineeshipSearchMapper").Named("TraineeshipCandidateWebMappers"); 

            For<IApprenticeshipVacancyDetailProvider>().Use<ApprenticeshipVacancyDetailProvider>().Ctor<IMapper>().Named("ApprenticeshipCandidateWebMappers");
            For<IApprenticeshipApplicationProvider>().Use<ApprenticeshipApplicationProvider>().Ctor<IMapper>().Named("ApprenticeshipCandidateWebMappers");
            For<IAccountProvider>().Use<AccountProvider>().Ctor<IMapper>().Named("ApprenticeshipCandidateWebMappers");
            For<ICandidateServiceProvider>()
                .Use<CandidateServiceProvider>()
                .Ctor<IMapper>()
                .Named("ApprenticeshipCandidateWebMappers");

            // Traineeship providers (web)
            For<IMapper>().Singleton().Use<TraineeshipCandidateWebMappers>().Name = "TraineeshipCandidateWebMappers";
            For<ITraineeshipVacancyDetailProvider>()
                .Use<TraineeshipVacancyDetailProvider>()
                .Ctor<IMapper>()
                .Named("TraineeshipCandidateWebMappers");
            For<ITraineeshipApplicationProvider>().Use<TraineeshipApplicationProvider>();

            For<HttpContextBase>().Use(ctx => new HttpContextWrapper(HttpContext.Current));

            // Would be good if we could do this for the base class CandidateControllerBase rather than each controller
            //ForConcreteType<HomeController>().Configure.Setter<IUserDataProvider>();
            //ForConcreteType<HomeController>().Configure.Setter<IEuCookieDirectiveProvider>();
        }
    }
}
