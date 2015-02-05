﻿namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using Application.Interfaces.Logging;
    using Domain.Entities.Vacancies.Traineeships;
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Vacancies;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Mapping;
    using Constants.Pages;
    using ViewModels.VacancySearch;

    public class TraineeshipVacancyDetailProvider : ITraineeshipVacancyDetailProvider
    {
        private readonly ILogService _logger;
        private readonly IMapper _mapper;
        private readonly IVacancySearchService<TraineeshipSearchResponse, TraineeshipVacancyDetail, TraineeshipSearchParameters> _vacancySearchService;
        private readonly ICandidateService _candidateService;

        public TraineeshipVacancyDetailProvider(
            IMapper mapper,
            IVacancySearchService<TraineeshipSearchResponse, TraineeshipVacancyDetail, TraineeshipSearchParameters> vacancySearchService, 
            ICandidateService candidateService, ILogService logger)
        {
            _mapper = mapper;
            _vacancySearchService = vacancySearchService;
            _candidateService = candidateService;
            _logger = logger;
        }

        public VacancyDetailViewModel GetVacancyDetailViewModel(Guid? candidateId, int vacancyId)
        {
            _logger.Debug(
                "Calling TraineeshipVacancyDetailProvider to get the Vacancy detail View Model for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            try
            {
                var vacancyDetail = candidateId.HasValue ?
                    _candidateService.GetTraineeshipVacancyDetail(candidateId.Value, vacancyId) :
                    _vacancySearchService.GetVacancyDetails(vacancyId);

                if (vacancyDetail == null) return null;

                var vacancyDetailViewModel = _mapper.Map<TraineeshipVacancyDetail, VacancyDetailViewModel>(vacancyDetail);

                if (candidateId == null) return vacancyDetailViewModel;

                var traineeshipApplication = _candidateService.GetTraineeshipApplication(candidateId.Value, vacancyId);

                if (traineeshipApplication == null) return vacancyDetailViewModel;

                // If candidate has applied for vacancy, include the details in the view model.
                vacancyDetailViewModel.DateApplied = traineeshipApplication.DateApplied;

                return vacancyDetailViewModel;
            }
            catch (CustomException e)
            {
                var message =
                    string.Format("Get Traineeship Vacancy View Model failed for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);

                _logger.Error(message, e);

                return new VacancyDetailViewModel(TraineeshipVacancyDetailPageMessages.GetVacancyDetailFailed);
            }
            catch (Exception e)
            {
                var message =
                    string.Format("Get Traineeship Vacancy View Model failed for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);

                _logger.Error(message, e);
                throw;
            }
        }
    }
}