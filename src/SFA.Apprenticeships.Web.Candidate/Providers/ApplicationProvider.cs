namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Linq;
    using Application.Interfaces.Candidates;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Mapping;
    using SFA.Apprenticeships.Web.Candidate.Constants.Pages;
    using ViewModels.Applications;
    using ViewModels.MyApplications;

    internal class ApplicationProvider : IApplicationProvider
    {
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
            catch (CustomException ex)
            {
                if (ex.Code == ErrorCodes.VacancyExpired)
                {
                    return new ApplicationViewModel(MyApplicationsPageMessages.DraftExpired);
                }
                else
                {
                    return new ApplicationViewModel(MyApplicationsPageMessages.RetrieveApplicationFailed);
                }
            }
        }

        public ApplicationViewModel PatchApplicationViewModel(Guid candidateId, ApplicationViewModel savedApplicationViewModel, ApplicationViewModel submittedApplicationViewModel)
        {
            if (!submittedApplicationViewModel.Candidate.AboutYou.RequiresSupportForInterview)
            {
                submittedApplicationViewModel.Candidate.AboutYou.AnythingWeCanDoToSupportYourInterview = string.Empty;
            }

            savedApplicationViewModel.Candidate.AboutYou = submittedApplicationViewModel.Candidate.AboutYou;
            savedApplicationViewModel.Candidate.Education = submittedApplicationViewModel.Candidate.Education;
            savedApplicationViewModel.Candidate.HasQualifications = submittedApplicationViewModel.Candidate.HasQualifications;
            savedApplicationViewModel.Candidate.Qualifications = submittedApplicationViewModel.Candidate.Qualifications;
            savedApplicationViewModel.Candidate.HasWorkExperience = submittedApplicationViewModel.Candidate.HasWorkExperience;
            savedApplicationViewModel.Candidate.WorkExperience = submittedApplicationViewModel.Candidate.WorkExperience;
            savedApplicationViewModel.Candidate.EmployerQuestionAnswers = submittedApplicationViewModel.Candidate.EmployerQuestionAnswers;
            savedApplicationViewModel.ApplicationAction = submittedApplicationViewModel.ApplicationAction;

            return savedApplicationViewModel;
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
            var applicationModel = _mapper.Map<ApplicationDetail, ApplicationViewModel>(applicationDetails);

            if (applicationModel == null)
            {
                throw new CustomException("Application not found", ErrorCodes.ApplicationNotFoundError);
            }

            var patchedApplicationModel = PatchWithVacancyDetail(candidateId, vacancyId, applicationModel);

            return new WhatHappensNextViewModel
            {
                VacancyReference = patchedApplicationModel.VacancyDetail.FullVacancyReferenceId,
                VacancyTitle = patchedApplicationModel.VacancyDetail.Title
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
                    DateUpdated = each.DateUpdated
                });

            return new MyApplicationsViewModel(applications);
        }

        #region Helpers
        private ApplicationViewModel PatchWithVacancyDetail(Guid candidateId, int vacancyId, ApplicationViewModel applicationViewModel)
        {
            //todo: why have a patch method like this? should be done in mapper
            var vacancyDetailViewModel = _vacancyDetailProvider.GetVacancyDetailViewModel(candidateId, vacancyId);

            if (vacancyDetailViewModel == null)
            {
                return null;
            }

            if (vacancyDetailViewModel.HasError())
            {
                throw new CustomException( "VacancyDetail.Error", vacancyDetailViewModel.ViewModelMessage);
            }

            applicationViewModel.VacancyDetail = vacancyDetailViewModel;
            applicationViewModel.Candidate.EmployerQuestionAnswers.SupplementaryQuestion1 = vacancyDetailViewModel.SupplementaryQuestion1;
            applicationViewModel.Candidate.EmployerQuestionAnswers.SupplementaryQuestion2 = vacancyDetailViewModel.SupplementaryQuestion2;

            return applicationViewModel;
        }
        #endregion
    }
}
