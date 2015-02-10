namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Logging;
    using Domain.Entities.Vacancies;
    using Application.Interfaces.Candidates;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Constants.Pages;
    using ViewModels.Applications;
    using ViewModels.MyApplications;
    using Common.Models.Application;
    using ErrorCodes = Domain.Entities.ErrorCodes;
    using ApplicationErrorCodes = Application.Interfaces.Applications.ErrorCodes;

    //TODO: DFSW/AG This whole class needs refactoring or possibly reimplementing plus unit tests.
    public class ApprenticeshipApplicationProvider : IApprenticeshipApplicationProvider
    {
        private readonly ILogService _logger;
        private readonly IMapper _mapper;
        private readonly IApprenticeshipVacancyDetailProvider _apprenticeshipVacancyDetailProvider;
        private readonly ICandidateService _candidateService;
        private readonly IConfigurationManager _configurationManager;

        public ApprenticeshipApplicationProvider(
            IApprenticeshipVacancyDetailProvider apprenticeshipVacancyDetailProvider,
            ICandidateService candidateService,
            IMapper mapper,
            IConfigurationManager configurationManager, ILogService logger)
        {
            _apprenticeshipVacancyDetailProvider = apprenticeshipVacancyDetailProvider;
            _candidateService = candidateService;
            _mapper = mapper;
            _configurationManager = configurationManager;
            _logger = logger;
        }

        //TODO: Move all usages of GetOrCreateApplicationViewModel to this method
        public ApprenticeshipApplicationViewModel GetApplicationViewModel(Guid candidateId, int vacancyId)
        {
            _logger.Debug(
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
                if (e.Code == ErrorCodes.EntityStateError)
                {
                    _logger.Info(e.Message, e);
                    return
                        new ApprenticeshipApplicationViewModel(MyApplicationsPageMessages.ApplicationInIncorrectState,
                            ApplicationViewModelStatus.ApplicationInIncorrectState);
                }

                var message =
                    string.Format(
                        "Unhandled custom exception while getting the Application View Model for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);
                _logger.Error(message, e);
                return new ApprenticeshipApplicationViewModel(MyApplicationsPageMessages.UnhandledError, ApplicationViewModelStatus.Error);
            }
            catch (Exception e)
            {
                var message = string.Format("Get Application View Model failed for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);

                _logger.Error(message, e);

                return
                    new ApprenticeshipApplicationViewModel(
                        MyApplicationsPageMessages.CreateOrRetrieveApplicationFailed,
                        ApplicationViewModelStatus.Error);
            }
        }

        public ApprenticeshipApplicationViewModel GetOrCreateApplicationViewModel(Guid candidateId, int vacancyId)
        {
            _logger.Debug(
                "Calling ApprenticeshipApplicationProvider to get the Application View Model for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            try
            {
                var applicationDetails = _candidateService.CreateApplication(candidateId, vacancyId);
                if (applicationDetails == null)
                {
                    return new ApprenticeshipApplicationViewModel
                    {
                        Status = ApplicationStatuses.ExpiredOrWithdrawn,
                        ViewModelMessage = MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable
                    };
                }
                var applicationViewModel = _mapper.Map<ApprenticeshipApplicationDetail, ApprenticeshipApplicationViewModel>(applicationDetails);
                return PatchWithVacancyDetail(candidateId, vacancyId, applicationViewModel);
            }
            catch (CustomException e)
            {
                if (e.Code == ErrorCodes.EntityStateError)
                {
                    _logger.Info(e.Message, e);
                    return
                        new ApprenticeshipApplicationViewModel(MyApplicationsPageMessages.ApplicationInIncorrectState,
                            ApplicationViewModelStatus.ApplicationInIncorrectState);
                }

                var message =
                    string.Format(
                        "Unhandled custom exception while getting the Application View Model for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);
                _logger.Error(message, e);
                return new ApprenticeshipApplicationViewModel("Unhandled error", ApplicationViewModelStatus.Error);
            }
            catch (Exception e)
            {
                var message = string.Format("Get Application View Model failed for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);

                _logger.Error(message, e);

                return
                    new ApprenticeshipApplicationViewModel(
                        MyApplicationsPageMessages.CreateOrRetrieveApplicationFailed,
                        ApplicationViewModelStatus.Error);
            }
        }

        public ApprenticeshipApplicationViewModel PatchApplicationViewModel(Guid candidateId,
            ApprenticeshipApplicationViewModel savedModel, ApprenticeshipApplicationViewModel submittedModel)
        {
            _logger.Debug(
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
                if (submittedModel == null)
                {
                    message += " submittedModel was null";
                }
                else if (submittedModel.Candidate == null)
                {
                    message += " submittedModel.Candidate was null";
                }
                else if (submittedModel.Candidate.AboutYou == null)
                {
                    message += " submittedModel.Candidate.AboutYou was null";
                }
                _logger.Error(message, e);
                throw;
            }
        }

        public void SaveApplication(Guid candidateId, int vacancyId,
            ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel)
        {
            _logger.Debug(
                "Calling ApprenticeshipApplicationProvider to save the Application View Model for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            try
            {
                var application =
                    _mapper.Map<ApprenticeshipApplicationViewModel, ApprenticeshipApplicationDetail>(
                        apprenticeshipApplicationViewModel);

                _candidateService.SaveApplication(candidateId, vacancyId, application);
                _logger.Debug("Application View Model saved for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);
            }
            catch (Exception e)
            {
                var message =
                    string.Format(
                        "Save application failed for user {0}.",
                        candidateId);
                _logger.Error(message, e);
                throw;
            }
        }

        public ApprenticeshipApplicationViewModel SubmitApplication(Guid candidateId, int vacancyId)
        {
            _logger.Debug(
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

                _logger.Debug("Application submitted for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);

                return model;
            }
            catch (CustomException e)
            {
                if (e.Code == ApplicationErrorCodes.ApplicationInIncorrectStateError)
                {
                    _logger.Info(e.Message, e);
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
                _logger.Error(message, e);

                return FailedApplicationViewModel(vacancyId, candidateId, "Submission of application",
                    ApplicationPageMessages.SubmitApplicationFailed, e);
            }
            catch (Exception e)
            {
                var message =
                    string.Format(
                        "Submit Application failed for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);
                _logger.Error(message, e);

                return FailedApplicationViewModel(vacancyId, candidateId, "Submission of application",
                    ApplicationPageMessages.SubmitApplicationFailed, e);
            }
        }

        public ApprenticeshipApplicationViewModel ArchiveApplication(Guid candidateId, int vacancyId)
        {
            _logger.Debug(
                "Calling ApprenticeshipApplicationProvider to archive the Application for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            try
            {
                _candidateService.ArchiveApplication(candidateId, vacancyId);
                _logger.Debug("Application archived for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);
            }
            catch (Exception e)
            {
                var message =
                    string.Format(
                        "Archive application failed for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);
                _logger.Error(message, e);

                return FailedApplicationViewModel(vacancyId, candidateId, "Archive of application",
                    ApplicationPageMessages.ArchiveFailed, e);
            }

            return new ApprenticeshipApplicationViewModel();
        }

        public ApprenticeshipApplicationViewModel UnarchiveApplication(Guid candidateId, int vacancyId)
        {
            _logger.Debug(
                "Calling ApprenticeshipApplicationProvider to ensure Application is unarchived for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            try
            {
                _candidateService.UnarchiveApplication(candidateId, vacancyId);

                _logger.Debug("Application unarchived for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);
            }
            catch (Exception e)
            {
                var message =
                    string.Format(
                        "Unarchive application failed for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);
                _logger.Error(message, e);

                return FailedApplicationViewModel(vacancyId, candidateId, "Unarchive of application",
                    ApplicationPageMessages.UnarchiveFailed, e);
            }

            return new ApprenticeshipApplicationViewModel();
        }

        public ApprenticeshipApplicationViewModel DeleteApplication(Guid candidateId, int vacancyId)
        {
            _logger.Debug(
                "Calling ApprenticeshipApplicationProvider to delete the Application for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            try
            {
                _candidateService.DeleteApplication(candidateId, vacancyId);
                _logger.Debug("Application deleted for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);
            }
            catch (CustomException e)
            {
                if (e.Code == ErrorCodes.EntityStateError)
                {
                    _logger.Info(e.Message, e);
                    return new ApprenticeshipApplicationViewModel();
                }

                var message =
                    string.Format(
                        "Unhandled custom exception while deleting the Application for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);
                _logger.Error(message, e);

                return FailedApplicationViewModel(vacancyId, candidateId, "Delete of application",
                    ApplicationPageMessages.DeleteFailed, e);
            }
            catch (Exception e)
            {
                var message =
                    string.Format(
                        "Delete application failed for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);
                _logger.Error(message, e);

                return FailedApplicationViewModel(vacancyId, candidateId, "Delete of application",
                    ApplicationPageMessages.DeleteFailed, e);
            }

            return new ApprenticeshipApplicationViewModel();
        }

        public WhatHappensNextViewModel GetWhatHappensNextViewModel(Guid candidateId, int vacancyId)
        {
            _logger.Debug(
                "Calling ApprenticeshipApplicationProvider to get the What Happens Next data for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            try
            {
                var applicationDetails = _candidateService.GetApplication(candidateId, vacancyId);
                var candidate = _candidateService.GetCandidate(candidateId);

                if (applicationDetails == null || candidate == null)
                {
                    var message =
                    string.Format("Get What Happens Next View Model failed as no application was found for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);

                    _logger.Info(message);

                    return new WhatHappensNextViewModel(MyApplicationsPageMessages.ApplicationNotFound);
                }

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
                    SentEmail = candidate.CommunicationPreferences.AllowEmail,
                    VacancyStatus = patchedModel.VacancyDetail.VacancyStatus
                };
            }
            catch (Exception e)
            {
                var message =
                    string.Format("Get What Happens Next View Model failed for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);

                _logger.Error(message, e);

                return new WhatHappensNextViewModel(
                    MyApplicationsPageMessages.CreateOrRetrieveApplicationFailed);
            }
        }

        public MyApplicationsViewModel GetMyApplications(Guid candidateId)
        {
            _logger.Debug("Calling ApprenticeshipApplicationProvider to get the applications for candidate ID: {0}.",
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
                        VacancyStatus = each.VacancyStatus,
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

                _logger.Error(message, e);

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

                _logger.Error(message, e);

                throw;
            }
        }

        private TraineeshipFeatureViewModel GetTraineeshipFeatureViewModel(Guid candidateId, IList<ApprenticeshipApplicationSummary> apprenticeshipApplicationSummaries, IList<TraineeshipApplicationSummary> traineeshipApplicationSummaries)
        {
            var candididate = _candidateService.GetCandidate(candidateId);

            var unsuccessfulApplicationsToShowTraineeshipsPrompt = _configurationManager.GetCloudAppSetting<int>("UnsuccessfulApplicationsToShowTraineeshipsPrompt");
            var allowTraineeshipPrompts = candididate.CommunicationPreferences.AllowTraineeshipPrompts;

            var sufficentUnsuccessfulApprenticeshipApplicationsToPrompt = apprenticeshipApplicationSummaries.Count(each => each.Status == ApplicationStatuses.Unsuccessful) >= unsuccessfulApplicationsToShowTraineeshipsPrompt;
            var candidateHasSuccessfulApprenticeshipApplication = apprenticeshipApplicationSummaries.Any(each => each.Status == ApplicationStatuses.Successful);
            var candidateHasAppliedForTraineeship = traineeshipApplicationSummaries.Any();

            var viewModel = new TraineeshipFeatureViewModel
            {
                ShowTraineeshipsPrompt = allowTraineeshipPrompts && sufficentUnsuccessfulApprenticeshipApplicationsToPrompt && !candidateHasSuccessfulApprenticeshipApplication && !candidateHasAppliedForTraineeship,
                ShowTraineeshipsLink = (sufficentUnsuccessfulApprenticeshipApplicationsToPrompt || candidateHasAppliedForTraineeship)
            };

            return viewModel;
        }

        #region Helpers

        private ApprenticeshipApplicationViewModel FailedApplicationViewModel(
            int vacancyId,
            Guid candidateId,
            string failure,
            string failMessage, Exception e)
        {
            var message = string.Format("{0} {1} failed for user {2}", failure, vacancyId, candidateId);
            _logger.Error(message, e);
            return new ApprenticeshipApplicationViewModel(failMessage, ApplicationViewModelStatus.Error);
        }

        private ApprenticeshipApplicationViewModel PatchWithVacancyDetail(
            Guid candidateId, int vacancyId, ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel)
        {
            // TODO: why have a patch method like this? should be done in mapper.
            var vacancyDetailViewModel = _apprenticeshipVacancyDetailProvider.GetVacancyDetailViewModel(candidateId, vacancyId);

            if (vacancyDetailViewModel == null || vacancyDetailViewModel.VacancyStatus == VacancyStatuses.Unavailable)
            {
                apprenticeshipApplicationViewModel.ViewModelMessage = MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable;
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
