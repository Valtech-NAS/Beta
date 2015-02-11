namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Linq;
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Logging;
    using Application.Interfaces.Vacancies;
    using Constants.Pages;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Mapping;
    using ViewModels.VacancySearch;

    public class ApprenticeshipVacancyDetailProvider : IApprenticeshipVacancyDetailProvider
    {
        private readonly ILogService _logger;
        private readonly IMapper _mapper;
        private readonly ICandidateService _candidateService;
        private readonly IVacancySearchService<ApprenticeshipSearchResponse, ApprenticeshipVacancyDetail, ApprenticeshipSearchParameters> _vacancySearchService;

        public ApprenticeshipVacancyDetailProvider(
            IVacancySearchService<ApprenticeshipSearchResponse,
            ApprenticeshipVacancyDetail, ApprenticeshipSearchParameters> vacancySearchService,
            ICandidateService candidateService,
            IMapper mapper, ILogService logger)
        {
            _vacancySearchService = vacancySearchService;
            _candidateService = candidateService;
            _mapper = mapper;
            _logger = logger;
        }

        public VacancyDetailViewModel GetVacancyDetailViewModel(Guid? candidateId, int vacancyId)
        {
            _logger.Debug(
                "Calling ApprenticeshipVacancyDetailProvider to get the Vacancy detail View Model for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            try
            {
                var vacancyDetail = candidateId.HasValue ?
                    _candidateService.GetApprenticeshipVacancyDetail(candidateId.Value, vacancyId) :
                    _vacancySearchService.GetVacancyDetails(vacancyId);

                if (vacancyDetail == null) return null;

                var vacancyDetailViewModel = _mapper.Map<ApprenticeshipVacancyDetail, VacancyDetailViewModel>(vacancyDetail);

                if (candidateId == null) return vacancyDetailViewModel;


                ApprenticeshipApplicationSummary applicationDetails = null;
                try
                {
                    var apprenticeshipApplicationSummaries = _candidateService.GetApprenticeshipApplications(candidateId.Value);
                    if (apprenticeshipApplicationSummaries != null && apprenticeshipApplicationSummaries.Count > 0)
                    {
                        applicationDetails = apprenticeshipApplicationSummaries.SingleOrDefault(a => a.LegacyVacancyId == vacancyId);
                    }
                }
                catch (Exception ex)
                {
                    var message = string.Format("Finding application failed for Candidate Id: {0}, Vacancy Id: {1}", candidateId, vacancyId);
                    _logger.Warn(message, ex);
                }

                if (applicationDetails == null) return vacancyDetailViewModel;

                // If candidate has applied for vacancy, include the details in the view model.
                vacancyDetailViewModel.CandidateApplicationStatus = applicationDetails.Status;
                vacancyDetailViewModel.DateApplied = applicationDetails.DateApplied;

                return vacancyDetailViewModel;
            }
            catch (CustomException e)
            {
                var message = string.Format("Get Apprenticeship Vacancy View Model failed for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);

                _logger.Error(message, e);

                return new VacancyDetailViewModel(ApprenticeshipVacancyDetailPageMessages.GetVacancyDetailFailed);
            }
            catch (Exception e)
            {
                var message = string.Format("Get Apprenticeship Vacancy View Model failed for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);

                _logger.Error(message, e);
                throw;
            }
        }
   }
}
