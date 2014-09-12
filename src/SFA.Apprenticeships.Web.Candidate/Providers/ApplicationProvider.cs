namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Linq;
    using Application.Interfaces.Candidates;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Mapping;
    using Constants.Pages;
    using NLog;
    using ViewModels.Applications;
    using ViewModels.MyApplications;
    using ErrorCodes = Domain.Entities.Exceptions.ErrorCodes;

    public class ApplicationProvider : IApplicationProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IVacancyDetailProvider _vacancyDetailProvider;
        private readonly ICandidateService _candidateService;
        private readonly IMapper _mapper;

        public ApplicationProvider(
            IVacancyDetailProvider vacancyDetailProvider,
            ICandidateService candidateService,
            IMapper mapper)
        {
            _vacancyDetailProvider = vacancyDetailProvider;
            _candidateService = candidateService;
            _mapper = mapper;
        }

        public ApplicationViewModel GetApplicationViewModel(Guid candidateId, int vacancyId)
        {
            try
            {
                var applicationDetails = _candidateService.CreateApplication(candidateId, vacancyId);
                var applicationViewModel = _mapper.Map<ApplicationDetail, ApplicationViewModel>(applicationDetails);

                return PatchWithVacancyDetail(candidateId, vacancyId, applicationViewModel);
            }
            catch (Exception e)
            {
                var message = string.Format("Get Application View Model failed for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);

                Logger.ErrorException(message, e);

                return new ApplicationViewModel(MyApplicationsPageMessages.CreateOrRetrieveApplicationFailed);
            }
        }

        public ApplicationViewModel PatchApplicationViewModel(Guid candidateId, ApplicationViewModel savedModel, ApplicationViewModel submittedModel)
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
            savedModel.ApplicationAction = submittedModel.ApplicationAction;

            return savedModel;
        }

        public void SaveApplication(Guid candidateId, int vacancyId, ApplicationViewModel applicationViewModel)
        {
            var application = _mapper.Map<ApplicationViewModel, ApplicationDetail>(applicationViewModel);

            _candidateService.SaveApplication(candidateId, vacancyId, application);
        }

        public ApplicationViewModel SubmitApplication(Guid candidateId, int vacancyId)
        {
            try
            {
                var model = GetApplicationViewModel(candidateId, vacancyId);
                
                if (model.HasError())
                {
                    return model;
                }

                _candidateService.SubmitApplication(candidateId, vacancyId);

                return model;
            }
            catch (CustomException e)
            {
                if (e.Code == ErrorCodes.ApplicationInIncorrectStateError)
                {
                    Logger.WarnException(e.Message, e);
                    return new ApplicationViewModel();
                }
                
                return ApplicationViewModelSubmitFailed(vacancyId, candidateId, e);
            }
            catch (Exception e)
            {
                return ApplicationViewModelSubmitFailed(vacancyId, candidateId, e);
            }
        }

        private ApplicationViewModel ApplicationViewModelSubmitFailed(int vacancyId, Guid candidateId, Exception ex)
        {
            var message = string.Format("Submission of application {0} failed for user {1}", vacancyId, candidateId);
            Logger.ErrorException(message, ex);
            return new ApplicationViewModel(ApplicationPageMessages.SubmitApplicationFailed);
        }

        public void ArchiveApplication(Guid candidateId, int vacancyId)
        {
            _candidateService.ArchiveApplication(candidateId, vacancyId);
        }

        public WhatHappensNextViewModel GetWhatHappensNextViewModel(Guid candidateId, int vacancyId)
        {
            try
            {
                var applicationDetails = _candidateService.GetApplication(candidateId, vacancyId);
                var model = _mapper.Map<ApplicationDetail, ApplicationViewModel>(applicationDetails);
                var patchedModel = PatchWithVacancyDetail(candidateId, vacancyId, model);

                if (patchedModel.HasError())
                {
                    return new WhatHappensNextViewModel(patchedModel.ViewModelMessage);
                }

                return new WhatHappensNextViewModel
                {
                    VacancyReference = patchedModel.VacancyDetail.FullVacancyReferenceId,
                    VacancyTitle = patchedModel.VacancyDetail.Title,
                    Status = patchedModel.Status
                };
            }
            catch (Exception e)
            {
                var message = string.Format("Get What Happens Next View Model failed for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);

                Logger.ErrorException(message, e);

                return new WhatHappensNextViewModel(
                    MyApplicationsPageMessages.CreateOrRetrieveApplicationFailed);
            }
        }

        public MyApplicationsViewModel GetMyApplications(Guid candidateId)
        {
            var applicationSummaries = _candidateService.GetApplications(candidateId);

            var applications = applicationSummaries
                .Select(each => new MyApplicationViewModel
                {
                    VacancyId = each.LegacyVacancyId,
                    Title = each.Title,
                    EmployerName = each.EmployerName,
                    UnsuccessfulReason = each.UnsuccessfulReason,
                    ApplicationStatus = each.Status,
                    IsArchived = each.IsArchived,
                    DateApplied = each.DateApplied,
                    ClosingDate = each.ClosingDate,
                    DateUpdated = each.DateUpdated
                })
                .ToList();

            return new MyApplicationsViewModel(applications);
        }

        #region Helpers

        private ApplicationViewModel PatchWithVacancyDetail(Guid candidateId, int vacancyId, ApplicationViewModel applicationViewModel)
        {
            // TODO: why have a patch method like this? should be done in mapper.
            var vacancyDetailViewModel = _vacancyDetailProvider.GetVacancyDetailViewModel(candidateId, vacancyId);

            if (vacancyDetailViewModel == null)
            {
                applicationViewModel.ViewModelMessage = MyApplicationsPageMessages.DraftExpired;
                applicationViewModel.Status = ApplicationStatuses.ExpiredOrWithdrawn;

                return applicationViewModel;
            }

            if (vacancyDetailViewModel.HasError())
            {
                applicationViewModel.ViewModelMessage = vacancyDetailViewModel.ViewModelMessage;

                return applicationViewModel;
            }

            applicationViewModel.VacancyDetail = vacancyDetailViewModel;
            applicationViewModel.Candidate.EmployerQuestionAnswers.SupplementaryQuestion1 = vacancyDetailViewModel.SupplementaryQuestion1;
            applicationViewModel.Candidate.EmployerQuestionAnswers.SupplementaryQuestion2 = vacancyDetailViewModel.SupplementaryQuestion2;

            return applicationViewModel;
        }

        #endregion
    }
}
