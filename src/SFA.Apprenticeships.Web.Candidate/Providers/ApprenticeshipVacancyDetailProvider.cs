namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Linq;
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancies;
    using Constants.Pages;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Mapping;
    using NLog;
    using ViewModels.VacancySearch;

    public class ApprenticeshipVacancyDetailProvider : IApprenticeshipVacancyDetailProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ICandidateService _candidateService;
        private readonly IMapper _mapper;
        private readonly IVacancySearchService<ApprenticeshipSummaryResponse, ApprenticeshipVacancyDetail, ApprenticeshipSearchParameters> _vacancySearchService;

        public ApprenticeshipVacancyDetailProvider(
            IVacancySearchService<ApprenticeshipSummaryResponse,
            ApprenticeshipVacancyDetail, ApprenticeshipSearchParameters> vacancySearchService,
            ICandidateService candidateService,
            IMapper mapper)
        {
            _vacancySearchService = vacancySearchService;
            _candidateService = candidateService;
            _mapper = mapper;
        }

        public VacancyDetailViewModel GetVacancyDetailViewModel(Guid? candidateId, int vacancyId)
        {
            Logger.Debug(
                "Calling ApprenticeshipVacancyDetailProvider to get the Vacancy detail View Model for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            try
            {
                var vacancyDetail = _vacancySearchService.GetVacancyDetails(vacancyId);

                if (vacancyDetail == null) return null;

                var vacancyDetailViewModel = _mapper.Map<ApprenticeshipVacancyDetail, VacancyDetailViewModel>(vacancyDetail);

                if (candidateId == null) return vacancyDetailViewModel;

                var applicationDetails = _candidateService
                    .GetApprenticeshipApplications(candidateId.Value)
                    .SingleOrDefault(a => a.LegacyVacancyId == vacancyId);

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

                Logger.Error(message, e);

                return new VacancyDetailViewModel(ApprenticeshipVacancyDetailPageMessages.GetVacancyDetailFailed);
            }
            catch (Exception e)
            {
                var message = string.Format("Get Apprenticeship Vacancy View Model failed for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);

                Logger.Error(message, e);
                throw;
            }
        }
   }
}
