namespace SFA.Apprenticeships.Web.Candidate.Mediators.Account
{
    using System;
    using Common.Constants;
    using Providers;
    using Validators;
    using ViewModels.Account;
    using Constants.Pages;
    using Domain.Entities.Applications;
    using ViewModels.MyApplications;

    public class AccountMediator : MediatorBase, IAccountMediator
    {
        private readonly IApprenticeshipApplicationProvider _apprenticeshipApplicationProvider;
        private readonly IAccountProvider _accountProvider;
        private readonly SettingsViewModelServerValidator _settingsViewModelServerValidator;

        public AccountMediator(
            IAccountProvider accountProvider,
            SettingsViewModelServerValidator settingsViewModelServerValidator, 
            IApprenticeshipApplicationProvider apprenticeshipApplicationProvider)
        {
            _accountProvider = accountProvider;
            _settingsViewModelServerValidator = settingsViewModelServerValidator;
            _apprenticeshipApplicationProvider = apprenticeshipApplicationProvider;
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

        public MediatorResponse<SettingsViewModel> Settings(Guid candidateId)
        {
            var model = _accountProvider.GetSettingsViewModel(candidateId);
            return GetMediatorResponse(Codes.AccountMediator.Settings.Success, model);
        }

        public MediatorResponse<SettingsViewModel> Settings(Guid candidateId, SettingsViewModel settingsViewModel)
        {
            var validationResult = _settingsViewModelServerValidator.Validate(settingsViewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(Codes.AccountMediator.Settings.ValidationError, settingsViewModel, validationResult);
            }

            var saved = _accountProvider.SaveSettings(candidateId, settingsViewModel);

            return !saved
                ? GetMediatorResponse(Codes.AccountMediator.Settings.SaveError, settingsViewModel, AccountPageMessages.SettingsUpdateFailed, UserMessageLevel.Warning)
                : GetMediatorResponse(Codes.AccountMediator.Settings.Success, settingsViewModel);
        }
    }
}