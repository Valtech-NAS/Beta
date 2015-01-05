namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Linq;
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Vacancies;
    using Constants.Pages;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using NLog;
    using ViewModels.VacancySearch;

    public class ApprenticeshipVacancyDetailProvider : IApprenticeshipVacancyDetailProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IApprenticeshipApplicationWriteRepository _apprenticeshipApplicationWriteRepository; //todo: MG: remove this!!
        private readonly ICandidateService _candidateService;
        private readonly IMapper _mapper;
        private readonly IVacancySearchService<ApprenticeshipSummaryResponse, ApprenticeshipVacancyDetail> _vacancySearchService;

        public ApprenticeshipVacancyDetailProvider(
            IVacancySearchService<ApprenticeshipSummaryResponse, ApprenticeshipVacancyDetail> vacancySearchService,
            ICandidateService candidateService,
            IApprenticeshipApplicationWriteRepository apprenticeshipApplicationWriteRepository,
            IMapper mapper)
        {
            _apprenticeshipApplicationWriteRepository = apprenticeshipApplicationWriteRepository;
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

                if (vacancyDetail == null)
                {
                    if (candidateId != null)
                    {
                        // Vacancy is being viewed by a signed-in candidate, update application status.
                        _apprenticeshipApplicationWriteRepository.ExpireOrWithdrawForCandidate(candidateId.Value, vacancyId);
                    }

                    return null;
                }

                var vacancyDetailViewModel =
                    _mapper.Map<ApprenticeshipVacancyDetail, VacancyDetailViewModel>(vacancyDetail);

                if (candidateId == null) { return vacancyDetailViewModel; }

                // If candidate has applied for vacancy, include the details in the view model.
                var applicationDetails = _candidateService
                    .GetApprenticeshipApplications(candidateId.Value)
                    .SingleOrDefault(a => a.LegacyVacancyId == vacancyId);

                if (applicationDetails == null) { return vacancyDetailViewModel; }

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
