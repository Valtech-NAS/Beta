namespace SFA.Apprenticeships.Web.Candidate.IoC
{
    using System.Web;
    using Application.Address;
    using Application.ApplicationUpdate;
    using Application.Authentication;
    using Application.Candidate;
    using Application.Candidate.Strategies;
    using Application.Candidate.Strategies.Apprenticeships;
    using Application.Candidate.Strategies.Traineeships;
    using Application.Communication;
    using Application.Communication.Strategies;
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Locations;
    using Application.Interfaces.Messaging;
    using Application.Interfaces.ReferenceData;
    using Application.Interfaces.Search;
    using Application.Interfaces.Users;
    using Application.Interfaces.Vacancies;
    using Application.Location;
    using Application.ReferenceData;
    using Application.UserAccount;
    using Application.UserAccount.Strategies;
    using Application.Vacancy;
    using Configuration;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.Traineeships;
    using Domain.Interfaces.Mapping;
    using Infrastructure.LegacyWebServices.ReferenceData;
    using Mappers;
    using Mediators;
    using Mediators.Account;
    using Mediators.Register;
    using Mediators.Traineeships;
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
            For<IVacancySearchService<ApprenticeshipSummaryResponse, ApprenticeshipVacancyDetail, ApprenticeshipSearchParameters>>().Use<VacancySearchService<ApprenticeshipSummaryResponse, ApprenticeshipVacancyDetail, ApprenticeshipSearchParameters>>();
            For<IVacancySearchService<TraineeshipSummaryResponse, TraineeshipVacancyDetail, TraineeshipSearchParameters>>().Use<VacancySearchService<TraineeshipSummaryResponse, TraineeshipVacancyDetail, TraineeshipSearchParameters>>();

            For<ICandidateService>().Use<CandidateService>()
                .Ctor<ISubmitApprenticeshipApplicationStrategy>("submitApprenticeshipApplicationStrategy").Is<LegacySubmitApprenticeshipApplicationStrategy>()
                .Ctor<ISubmitTraineeshipApplicationStrategy>("submitTraineeshipApplicationStrategy").Is<LegacySubmitTraineeshipApplicationStrategy>();

            For<ISendCandidateCommunicationStrategy>().Use<QueueCandidateCommunicationStrategy>();
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
            For<ISubmitApprenticeshipApplicationStrategy>().Use<LegacySubmitApprenticeshipApplicationStrategy>();
            For<ISendApplicationSubmittedStrategy>().Use<LegacyQueueApprenticeshipApplicationSubmittedStrategy>();
            For<ISendTraineeshipApplicationSubmittedStrategy>().Use<LegacyQueueTraineeshipApplicationSubmittedStrategy>(); 
            For<IResendActivationCodeStrategy>().Use<ResendActivationCodeStrategy>().Ctor<ICodeGenerator>().Named(codeGenerator);
            For<ISendAccountUnlockCodeStrategy>().Use<SendAccountUnlockCodeStrategy>();
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
            For<ICreateApprenticeshipApplicationStrategy>().Use<CreateApprenticeshipApplicationStrategy>();
            For<ICreateTraineeshipApplicationStrategy>().Use<CreateTraineeshipApplicationStrategy>();
            For<ISaveApprenticeshipApplicationStrategy>().Use<SaveApprenticeshipApplicationStrategy>();
            For<ISaveTraineeshipApplicationStrategy>().Use<SaveTraineeshipApplicationStrategy>();
            For<IArchiveApplicationStrategy>().Use<ArchiveApprenticeshipApplicationStrategy>();
            For<IDeleteApplicationStrategy>().Use<DeleteApprenticeshipApplicationStrategy>();
            For<IAuthenticateCandidateStrategy>().Use<AuthenticateCandidateStrategy>();
            For<IUserAccountService>().Use<UserAccountService>();
            For<IAddressSearchService>().Use<AddressSearchService>();
            For<IAuthenticationService>().Use<AuthenticationService>();
            For<ICommunicationService>().Use<CommunicationService>();
            For<IGetCandidateApprenticeshipApplicationsStrategy>().Use<LegacyGetCandidateApprenticeshipApplicationsStrategy>();
            For<ILegacyGetCandidateVacancyDetailStrategy<ApprenticeshipVacancyDetail>>().Use<LegacyGetCandidateVacancyDetailStrategy<ApprenticeshipVacancyDetail>>();
            For<ILegacyGetCandidateVacancyDetailStrategy<TraineeshipVacancyDetail>>().Use<LegacyGetCandidateVacancyDetailStrategy<TraineeshipVacancyDetail>>();
            For<IApplicationStatusUpdater>().Use<ApplicationStatusUpdater>();
            For<IApplicationVacancyStatusUpdater>().Use<ApplicationVacancyStatusUpdater>();
            For<IReferenceDataService>().Use<ReferenceDataService>();
            For<IReferenceDataProvider>().Use<ReferenceDataProvider>();
            
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

            For<IFeatureToggle>().Use<FeatureToggle>();

            // Traineeship providers (web)
            For<IMapper>().Singleton().Use<TraineeshipCandidateWebMappers>().Name = "TraineeshipCandidateWebMappers";
            For<ITraineeshipVacancyDetailProvider>()
                .Use<TraineeshipVacancyDetailProvider>()
                .Ctor<IMapper>()
                .Named("TraineeshipCandidateWebMappers");
            For<ITraineeshipApplicationProvider>().Use<TraineeshipApplicationProvider>();
            For<IGetCandidateTraineeshipApplicationsStrategy>().Use<GetCandidateTraineeshipApplicationsStrategy>();

            For<HttpContextBase>().Use(ctx => new HttpContextWrapper(HttpContext.Current));

            // Would be good if we could do this for the base class CandidateControllerBase rather than each controller
            //ForConcreteType<HomeController>().Configure.Setter<IUserDataProvider>();
            //ForConcreteType<HomeController>().Configure.Setter<IEuCookieDirectiveProvider>();

            //Mediators
            For<IApprenticeshipApplicationMediator>().Use<ApprenticeshipApplicationMediator>();
            For<IApprenticeshipSearchMediator>().Use<ApprenticeshipSearchMediator>();
            For<ITraineeshipApplicationMediator>().Use<TraineeshipApplicationMediator>();
            For<ITraineeshipSearchMediator>().Use<TraineeshipSearchMediator>();
            For<IAccountMediator>().Use<AccountMediator>();
            For<IRegisterMediator>().Use<RegisterMediator>();
            For<ILoginMediator>().Use<LoginMediator>();
        }
    }
}
