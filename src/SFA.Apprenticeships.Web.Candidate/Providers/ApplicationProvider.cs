namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Application.Interfaces.Candidates;
    using Common.Providers;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Mapping;
    using ViewModels.Applications;
    using ViewModels.MyApplications;

    internal class ApplicationProvider : IApplicationProvider
    {
        private const string ApplicationContextName = "Application.Context";

        private readonly ICandidateService _candidateService;
        private readonly IMapper _mapper;
        private readonly IUserServiceProvider _userServiceProvider;
        private readonly IVacancyDetailProvider _vacancyDetailProvider;

        public ApplicationProvider(
            IVacancyDetailProvider vacancyDetailProvider,
            ICandidateService candidateService,
            IUserServiceProvider userServiceProvider,
            IMapper mapper)
        {
            _vacancyDetailProvider = vacancyDetailProvider;
            _candidateService = candidateService;
            _userServiceProvider = userServiceProvider;
            _mapper = mapper;
        }

        public ApplicationViewModel GetApplication(Guid applicationId)
        {
            var applicationEntityId = GetApplicationEntityIdFromContext(applicationId);
            var applicationDetail = _candidateService.GetApplication(applicationEntityId);

            var model = _mapper.Map<ApplicationDetail, ApplicationViewModel>(applicationDetail);
            model.ApplicationViewId = applicationId;

            return PatchWithVacancyDetail(
                model, applicationDetail.CandidateId, applicationDetail.Vacancy.Id);
        }

        public ApplicationViewModel GetApplicationViewModel(int vacancyId, Guid candidateId)
        {
            var applicationDetails = _candidateService.CreateApplication(candidateId, vacancyId);
            var viewModelId = PutApplicationIntoContext(applicationDetails.EntityId);
            var applicationViewModel = _mapper.Map<ApplicationDetail, ApplicationViewModel>(applicationDetails);
            applicationViewModel.ApplicationViewId = viewModelId;

            return PatchWithVacancyDetail(
                applicationViewModel, candidateId, vacancyId);
        }

        public void SaveApplication(ApplicationViewModel applicationViewModel)
        {
            var model = GetApplication(applicationViewModel.ApplicationViewId);

            model.Candidate.AboutYou = applicationViewModel.Candidate.AboutYou;
            model.Candidate.Education = applicationViewModel.Candidate.Education;
            //TODO uncomment after qualification and work experience is done
            //model.Candidate.Qualifications = applicationViewModel.Candidate.Qualifications;
            //model.Candidate.WorkExperience = applicationViewModel.Candidate.WorkExperience;
            model.Candidate.EmployerQuestionAnswers = applicationViewModel.Candidate.EmployerQuestionAnswers;
           
            var application = _mapper.Map<ApplicationViewModel, ApplicationDetail>(model);
            application.EntityId = GetApplicationEntityIdFromContext(applicationViewModel.ApplicationViewId);

            _candidateService.SaveApplication(application);
        }

        public void SubmitApplication(Guid applicationId)
        {
            var applicationEntityId = GetApplicationEntityIdFromContext(applicationId);
            _candidateService.SubmitApplication(applicationEntityId);
        }

        public WhatHappensNextViewModel GetSubmittedApplicationVacancySummary(Guid applicationId)
        {
            var model = GetApplication(applicationId);

            if (model == null)
            {
                throw  new CustomException("Application not found", ErrorCodes.ApplicationNotFoundError);
            }

            return new WhatHappensNextViewModel
            {
                VacancyReference = model.VacancyDetail.FullVacancyReferenceId,
                VacancyTitle = model.VacancyDetail.Title
            };
        }

        public MyApplicationsViewModel GetMyApplications(Guid candidateId)
        {
            var applicationSummaries = _candidateService.GetApplications(candidateId);

            var applications = applicationSummaries
                .OrderByDescending(each => each.DateUpdated)
                .Select(each => new MyApplicationViewModel
                {
                    VacancyId = each.LegacyVacancyId,
                    Title = each.Title,
                    UnsuccessfulReason = null, // TODO: US154: AG: does not exist in ApplicationSummary.
                    WithdrawnOrDeclinedReason = null, // TODO: US154: AG: does not exist in ApplicationSummary.
                    ApplicationStatus = each.Status,
                    DateApplied = each.DateApplied

                });

            return new MyApplicationsViewModel(applications);
        }

        #region Helpers

        private Guid GetApplicationEntityIdFromContext(Guid applicationViewId)
        {
            var httpContext = new HttpContextWrapper(HttpContext.Current);
            var applicationContext = _userServiceProvider.GetEntityContextCookie(httpContext, ApplicationContextName);

            if (applicationContext != null && applicationContext.ViewModelId == applicationViewId)
            {
                return applicationContext.EntityId;
            }

            throw new CustomException("ApplicationViewId is not in context", ErrorCodes.ApplicationViewIdNotFound);
        }

        private Guid PutApplicationIntoContext(Guid applicationEntityId)
        {
            var viewModelId = Guid.NewGuid();
            var httpContext = new HttpContextWrapper(HttpContext.Current);
            _userServiceProvider
                .SetEntityContextCookie(httpContext, applicationEntityId, viewModelId, ApplicationContextName);
            return viewModelId;
        }

        private ApplicationViewModel PatchWithVacancyDetail(
            ApplicationViewModel applicationViewModel, Guid candidateId, int vacancyId)
        {
            var vacancyDetailViewModel = _vacancyDetailProvider.GetVacancyDetailViewModel(candidateId, vacancyId);

            if (vacancyDetailViewModel == null)
            {
                return null;
            }

            applicationViewModel.VacancyDetail = vacancyDetailViewModel;
            applicationViewModel.Candidate.EmployerQuestionAnswers.SupplementaryQuestion1 =
                vacancyDetailViewModel.SupplementaryQuestion1;
            applicationViewModel.Candidate.EmployerQuestionAnswers.SupplementaryQuestion2 =
                vacancyDetailViewModel.SupplementaryQuestion2;

            return applicationViewModel;
        }

        #endregion
    }
}