namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Linq;
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Vacancies;
    using Constants.Pages;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using NLog;
    using ViewModels.VacancySearch;

    public class VacancyDetailProvider : IVacancyDetailProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IApplicationWriteRepository _applicationWriteRepository;
        private readonly ICandidateService _candidateService;
        private readonly IMapper _mapper;
        private readonly IVacancyDataService _vacancyDataService;

        public VacancyDetailProvider(
            IVacancyDataService vacancyDataService,
            ICandidateService candidateService,
            IApplicationWriteRepository applicationWriteRepository,
            IMapper mapper)
        {
            _applicationWriteRepository = applicationWriteRepository;
            _vacancyDataService = vacancyDataService;
            _candidateService = candidateService;
            _mapper = mapper;
        }

        public VacancyDetailViewModel GetVacancyDetailViewModel(Guid? candidateId, int vacancyId)
        {
            Logger.Debug(
                "Calling VacancyDetailProvider to get the Vacancy detail View Model for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            try
            {
                var vacancyDetail = _vacancyDataService.GetVacancyDetails(vacancyId);

                if (vacancyDetail == null)
                {
                    if (candidateId != null)
                    {
                        // Vacancy is being viewed by a signed-in candidate, update application status.
                        _applicationWriteRepository.ExpireOrWithdrawForCandidate(candidateId.Value, vacancyId);
                    }

                    return null;
                }

                var vacancyDetailViewModel =
                    _mapper.Map<VacancyDetail, VacancyDetailViewModel>(vacancyDetail);

                if (candidateId == null) { return vacancyDetailViewModel; }

                // If candidate has applied for vacancy, include the details in the view model.
                var applicationDetails = GetCandidateApplication(candidateId.Value, vacancyId);

                if (applicationDetails == null) { return vacancyDetailViewModel; }

                vacancyDetailViewModel.CandidateApplicationStatus = applicationDetails.Status;
                vacancyDetailViewModel.DateApplied = applicationDetails.DateApplied;

                return vacancyDetailViewModel;
            }
            catch (CustomException e)
            {
                var message = string.Format("Get Vacancy View Model failed for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);

                Logger.Error(message, e);

                return new VacancyDetailViewModel(VacancyDetailPageMessages.GetVacancyDetailFailed);
            }
            catch (Exception e)
            {
                var message = string.Format("Get Vacancy View Model failed for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);

                Logger.Error(message, e);
                throw;
            }
        }

        private ApplicationSummary GetCandidateApplication(Guid candidateId, int vacancyId)
        {
            return _candidateService
                .GetApplications(candidateId)
                .SingleOrDefault(a => a.LegacyVacancyId == vacancyId);
        }
    }
}