namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Security;
    using Common.Constants;
    using Common.Models.Application;
    using Common.Providers;
    using Constants.Pages;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Configuration;
    using Providers;
    using ViewModels.Applications;
    using ViewModels.Candidate;

    public class TraineeshipApplicationMediator : ApplicationMediatorBase, ITraineeshipApplicationMediator
    {
        private readonly ITraineeshipApplicationProvider _traineeshipApplicationProvider;

        public TraineeshipApplicationMediator(ITraineeshipApplicationProvider traineeshipApplicationProvider, IConfigurationManager configManager, IUserDataProvider userDataProvider) : base(configManager, userDataProvider)
        {
            _traineeshipApplicationProvider = traineeshipApplicationProvider;
        }

        public MediatorResponse<TraineeshipApplicationViewModel> Apply(Guid candidateId, int vacancyId)
        {
            var model = _traineeshipApplicationProvider.GetApplicationViewModel(candidateId, vacancyId);

            if (model.HasError())
            {
                return GetMediatorResponse<TraineeshipApplicationViewModel>(Codes.TraineeshipApplication.Apply.HasError);
            }

            model.SessionTimeout = FormsAuthentication.Timeout.TotalSeconds - 30;

            return GetMediatorResponse(Codes.TraineeshipApplication.Apply.Ok, model);
        }

        public MediatorResponse<TraineeshipApplicationViewModel> Submit(Guid candidateId, int vacancyId, TraineeshipApplicationViewModel viewModel)
        {
            viewModel = StripApplicationViewModelBeforeValidation(viewModel);

            var savedModel = _traineeshipApplicationProvider.GetApplicationViewModel(candidateId, vacancyId);

            viewModel = _traineeshipApplicationProvider.PatchApplicationViewModel(candidateId, savedModel, viewModel);

            var submittedApplicationModel = _traineeshipApplicationProvider.SubmitApplication(candidateId, vacancyId, viewModel);

            if (submittedApplicationModel.ViewModelStatus == ApplicationViewModelStatus.ApplicationInIncorrectState)
            {
                return GetMediatorResponse<TraineeshipApplicationViewModel>(Codes.TraineeshipApplication.Submit.IncorrectState);
            }
            if (submittedApplicationModel.ViewModelStatus == ApplicationViewModelStatus.Error)
            {
                // TODO: change this to something specific to traineeships?
                return GetMediatorResponse(Codes.TraineeshipApplication.Submit.Error, viewModel, ApplicationPageMessages.SubmitApplicationFailed, UserMessageLevel.Warning, new { vacancyId });
            }

            var parameters = new
            {
                vacancyId,
                vacancyReference = submittedApplicationModel.VacancyDetail.VacancyReference,
                vacancyTitle = submittedApplicationModel.VacancyDetail.Title
            };
            return GetMediatorResponse<TraineeshipApplicationViewModel>(Codes.TraineeshipApplication.Submit.Ok, parameters: parameters);
        }

        public MediatorResponse<TraineeshipApplicationViewModel> AddEmptyQualificationRows(TraineeshipApplicationViewModel viewModel)
        {
            viewModel.Candidate.Qualifications = RemoveEmptyRowsFromQualifications(viewModel.Candidate.Qualifications);
            viewModel.Candidate.HasQualifications = viewModel.Candidate.Qualifications.Count() != 0;
            viewModel.DefaultQualificationRows = 5;
            viewModel.DefaultWorkExperienceRows = 0;

            return GetMediatorResponse(Codes.TraineeshipApplication.AddEmptyQualificationRows.Ok, viewModel);
        }

        public MediatorResponse<TraineeshipApplicationViewModel> AddEmptyWorkExperienceRows(TraineeshipApplicationViewModel viewModel)
        {
            viewModel.Candidate.WorkExperience = RemoveEmptyRowsFromWorkExperience(viewModel.Candidate.WorkExperience);
            viewModel.Candidate.HasWorkExperience = viewModel.Candidate.WorkExperience.Count() != 0;

            viewModel.DefaultQualificationRows = 0;
            viewModel.DefaultWorkExperienceRows = 3;

            return GetMediatorResponse(Codes.TraineeshipApplication.AddEmptyWorkExperienceRows.Ok, viewModel);
        }

        public MediatorResponse<WhatHappensNextViewModel> WhatHappensNext(Guid candidateId, int vacancyId, string vacancyReference, string vacancyTitle)
        {
            var model = _traineeshipApplicationProvider.GetWhatHappensNextViewModel(candidateId, vacancyId);

            // TODO: change to something specific to traineeships?
            if (model.Status == ApplicationStatuses.ExpiredOrWithdrawn)
            {
                return GetMediatorResponse<WhatHappensNextViewModel>(Codes.TraineeshipApplication.WhatHappensNext.VacancyNotFound);
            }

            if (model.HasError())
            {
                model.VacancyReference = vacancyReference;
                model.VacancyTitle = vacancyTitle;
            }

            return GetMediatorResponse(Codes.TraineeshipApplication.WhatHappensNext.Ok, model);
        }

        private static TraineeshipApplicationViewModel StripApplicationViewModelBeforeValidation(TraineeshipApplicationViewModel model)
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