﻿namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Linq;
    using Application.Interfaces.Vacancies;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using NLog;
    using ViewModels.VacancySearch;
    using Application.Interfaces.Candidates;
    using Domain.Entities.Exceptions;
    using Constants.Pages;

    public class VacancyDetailProvider : IVacancyDetailProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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

                if (vacancyDetail == null)
                {
                    if (candidateId != null)
                    {
                        // Vacancy is being viewed by a signed-in candidate, update application status.
                        _applicationWriteRepository.ExpireOrWithdrawForCandidate(candidateId.Value, vacancyId);
                    }

                    return null;
                }

                var vacancyDetailViewModel = _mapper.Map<VacancyDetail, VacancyDetailViewModel>(vacancyDetail);

                if (candidateId != null)
                {
                    // If candidate has applied for vacancy, include the details in the view model.
                    var applicationDetails = GetCandidateApplication(candidateId.Value, vacancyId);

                    if (applicationDetails != null)
                    {
                        vacancyDetailViewModel.CandidateApplicationStatus = applicationDetails.Status;
                        vacancyDetailViewModel.DateApplied = applicationDetails.DateApplied;
                    }
                }

                return vacancyDetailViewModel;
            }
            catch (CustomException e)
            {
                var message = string.Format("Get Vacancy View Model failed for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);

                Logger.ErrorException(message, e);

                return new VacancyDetailViewModel(VacancyDetailPageMessages.GetVacancyDetailFailed);
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
