namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Linq;
    using Application.Interfaces.Vacancies;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using ViewModels.VacancySearch;
    using Application.Interfaces.Candidates;
    using Domain.Entities.Exceptions;
    using Constants.Pages;
    using ErrorCodes = Domain.Entities.Exceptions.ErrorCodes;

    public class VacancyDetailProvider : IVacancyDetailProvider
    {
        private readonly IVacancyDataService _vacancyDataService;
        private readonly ICandidateService _candidateService;
        private readonly IMapper _mapper;
        private readonly IApplicationReadRepository _applicationReadRepository;
        private readonly IApplicationWriteRepository _applicationWriteRepository;

        public VacancyDetailProvider(
            IVacancyDataService vacancyDataService,
            ICandidateService candidateService,
            IApplicationReadRepository applicationReadRepository,
            IApplicationWriteRepository applicationWriteRepository,
            IMapper mapper)
        {
            _applicationWriteRepository = applicationWriteRepository;
            _applicationReadRepository = applicationReadRepository;
            _vacancyDataService = vacancyDataService;
            _candidateService = candidateService;
            _mapper = mapper;
        }

        public VacancyDetailViewModel GetVacancyDetailViewModel(Guid? candidateId, int vacancyId)
        {
            try
            {
                var vacancyDetail = _vacancyDataService.GetVacancyDetails(vacancyId);

                if (IsVacancyExpiredOrWithdrawn(vacancyDetail))
                {
                    // Vacancy has expired, closing date is before today.
                    ExpireVacancy(candidateId, vacancyId);
                    return null;
                }

                var vacancyDetailViewModel = _mapper.Map<VacancyDetail, VacancyDetailViewModel>(vacancyDetail);

                // If candidate has applied for vacancy, include the details in the view model.
                var applicationDetails = GetCandidateApplication(candidateId, vacancyId);

                if (applicationDetails != null)
                {
                    vacancyDetailViewModel.CandidateApplicationStatus = applicationDetails.Status;
                    vacancyDetailViewModel.DateApplied = applicationDetails.DateApplied;
                }

                return vacancyDetailViewModel;
            }
            catch (CustomException)
            {
                return new VacancyDetailViewModel(VacancyDetailPageMessages.GetVacancyDetailFailed);
            }
        }

        private void ExpireVacancy(Guid? candidateId, int vacancyId)
        {
            if (candidateId == null)
            {
                // Vacancy is not being viewed by a signed-in candidate.
                return;
            }

            // Vacancy is not being viewed by a signed-in candidate.
            var applicationDetail = _applicationReadRepository.GetForCandidate(
                candidateId.Value, applicationdDetail => applicationdDetail.Vacancy.Id == vacancyId);

            applicationDetail.Status = ApplicationStatuses.ExpiredOrWithdrawn;
            _applicationWriteRepository.Save(applicationDetail);
        }

        private static bool IsVacancyExpiredOrWithdrawn(VacancyDetail vacancyDetail)
        {
            // TODO: AG: we have this logic in a few places (look for "< DateTime.Today"). This should be factored out into a strategy or service?
            return vacancyDetail == null || vacancyDetail.ClosingDate < DateTime.Today.ToUniversalTime();
        }

        private ApplicationSummary GetCandidateApplication(Guid? candidateId, int vacancyId)
        {
            if (candidateId == null)
            {
                // Vacancy is not being viewed by a signed-in candidate.
                return null;
            }

            return _candidateService
                .GetApplications(candidateId.Value)
                .SingleOrDefault(a => a.LegacyVacancyId == vacancyId);
        }
    }
}
