namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Linq;
    using Application.Interfaces.Candidates;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Mapping;
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
                if (ex.Code == ErrorCodes.VacancyExpired) return null;

                throw;
            }
        }

        public ApplicationViewModel UpdateApplicationViewModel(Guid candidateId, ApplicationViewModel submittedApplicationViewModel)
        {
            var model = GetApplicationViewModel(candidateId, submittedApplicationViewModel.VacancyId);

            if (!submittedApplicationViewModel.Candidate.AboutYou.RequiresSupportForInterview)
            {
                submittedApplicationViewModel.Candidate.AboutYou.AnythingWeCanDoToSupportYourInterview = string.Empty;
            }

            model.Candidate.AboutYou = submittedApplicationViewModel.Candidate.AboutYou;
            model.Candidate.Education = submittedApplicationViewModel.Candidate.Education;
            model.Candidate.HasQualifications = submittedApplicationViewModel.Candidate.HasQualifications;
            model.Candidate.Qualifications = submittedApplicationViewModel.Candidate.Qualifications;
            model.Candidate.HasWorkExperience = submittedApplicationViewModel.Candidate.HasWorkExperience;
            model.Candidate.WorkExperience = submittedApplicationViewModel.Candidate.WorkExperience;
            model.Candidate.EmployerQuestionAnswers = submittedApplicationViewModel.Candidate.EmployerQuestionAnswers;

            return model;
        }

        public void SaveApplication(Guid candidateId, int vacancyId, ApplicationViewModel applicationViewModel)
        {
            var model = UpdateApplicationViewModel(candidateId, applicationViewModel);

            var application = _mapper.Map<ApplicationViewModel, ApplicationDetail>(model);

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
                    WithdrawnOrDeclinedReason = each.WithdrawnOrDeclinedReason,
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

            applicationViewModel.VacancyDetail = vacancyDetailViewModel;
            applicationViewModel.Candidate.EmployerQuestionAnswers.SupplementaryQuestion1 = vacancyDetailViewModel.SupplementaryQuestion1;
            applicationViewModel.Candidate.EmployerQuestionAnswers.SupplementaryQuestion2 = vacancyDetailViewModel.SupplementaryQuestion2;

            return applicationViewModel;
        }
        #endregion
    }
}
