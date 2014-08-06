namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Linq;
    using Application.Interfaces.Vacancies;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Mapping;
    using ViewModels.VacancySearch;
    using Application.Interfaces.Candidates;

    public class VacancyDetailProvider : IVacancyDetailProvider
    {
        private readonly IVacancyDataService _vacancyDataService;
        private readonly ICandidateService _candidateService;
        private readonly IMapper _mapper;

        public VacancyDetailProvider(
            IVacancyDataService vacancyDataService,
            ICandidateService candidateService,
            IMapper mapper)
        {
            _vacancyDataService = vacancyDataService;
            _candidateService = candidateService;
            _mapper = mapper;
        }

        public VacancyDetailViewModel GetVacancyDetailViewModel(Guid? candidateId, int vacancyId)
        {
            var vacancyDetail = _vacancyDataService.GetVacancyDetails(vacancyId);

            if (vacancyDetail == null)
            {
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
