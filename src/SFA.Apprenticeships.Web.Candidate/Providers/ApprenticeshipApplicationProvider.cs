namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Configuration;
    using NLog;
    using Application.Interfaces.Candidates;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Infrastructure.PerformanceCounters;
    using Constants.Pages;
    using ViewModels.Applications;
    using ViewModels.MyApplications;
    using Common.Models.Application;
    using ErrorCodes = Domain.Entities.Exceptions.ErrorCodes;

    public class ApprenticeshipApplicationProvider : IApprenticeshipApplicationProvider
    {
        private const string WebRolePerformanceCounterCategory = "SFA.Apprenticeships.Web.Candidate";
        private const string ApplicationSubmissionCounter = "ApplicationSubmission";

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IApprenticeshipVacancyDetailProvider _apprenticeshipVacancyDetailProvider;
        private readonly ICandidateService _candidateService;
        private readonly IConfigurationManager _configurationManager;
        private readonly IFeatureToggle _featureToggle;
        private readonly IMapper _mapper;
        private readonly IPerformanceCounterService _performanceCounterService;

        public ApprenticeshipApplicationProvider(
            IApprenticeshipVacancyDetailProvider apprenticeshipVacancyDetailProvider,
            ICandidateService candidateService,
            IMapper mapper,
            IPerformanceCounterService performanceCounterService,
            IConfigurationManager configurationManager,
            IFeatureToggle featureToggle)
        {
            _apprenticeshipVacancyDetailProvider = apprenticeshipVacancyDetailProvider;
            _candidateService = candidateService;
            _mapper = mapper;
            _performanceCounterService = performanceCounterService;
            _configurationManager = configurationManager;
            _featureToggle = featureToggle;
        }

        //TODO: Move all usages of GetOrCreateApplicationViewModel to this method
        public ApprenticeshipApplicationViewModel GetApplicationViewModel(Guid candidateId, int vacancyId)
        {
            Logger.Debug(
                "Calling ApprenticeshipApplicationProvider to get the Application View Model for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            try
            {
                var applicationDetails = _candidateService.GetApplication(candidateId, vacancyId);
                if (applicationDetails == null)
                {
                    return new ApprenticeshipApplicationViewModel(MyApplicationsPageMessages.ApplicationNotFound, ApplicationViewModelStatus.ApplicationNotFound);
                }
                var applicationViewModel = _mapper.Map<ApprenticeshipApplicationDetail, ApprenticeshipApplicationViewModel>(applicationDetails);
                return PatchWithVacancyDetail(candidateId, vacancyId, applicationViewModel);
            }
            catch (CustomException e)
            {
                if (e.Code == ErrorCodes.ApplicationInIncorrectStateError)
                {
                    Logger.Info(e.Message, e);
                    return
                        new ApprenticeshipApplicationViewModel(MyApplicationsPageMessages.ApplicationInIncorrectState,
                            ApplicationViewModelStatus.ApplicationInIncorrectState);
                }

                var message =
                    string.Format(
                        "Unhandled custom exception while getting the Application View Model for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);
                Logger.Error(message, e);
                return new ApprenticeshipApplicationViewModel("Unhandled error", ApplicationViewModelStatus.Error);
            }
            catch (Exception e)
            {
                var message = string.Format("Get Application View Model failed for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);

                Logger.Error(message, e);

                return
                    new ApprenticeshipApplicationViewModel(
                        MyApplicationsPageMessages.CreateOrRetrieveApplicationFailed,
                        ApplicationViewModelStatus.Error);
            }
        }

        public ApprenticeshipApplicationViewModel GetOrCreateApplicationViewModel(Guid candidateId, int vacancyId)
        {
            Logger.Debug(
                "Calling ApprenticeshipApplicationProvider to get the Application View Model for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            try
            {
                var applicationDetails = _candidateService.CreateApplication(candidateId, vacancyId);
                var applicationViewModel = _mapper.Map<ApprenticeshipApplicationDetail, ApprenticeshipApplicationViewModel>(applicationDetails);
                return PatchWithVacancyDetail(candidateId, vacancyId, applicationViewModel);
            }
            catch (CustomException e)
            {
                if (e.Code == ErrorCodes.ApplicationInIncorrectStateError)
                {
                    Logger.Info(e.Message, e);
                    return
                        new ApprenticeshipApplicationViewModel(MyApplicationsPageMessages.ApplicationInIncorrectState,
                            ApplicationViewModelStatus.ApplicationInIncorrectState);
                }

                var message =
                    string.Format(
                        "Unhandled custom exception while getting the Application View Model for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);
                Logger.Error(message, e);
                return new ApprenticeshipApplicationViewModel("Unhandled error", ApplicationViewModelStatus.Error);
            }
            catch (Exception e)
            {
                var message = string.Format("Get Application View Model failed for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);

                Logger.Error(message, e);

                return
                    new ApprenticeshipApplicationViewModel(
                        MyApplicationsPageMessages.CreateOrRetrieveApplicationFailed,
                        ApplicationViewModelStatus.Error);
            }
        }

        public ApprenticeshipApplicationViewModel PatchApplicationViewModel(Guid candidateId,
            ApprenticeshipApplicationViewModel savedModel, ApprenticeshipApplicationViewModel submittedModel)
        {
            Logger.Debug(
                "Calling ApprenticeshipApplicationProvider to patch the Application View Model for candidate ID: {0}.",
                candidateId);

            try
            {
                if (!submittedModel.Candidate.AboutYou.RequiresSupportForInterview)
                {
                    submittedModel.Candidate.AboutYou.AnythingWeCanDoToSupportYourInterview = string.Empty;
                }

                savedModel.Candidate.AboutYou = submittedModel.Candidate.AboutYou;
                savedModel.Candidate.Education = submittedModel.Candidate.Education;
                savedModel.Candidate.HasQualifications = submittedModel.Candidate.HasQualifications;
                savedModel.Candidate.Qualifications = submittedModel.Candidate.Qualifications;
                savedModel.Candidate.HasWorkExperience = submittedModel.Candidate.HasWorkExperience;
                savedModel.Candidate.WorkExperience = submittedModel.Candidate.WorkExperience;
                savedModel.Candidate.EmployerQuestionAnswers = submittedModel.Candidate.EmployerQuestionAnswers;

                return savedModel;
            }
            catch (Exception e)
            {
                var message =
                    string.Format(
                        "Patch application View Model failed for user {0}.", candidateId);
                Logger.Error(message, e);
                throw;
            }
        }

        public void SaveApplication(Guid candidateId, int vacancyId,
            ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel)
        {
            Logger.Debug(
                "Calling ApprenticeshipApplicationProvider to save the Application View Model for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            try
            {
                var application =
                    _mapper.Map<ApprenticeshipApplicationViewModel, ApprenticeshipApplicationDetail>(
                        apprenticeshipApplicationViewModel);

                _candidateService.SaveApplication(candidateId, vacancyId, application);
                Logger.Debug("Application View Model saved for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);
            }
            catch (Exception e)
            {
                var message =
                    string.Format(
                        "Save application failed for user {0}.",
                        candidateId);
                Logger.Error(message, e);
                throw;
            }
        }

        public ApprenticeshipApplicationViewModel SubmitApplication(Guid candidateId, int vacancyId)
        {
            Logger.Debug(
                "Calling ApprenticeshipApplicationProvider to submit the Application for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            var model = new ApprenticeshipApplicationViewModel();

            try
            {
                model = GetOrCreateApplicationViewModel(candidateId, vacancyId);

                if (model.HasError())
                {
                    return model;
                }

                _candidateService.SubmitApplication(candidateId, vacancyId);

                IncrementApplicationSubmissionCounter();


                Logger.Debug("Application submitted for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);

                return model;
            }
            catch (CustomException e)
            {
                if (e.Code == ErrorCodes.ApplicationInIncorrectStateError)
                {
                    Logger.Info(e.Message, e);
                    return
                        new ApprenticeshipApplicationViewModel(ApplicationViewModelStatus.ApplicationInIncorrectState)
                        {
                            Status = model.Status
                        };
                }

                var message =
                    string.Format(
                        "Unhandled custom exception while submitting the Application for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);
                Logger.Error(message, e);

                return FailedApplicationViewModel(vacancyId, candidateId, "Submission of application",
                    ApplicationPageMessages.SubmitApplicationFailed, e);
            }
            catch (Exception e)
            {
                var message =
                    string.Format(
                        "Submit Application failed for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);
                Logger.Error(message, e);

                return FailedApplicationViewModel(vacancyId, candidateId, "Submission of application",
                    ApplicationPageMessages.SubmitApplicationFailed, e);
            }
        }

        public ApprenticeshipApplicationViewModel ArchiveApplication(Guid candidateId, int vacancyId)
        {
            Logger.Debug(
                "Calling ApprenticeshipApplicationProvider to archive the Application for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            try
            {
                _candidateService.ArchiveApplication(candidateId, vacancyId);
                Logger.Debug("Application archived for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);
            }
            catch (Exception e)
            {
                var message =
                    string.Format(
                        "Archive application failed for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);
                Logger.Error(message, e);

                return FailedApplicationViewModel(vacancyId, candidateId, "Archive of application",
                    ApplicationPageMessages.ArchiveFailed, e);
            }

            return new ApprenticeshipApplicationViewModel();
        }

        public ApprenticeshipApplicationViewModel UnarchiveApplication(Guid candidateId, int vacancyId)
        {
            Logger.Debug(
                "Calling ApprenticeshipApplicationProvider to ensure Application is unarchived for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            try
            {
                _candidateService.UnarchiveApplication(candidateId, vacancyId);

                Logger.Debug("Application unarchived for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);
            }
            catch (Exception e)
            {
                var message =
                    string.Format(
                        "Unarchive application failed for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);
                Logger.Error(message, e);

                return FailedApplicationViewModel(vacancyId, candidateId, "Unarchive of application",
                    ApplicationPageMessages.UnarchiveFailed, e);
            }

            return new ApprenticeshipApplicationViewModel();
        }

        public ApprenticeshipApplicationViewModel DeleteApplication(Guid candidateId, int vacancyId)
        {
            Logger.Debug(
                "Calling ApprenticeshipApplicationProvider to delete the Application for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            try
            {
                _candidateService.DeleteApplication(candidateId, vacancyId);
                Logger.Debug("Application deleted for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);
            }
            catch (CustomException e)
            {
                if (e.Code == ErrorCodes.ApplicationInIncorrectStateError)
                {
                    Logger.Info(e.Message, e);
                    return new ApprenticeshipApplicationViewModel();
                }

                var message =
                    string.Format(
                        "Unhandled custom exception while deleting the Application for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);
                Logger.Error(message, e);

                return FailedApplicationViewModel(vacancyId, candidateId, "Delete of application",
                    ApplicationPageMessages.DeleteFailed, e);
            }
            catch (Exception e)
            {
                var message =
                    string.Format(
                        "Delete application failed for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);
                Logger.Error(message, e);

                return FailedApplicationViewModel(vacancyId, candidateId, "Delete of application",
                    ApplicationPageMessages.DeleteFailed, e);
            }

            return new ApprenticeshipApplicationViewModel();
        }

        public WhatHappensNextViewModel GetWhatHappensNextViewModel(Guid candidateId, int vacancyId)
        {
            Logger.Debug(
                "Calling ApprenticeshipApplicationProvider to get the What Happens Next data for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            try
            {
                var applicationDetails = _candidateService.GetApplication(candidateId, vacancyId);
                var candidate = _candidateService.GetCandidate(candidateId);
                var model =
                    _mapper.Map<ApprenticeshipApplicationDetail, ApprenticeshipApplicationViewModel>(applicationDetails);
                var patchedModel = PatchWithVacancyDetail(candidateId, vacancyId, model);

                if (patchedModel.HasError())
                {
                    return new WhatHappensNextViewModel(patchedModel.ViewModelMessage);
                }

                return new WhatHappensNextViewModel
                {
                    VacancyReference = patchedModel.VacancyDetail.VacancyReference,
                    VacancyTitle = patchedModel.VacancyDetail.Title,
                    Status = patchedModel.Status,
                    SentEmail = candidate.CommunicationPreferences.AllowEmail
                };
            }
            catch (Exception e)
            {
                var message =
                    string.Format("Get What Happens Next View Model failed for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);

                Logger.Error(message, e);

                return new WhatHappensNextViewModel(
                    MyApplicationsPageMessages.CreateOrRetrieveApplicationFailed);
            }
        }

        public MyApplicationsViewModel GetMyApplications(Guid candidateId)
        {
            Logger.Debug("Calling ApprenticeshipApplicationProvider to get the applications for candidate ID: {0}.",
                candidateId);

            try
            {
                var apprenticeshipApplicationSummaries = _candidateService.GetApprenticeshipApplications(candidateId);

                var apprenticeshipApplications = apprenticeshipApplicationSummaries
                    .Select(each => new MyApprenticeshipApplicationViewModel
                    {
                        VacancyId = each.LegacyVacancyId,
                        Title = each.Title,
                        EmployerName = each.EmployerName,
                        UnsuccessfulReason = each.UnsuccessfulReason,
                        ApplicationStatus = each.Status,
                        VacancyStatus = each.VacancyStatus,
                        IsArchived = each.IsArchived,
                        DateApplied = each.DateApplied,
                        ClosingDate = each.ClosingDate,
                        DateUpdated = each.DateUpdated
                    })
                    .ToList();

                var traineeshipApplicationSummaries = _candidateService.GetTraineeshipApplications(candidateId);

                var traineeshipApplications = traineeshipApplicationSummaries
                    .Select(each => new MyTraineeshipApplicationViewModel
                    {
                        VacancyId = each.LegacyVacancyId,
                        Title = each.Title,
                        EmployerName = each.EmployerName,
                        IsArchived = each.IsArchived,
                        DateApplied = each.DateApplied
                    })
                    .ToList();

                var traineeshipFeatureViewModel = GetTraineeshipFeatureViewModel(candidateId, apprenticeshipApplicationSummaries, traineeshipApplicationSummaries);

                return new MyApplicationsViewModel(
                    apprenticeshipApplications, traineeshipApplications, traineeshipFeatureViewModel);
            }
            catch (Exception e)
            {
                var message = string.Format("Get MyApplications failed for candidate ID: {0}.", candidateId);

                Logger.Error(message, e);

                throw;
            }
        }

        public TraineeshipFeatureViewModel GetTraineeshipFeatureViewModel(Guid candidateId)
        {
            try
            {
                var apprenticeshipApplicationSummaries = _candidateService.GetApprenticeshipApplications(candidateId);
                var traineeshipApplicationSummaries = _candidateService.GetTraineeshipApplications(candidateId);
                var traineeshipFeatureViewModel = GetTraineeshipFeatureViewModel(candidateId, apprenticeshipApplicationSummaries, traineeshipApplicationSummaries);

                return traineeshipFeatureViewModel;
            }
            catch (Exception e)
            {
                var message = string.Format("Get Traineeship Feature View Model failed for candidate ID: {0}.", candidateId);

                Logger.Error(message, e);

                throw;
            }
        }

        private void IncrementApplicationSubmissionCounter()
        {
            if (_configurationManager.GetCloudAppSetting<bool>("PerformanceCountersEnabled"))
            {
                _performanceCounterService.IncrementCounter(WebRolePerformanceCounterCategory,
                    ApplicationSubmissionCounter);
            }
        }

        private TraineeshipFeatureViewModel GetTraineeshipFeatureViewModel(Guid candidateId, IList<ApprenticeshipApplicationSummary> apprenticeshipApplicationSummaries, IList<TraineeshipApplicationSummary> traineeshipApplicationSummaries)
        {
            var candididate = _candidateService.GetCandidate(candidateId);

            var unsuccessfulApplicationsToShowTraineeshipsPrompt = _configurationManager.GetCloudAppSetting<int>("UnsuccessfulApplicationsToShowTraineeshipsPrompt");
            var traineeshipsFeatureActive = _featureToggle.IsActive(Feature.Traineeships);
            var allowTraineeshipPrompts = candididate.CommunicationPreferences.AllowTraineeshipPrompts;

            var sufficentUnsuccessfulApprenticeshipApplicationsToPrompt = apprenticeshipApplicationSummaries.Count(each => each.Status == ApplicationStatuses.Unsuccessful) >= unsuccessfulApplicationsToShowTraineeshipsPrompt;
            var candidateHasSuccessfulApprenticeshipApplication = apprenticeshipApplicationSummaries.Any(each => each.Status == ApplicationStatuses.Successful);
            var candidateHasAppliedForTraineeship = traineeshipApplicationSummaries.Any();

            var viewModel = new TraineeshipFeatureViewModel
            {
                ShowTraineeshipsPrompt = traineeshipsFeatureActive && allowTraineeshipPrompts && sufficentUnsuccessfulApprenticeshipApplicationsToPrompt && !candidateHasSuccessfulApprenticeshipApplication && !candidateHasAppliedForTraineeship,
                ShowTraineeshipsLink = traineeshipsFeatureActive && (sufficentUnsuccessfulApprenticeshipApplicationsToPrompt || candidateHasAppliedForTraineeship)
            };

            return viewModel;
        }

        #region Helpers

        private static ApprenticeshipApplicationViewModel FailedApplicationViewModel(
            int vacancyId, 
            Guid candidateId,
            string failure,
            string failMessage, Exception e)
        {
            var message = string.Format("{0} {1} failed for user {2}", failure, vacancyId, candidateId);
            Logger.Error(message, e);
            return new ApprenticeshipApplicationViewModel(failMessage, ApplicationViewModelStatus.Error);
        }

        private ApprenticeshipApplicationViewModel PatchWithVacancyDetail(Guid candidateId, int vacancyId,
            ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel)
        {
            // TODO: why have a patch method like this? should be done in mapper.
            var vacancyDetailViewModel = _apprenticeshipVacancyDetailProvider.GetVacancyDetailViewModel(candidateId, vacancyId);

            if (vacancyDetailViewModel == null)
            {
                apprenticeshipApplicationViewModel.ViewModelMessage = MyApplicationsPageMessages.DraftExpired;
                apprenticeshipApplicationViewModel.Status = ApplicationStatuses.ExpiredOrWithdrawn;

                return apprenticeshipApplicationViewModel;
            }

            if (vacancyDetailViewModel.HasError())
            {
                apprenticeshipApplicationViewModel.ViewModelMessage = vacancyDetailViewModel.ViewModelMessage;

                return apprenticeshipApplicationViewModel;
            }

            apprenticeshipApplicationViewModel.VacancyDetail = vacancyDetailViewModel;
            apprenticeshipApplicationViewModel.Candidate.EmployerQuestionAnswers.SupplementaryQuestion1 = vacancyDetailViewModel.SupplementaryQuestion1;
            apprenticeshipApplicationViewModel.Candidate.EmployerQuestionAnswers.SupplementaryQuestion2 = vacancyDetailViewModel.SupplementaryQuestion2;

            return apprenticeshipApplicationViewModel;
        }

        #endregion
    }
}
