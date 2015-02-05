namespace SFA.Apprenticeships.Web.Candidate.Mediators.Account
{
    using System;
    using Common.Constants;
    using Constants.Pages;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Configuration;
    using Providers;
    using Validators;
    using ViewModels.Account;
    using ViewModels.MyApplications;

    public class AccountMediator : MediatorBase, IAccountMediator
    {
        private readonly IApprenticeshipApplicationProvider _apprenticeshipApplicationProvider;
        private readonly IApprenticeshipVacancyDetailProvider _apprenticeshipVacancyDetailProvider;
        private readonly ITraineeshipVacancyDetailProvider _traineeshipVacancyDetailProvider;
        private readonly IConfigurationManager _configurationManager;
        private readonly IAccountProvider _accountProvider;
        private readonly ICandidateServiceProvider _candidateServiceProvider;
        private readonly SettingsViewModelServerValidator _settingsViewModelServerValidator;

        public AccountMediator(
            IAccountProvider accountProvider,
            ICandidateServiceProvider candidateServiceProvider,
            SettingsViewModelServerValidator settingsViewModelServerValidator, 
            IApprenticeshipApplicationProvider apprenticeshipApplicationProvider,
            IApprenticeshipVacancyDetailProvider apprenticeshipVacancyDetailProvider,
            ITraineeshipVacancyDetailProvider traineeshipVacancyDetailProvider,
            IConfigurationManager configurationManager)
        {
            _accountProvider = accountProvider;
            _candidateServiceProvider = candidateServiceProvider;
            _settingsViewModelServerValidator = settingsViewModelServerValidator;
            _apprenticeshipApplicationProvider = apprenticeshipApplicationProvider;
            _apprenticeshipVacancyDetailProvider = apprenticeshipVacancyDetailProvider;
            _configurationManager = configurationManager;
            _traineeshipVacancyDetailProvider = traineeshipVacancyDetailProvider;
        }

        public MediatorResponse<MyApplicationsViewModel> Index(Guid candidateId, string deletedVacancyId, string deletedVacancyTitle)
        {
            var model = _apprenticeshipApplicationProvider.GetMyApplications(candidateId);
            model.DeletedVacancyId = deletedVacancyId;
            model.DeletedVacancyTitle = deletedVacancyTitle;
            return GetMediatorResponse(Codes.AccountMediator.Index.Success, model);
        }

        public MediatorResponse Archive(Guid candidateId, int vacancyId)
        {
            var applicationViewModel = _apprenticeshipApplicationProvider.ArchiveApplication(candidateId, vacancyId);

            if (applicationViewModel.HasError())
            {
                return GetMediatorResponse(Codes.AccountMediator.Archive.ErrorArchiving, applicationViewModel.ViewModelMessage, UserMessageLevel.Warning);
            }

            return GetMediatorResponse(Codes.AccountMediator.Archive.SuccessfullyArchived, MyApplicationsPageMessages.ApplicationArchived, UserMessageLevel.Success);
        }

        public MediatorResponse Delete(Guid candidateId, int vacancyId)
        {
            var viewModel = _apprenticeshipApplicationProvider.GetApplicationViewModel(candidateId, vacancyId);

            if (viewModel.HasError())
            {
                if (viewModel.Status != ApplicationStatuses.ExpiredOrWithdrawn)
                {
                    return GetMediatorResponse(Codes.AccountMediator.Delete.AlreadyDeleted, MyApplicationsPageMessages.ApplicationDeleted, UserMessageLevel.Warning);
                }
            }

            var applicationViewModel = _apprenticeshipApplicationProvider.DeleteApplication(candidateId, vacancyId);

            if (applicationViewModel.HasError())
            {
                return GetMediatorResponse(Codes.AccountMediator.Delete.ErrorDeleting, applicationViewModel.ViewModelMessage, UserMessageLevel.Warning);
            }

            if (viewModel.VacancyDetail == null)
            {
                return GetMediatorResponse(Codes.AccountMediator.Delete.SuccessfullyDeletedExpiredOrWithdrawn, MyApplicationsPageMessages.ApplicationDeleted, UserMessageLevel.Success);
            }

            return GetMediatorResponse(Codes.AccountMediator.Delete.SuccessfullyDeleted, viewModel.VacancyDetail.Title, UserMessageLevel.Success);
        }

        public MediatorResponse DismissTraineeshipPrompts(Guid candidateId)
        {
            if (_accountProvider.DismissTraineeshipPrompts(candidateId))
            {
                return GetMediatorResponse(Codes.AccountMediator.DismissTraineeshipPrompts.SuccessfullyDismissed);                
            }

            return GetMediatorResponse(Codes.AccountMediator.DismissTraineeshipPrompts.ErrorDismissing, MyApplicationsPageMessages.DismissTraineeshipPromptsFailed, UserMessageLevel.Error);
        }

        public MediatorResponse<SettingsViewModel> Settings(Guid candidateId)
        {
            var model = _accountProvider.GetSettingsViewModel(candidateId);
            var traineeshipFeature = _apprenticeshipApplicationProvider.GetTraineeshipFeatureViewModel(candidateId);
            model.TraineeshipFeature = traineeshipFeature;
            return GetMediatorResponse(Codes.AccountMediator.Settings.Success, model);
        }

        public MediatorResponse<SettingsViewModel> SaveSettings(Guid candidateId, SettingsViewModel settingsViewModel)
        {
            var validationResult = _settingsViewModelServerValidator.Validate(settingsViewModel);
            var traineeshipFeature = _apprenticeshipApplicationProvider.GetTraineeshipFeatureViewModel(candidateId);
            settingsViewModel.TraineeshipFeature = traineeshipFeature;

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(Codes.AccountMediator.Settings.ValidationError, settingsViewModel, validationResult);
            }

            var saved = _accountProvider.SaveSettings(candidateId, settingsViewModel);

            return !saved
                ? GetMediatorResponse(Codes.AccountMediator.Settings.SaveError, settingsViewModel, AccountPageMessages.SettingsUpdateFailed, UserMessageLevel.Warning)
                : GetMediatorResponse(Codes.AccountMediator.Settings.Success, settingsViewModel);
        }

        public MediatorResponse Track(Guid candidateId, int vacancyId)
        {
            var applicationViewModel = _apprenticeshipApplicationProvider.UnarchiveApplication(candidateId, vacancyId);

            if (applicationViewModel.HasError())
            {
                return GetMediatorResponse(Codes.AccountMediator.Track.ErrorTracking, applicationViewModel.ViewModelMessage, UserMessageLevel.Warning);
            }

            return GetMediatorResponse(Codes.AccountMediator.Track.SuccessfullyTracked);
        }

        public MediatorResponse AcceptTermsAndConditions(Guid candidateId)
        {
            try
            {
                var candidate = _candidateServiceProvider.GetCandidate(candidateId);
                var currentTsAndCsVersion = _configurationManager.GetAppSetting<string>(Constants.Settings.TermsAndConditionsVersion);

                if (candidate.RegistrationDetails.AcceptedTermsAndConditionsVersion == currentTsAndCsVersion)
                {
                    return GetMediatorResponse(Codes.AccountMediator.AcceptTermsAndConditions.AlreadyAccepted);
                }

                var success = _candidateServiceProvider.AcceptTermsAndConditions(candidateId, currentTsAndCsVersion);

                if (success)
                {
                    return GetMediatorResponse(Codes.AccountMediator.AcceptTermsAndConditions.SuccessfullyAccepted);
                }

            }
            catch{}

            return GetMediatorResponse(Codes.AccountMediator.AcceptTermsAndConditions.ErrorAccepting);
        }

        public MediatorResponse ApprenticeshipVacancyDetails(Guid candidateId, int vacancyId)
        {
            var vacancyDetailViewModel = _apprenticeshipVacancyDetailProvider.GetVacancyDetailViewModel(candidateId, vacancyId);

            if (vacancyDetailViewModel == null || vacancyDetailViewModel.VacancyStatus == VacancyStatuses.Unavailable)
            {
                return GetMediatorResponse(Codes.AccountMediator.VacancyDetails.Unavailable, MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable, UserMessageLevel.Warning);
            }

            if (vacancyDetailViewModel.HasError())
            {
                return GetMediatorResponse(Codes.AccountMediator.VacancyDetails.Error, vacancyDetailViewModel.ViewModelMessage, UserMessageLevel.Error);
            }

            return GetMediatorResponse(Codes.AccountMediator.VacancyDetails.Available);
        }

        public MediatorResponse TraineeshipVacancyDetails(Guid candidateId, int vacancyId)
        {
            var vacancyDetailViewModel = _traineeshipVacancyDetailProvider.GetVacancyDetailViewModel(candidateId, vacancyId);

            if (vacancyDetailViewModel == null || vacancyDetailViewModel.VacancyStatus == VacancyStatuses.Unavailable)
            {
                return GetMediatorResponse(Codes.AccountMediator.VacancyDetails.Unavailable, MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable, UserMessageLevel.Warning);
            }

            if (vacancyDetailViewModel.HasError())
            {
                return GetMediatorResponse(Codes.AccountMediator.VacancyDetails.Error, vacancyDetailViewModel.ViewModelMessage, UserMessageLevel.Error);
            }

            return GetMediatorResponse(Codes.AccountMediator.VacancyDetails.Available);
        }
    }
}