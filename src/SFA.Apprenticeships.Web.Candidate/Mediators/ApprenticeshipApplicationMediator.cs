﻿namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    using System;
    using System.Linq;
    using System.Web.Security;
    using Common.Constants;
    using Common.Models.Application;
    using Common.Providers;
    using Constants.Pages;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Configuration;
    using Helpers;
    using Providers;
    using Validators;
    using ViewModels.Applications;

    public class ApprenticeshipApplicationMediator : ApplicationMediatorBase, IApprenticeshipApplicationMediator
    {
        private readonly IApprenticeshipApplicationProvider _apprenticeshipApplicationProvider;
        private readonly ApprenticeshipApplicationViewModelServerValidator _apprenticeshipApplicationViewModelFullValidator;
        private readonly ApprenticeshipApplicationViewModelSaveValidator _apprenticeshipApplicationViewModelSaveValidator;

        public ApprenticeshipApplicationMediator(IApprenticeshipApplicationProvider apprenticeshipApplicationProvider, ApprenticeshipApplicationViewModelServerValidator apprenticeshipApplicationViewModelFullValidator, ApprenticeshipApplicationViewModelSaveValidator apprenticeshipApplicationViewModelSaveValidator, IConfigurationManager configManager, IUserDataProvider userDataProvider)
            : base(configManager, userDataProvider)
        {
            _apprenticeshipApplicationProvider = apprenticeshipApplicationProvider;
            _apprenticeshipApplicationViewModelFullValidator = apprenticeshipApplicationViewModelFullValidator;
            _apprenticeshipApplicationViewModelSaveValidator = apprenticeshipApplicationViewModelSaveValidator;
        }

        public MediatorResponse<ApprenticeshipApplicationViewModel> Resume(Guid candidateId, int vacancyId)
        {
            var model = _apprenticeshipApplicationProvider.GetOrCreateApplicationViewModel(candidateId, vacancyId);

            if (model.HasError())
            {
                return GetMediatorResponse<ApprenticeshipApplicationViewModel>(Codes.ApprenticeshipApplication.Resume.HasError, null, model.ViewModelMessage, UserMessageLevel.Warning);
            }

            return GetMediatorResponse<ApprenticeshipApplicationViewModel>(Codes.ApprenticeshipApplication.Submit.Ok, parameters: new { vacancyId });
        }

        public MediatorResponse<ApprenticeshipApplicationViewModel> Apply(Guid candidateId, int vacancyId)
        {
            var model = _apprenticeshipApplicationProvider.GetOrCreateApplicationViewModel(candidateId, vacancyId);

            if (model.Status == ApplicationStatuses.ExpiredOrWithdrawn)
            {
                return GetMediatorResponse<ApprenticeshipApplicationViewModel>(Codes.ApprenticeshipApplication.Apply.VacancyNotFound);
            }

            if (model.HasError())
            {
                return GetMediatorResponse<ApprenticeshipApplicationViewModel>(Codes.ApprenticeshipApplication.Apply.HasError);
            }

            model.SessionTimeout = FormsAuthentication.Timeout.TotalSeconds - 30;

            return GetMediatorResponse(Codes.ApprenticeshipApplication.Apply.Ok, model);
        }

        public MediatorResponse<ApprenticeshipApplicationViewModel> PreviewAndSubmit(Guid candidateId, int vacancyId, ApprenticeshipApplicationViewModel viewModel)
        {
            viewModel = StripApplicationViewModelBeforeValidation(viewModel);

            var savedModel = _apprenticeshipApplicationProvider.GetOrCreateApplicationViewModel(candidateId, vacancyId);

            if (savedModel.Status == ApplicationStatuses.ExpiredOrWithdrawn)
            {
                return GetMediatorResponse<ApprenticeshipApplicationViewModel>(Codes.ApprenticeshipApplication.PreviewAndSubmit.VacancyNotFound);
            }
            if (savedModel.Status != ApplicationStatuses.Draft)
            {
                return GetMediatorResponse<ApprenticeshipApplicationViewModel>(Codes.ApprenticeshipApplication.PreviewAndSubmit.IncorrectState);
            }

            viewModel.SessionTimeout = FormsAuthentication.Timeout.TotalSeconds - 30;

            if (savedModel.HasError())
            {
                return GetMediatorResponse(Codes.ApprenticeshipApplication.PreviewAndSubmit.Error, viewModel, ApplicationPageMessages.PreviewFailed, UserMessageLevel.Warning);
            }

            var result = _apprenticeshipApplicationViewModelFullValidator.Validate(viewModel);

            viewModel = _apprenticeshipApplicationProvider.PatchApplicationViewModel(candidateId, savedModel, viewModel);

            if (!result.IsValid)
            {
                return GetMediatorResponse(Codes.ApprenticeshipApplication.PreviewAndSubmit.ValidationError, viewModel, result);
            }

            _apprenticeshipApplicationProvider.SaveApplication(candidateId, vacancyId, viewModel);

            return GetMediatorResponse<ApprenticeshipApplicationViewModel>(Codes.ApprenticeshipApplication.PreviewAndSubmit.Ok, parameters: new { id = vacancyId });
        }

        public MediatorResponse<ApprenticeshipApplicationViewModel> Save(Guid candidateId, int vacancyId, ApprenticeshipApplicationViewModel viewModel)
        {
            viewModel = StripApplicationViewModelBeforeValidation(viewModel);

            var savedModel = _apprenticeshipApplicationProvider.GetOrCreateApplicationViewModel(candidateId, vacancyId);

            if (savedModel.Status == ApplicationStatuses.ExpiredOrWithdrawn)
            {
                return GetMediatorResponse<ApprenticeshipApplicationViewModel>(Codes.ApprenticeshipApplication.Save.VacancyNotFound);
            }

            viewModel.SessionTimeout = FormsAuthentication.Timeout.TotalSeconds - 30;

            if (savedModel.HasError())
            {
                return GetMediatorResponse(Codes.ApprenticeshipApplication.Save.Error, viewModel, ApplicationPageMessages.SaveFailed, UserMessageLevel.Warning);
            }

            var result = _apprenticeshipApplicationViewModelSaveValidator.Validate(viewModel);

            viewModel = _apprenticeshipApplicationProvider.PatchApplicationViewModel(candidateId, savedModel, viewModel);

            if (!result.IsValid)
            {
                return GetMediatorResponse(Codes.ApprenticeshipApplication.Save.ValidationError, viewModel, result);
            }

            _apprenticeshipApplicationProvider.SaveApplication(candidateId, vacancyId, viewModel);

            viewModel = _apprenticeshipApplicationProvider.GetOrCreateApplicationViewModel(candidateId, vacancyId);
            viewModel.SessionTimeout = FormsAuthentication.Timeout.TotalSeconds - 30;

            return GetMediatorResponse(Codes.ApprenticeshipApplication.Save.Ok, viewModel);
        }

        public MediatorResponse<AutoSaveResultViewModel> AutoSave(Guid candidateId, int vacancyId, ApprenticeshipApplicationViewModel viewModel)
        {
            var autoSaveResult = new AutoSaveResultViewModel();

            var savedModel = _apprenticeshipApplicationProvider.GetOrCreateApplicationViewModel(candidateId, vacancyId);

            if (savedModel.Status == ApplicationStatuses.ExpiredOrWithdrawn)
            {
                return GetMediatorResponse(Codes.ApprenticeshipApplication.AutoSave.VacancyNotFound, autoSaveResult);
            }

            viewModel.SessionTimeout = FormsAuthentication.Timeout.TotalSeconds - 30;

            if (savedModel.HasError())
            {
                return GetMediatorResponse(Codes.ApprenticeshipApplication.AutoSave.HasError, autoSaveResult);
            }

            var result = _apprenticeshipApplicationViewModelSaveValidator.Validate(viewModel);

            viewModel = _apprenticeshipApplicationProvider.PatchApplicationViewModel(candidateId, savedModel, viewModel);

            if (!result.IsValid)
            {
                return GetMediatorResponse(Codes.ApprenticeshipApplication.AutoSave.ValidationError, autoSaveResult, result);
            }

            _apprenticeshipApplicationProvider.SaveApplication(candidateId, vacancyId, viewModel);

            viewModel = _apprenticeshipApplicationProvider.GetOrCreateApplicationViewModel(candidateId, vacancyId);
            viewModel.SessionTimeout = FormsAuthentication.Timeout.TotalSeconds - 30;

            autoSaveResult.Status = "succeeded";

            if (viewModel.DateUpdated != null)
            {
                autoSaveResult.DateTimeMessage = AutoSaveDateTimeHelper.GetDisplayDateTime((DateTime)viewModel.DateUpdated);
            }

            return GetMediatorResponse(Codes.ApprenticeshipApplication.AutoSave.Ok, autoSaveResult);
        }

        public MediatorResponse<ApprenticeshipApplicationViewModel> Submit(Guid candidateId, int vacancyId)
        {
            var savedModel = _apprenticeshipApplicationProvider.GetApplicationViewModel(candidateId, vacancyId);

            var result = _apprenticeshipApplicationViewModelSaveValidator.Validate(savedModel);

            if (!result.IsValid)
            {
                return GetMediatorResponse(Codes.ApprenticeshipApplication.Submit.ValidationError, savedModel, result);
            }

            var model = _apprenticeshipApplicationProvider.SubmitApplication(candidateId, vacancyId);

            if (savedModel.Status == ApplicationStatuses.ExpiredOrWithdrawn)
            {
                return GetMediatorResponse<ApprenticeshipApplicationViewModel>(Codes.ApprenticeshipApplication.Submit.VacancyNotFound);
            }

            if (model.ViewModelStatus == ApplicationViewModelStatus.ApplicationInIncorrectState)
            {
                return GetMediatorResponse<ApprenticeshipApplicationViewModel>(Codes.ApprenticeshipApplication.Submit.IncorrectState);
            }
            if (model.ViewModelStatus == ApplicationViewModelStatus.Error)
            {
                return GetMediatorResponse<ApprenticeshipApplicationViewModel>(Codes.ApprenticeshipApplication.Submit.Error, null, ApplicationPageMessages.SubmitApplicationFailed, UserMessageLevel.Warning, new { id = vacancyId });
            }

            var parameters = new
            {
                id = vacancyId,
                vacancyReference = model.VacancyDetail.VacancyReference,
                vacancyTitle = model.VacancyDetail.Title
            };
            return GetMediatorResponse<ApprenticeshipApplicationViewModel>(Codes.ApprenticeshipApplication.Submit.Ok, parameters: parameters);
        }

        public MediatorResponse<ApprenticeshipApplicationViewModel> AddEmptyQualificationRows(ApprenticeshipApplicationViewModel viewModel)
        {
            viewModel.Candidate.Qualifications = RemoveEmptyRowsFromQualifications(viewModel.Candidate.Qualifications);
            viewModel.Candidate.HasQualifications = viewModel.Candidate.Qualifications.Count() != 0;
            viewModel.DefaultQualificationRows = 5;
            viewModel.DefaultWorkExperienceRows = 0;

            return GetMediatorResponse(Codes.ApprenticeshipApplication.AddEmptyQualificationRows.Ok, viewModel);
        }

        public MediatorResponse<ApprenticeshipApplicationViewModel> AddEmptyWorkExperienceRows(ApprenticeshipApplicationViewModel viewModel)
        {
            viewModel.Candidate.WorkExperience = RemoveEmptyRowsFromWorkExperience(viewModel.Candidate.WorkExperience);
            viewModel.Candidate.HasWorkExperience = viewModel.Candidate.WorkExperience.Count() != 0;

            viewModel.DefaultQualificationRows = 0;
            viewModel.DefaultWorkExperienceRows = 3;

            return GetMediatorResponse(Codes.ApprenticeshipApplication.AddEmptyWorkExperienceRows.Ok, viewModel);
        }

        public MediatorResponse<ApprenticeshipApplicationViewModel> Preview(Guid candidateId, int vacancyId)
        {
            var model = _apprenticeshipApplicationProvider.GetOrCreateApplicationViewModel(candidateId, vacancyId);

            if (model.Status == ApplicationStatuses.ExpiredOrWithdrawn)
            {
                return GetMediatorResponse<ApprenticeshipApplicationViewModel>(Codes.ApprenticeshipApplication.Preview.VacancyNotFound);
            }

            if (model.HasError())
            {
                return GetMediatorResponse<ApprenticeshipApplicationViewModel>(Codes.ApprenticeshipApplication.Preview.HasError);
            }

            return GetMediatorResponse(Codes.ApprenticeshipApplication.Preview.Ok, model);
        }

        public MediatorResponse<WhatHappensNextViewModel> WhatHappensNext(Guid candidateId, int vacancyId, string vacancyReference, string vacancyTitle)
        {
            var model = _apprenticeshipApplicationProvider.GetWhatHappensNextViewModel(candidateId, vacancyId);

            if (model.Status == ApplicationStatuses.ExpiredOrWithdrawn)
            {
                return GetMediatorResponse<WhatHappensNextViewModel>(Codes.ApprenticeshipApplication.WhatHappensNext.VacancyNotFound);
            }

            if (model.HasError())
            {
                model.VacancyReference = vacancyReference;
                model.VacancyTitle = vacancyTitle;
            }

            return GetMediatorResponse(Codes.ApprenticeshipApplication.WhatHappensNext.Ok, model);
        }

        private static ApprenticeshipApplicationViewModel StripApplicationViewModelBeforeValidation(ApprenticeshipApplicationViewModel model)
        {
            model.Candidate.Qualifications = RemoveEmptyRowsFromQualifications(model.Candidate.Qualifications);
            model.Candidate.WorkExperience = RemoveEmptyRowsFromWorkExperience(model.Candidate.WorkExperience);

            model.DefaultQualificationRows = 0;
            model.DefaultWorkExperienceRows = 0;

            if (model.IsJavascript)
            {
                return model;
            }

            model.Candidate.HasQualifications = model.Candidate.Qualifications.Count() != 0;
            model.Candidate.HasWorkExperience = model.Candidate.WorkExperience.Count() != 0;

            return model;
        }
    }
}