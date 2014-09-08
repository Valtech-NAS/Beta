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

    internal class ApplicationProvider : IApplicationProvider
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

                if (applicationDetails.Status == ApplicationStatuses.ExpiredOrWithdrawn)
                {
                    return new ApplicationViewModel(MyApplicationsPageMessages.DraftExpired)
                    {
                        Status = applicationDetails.Status
                    };
                }

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

        public void SubmitApplication(Guid candidateId, int vacancyId)
        {
            _candidateService.SubmitApplication(candidateId, vacancyId);
        }

        public void ArchiveApplication(Guid candidateId, int vacancyId)
        {
            _candidateService.ArchiveApplication(candidateId, vacancyId);
        }

        public WhatHappensNextViewModel GetSubmittedApplicationVacancySummary(Guid candidateId, int vacancyId)
        {
            var applicationDetails = _candidateService.GetApplication(candidateId, vacancyId);

            if (applicationDetails != null)
            {
                var model = _mapper.Map<ApplicationDetail, ApplicationViewModel>(applicationDetails);

                var patchedModel = PatchWithVacancyDetail(candidateId, vacancyId, model);

                if (patchedModel != null)
                {
                    return new WhatHappensNextViewModel
                    {
                        VacancyReference = patchedModel.VacancyDetail.FullVacancyReferenceId,
                        VacancyTitle = patchedModel.VacancyDetail.Title,
                        Status = patchedModel.Status
                    };
                }
            }

            return new WhatHappensNextViewModel(MyApplicationsPageMessages.DraftExpired)
            {
                Status = ApplicationStatuses.ExpiredOrWithdrawn
            };
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
                return null;
            }

            if (vacancyDetailViewModel.HasError())
            {
                throw new CustomException("VacancyDetail.Error", vacancyDetailViewModel.ViewModelMessage);
            }

            applicationViewModel.VacancyDetail = vacancyDetailViewModel;
            applicationViewModel.Candidate.EmployerQuestionAnswers.SupplementaryQuestion1 = vacancyDetailViewModel.SupplementaryQuestion1;
            applicationViewModel.Candidate.EmployerQuestionAnswers.SupplementaryQuestion2 = vacancyDetailViewModel.SupplementaryQuestion2;

            return applicationViewModel;
        }
        #endregion
    }
}
