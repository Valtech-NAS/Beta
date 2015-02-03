namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using Application.Interfaces.Logging;
    using Domain.Entities.Vacancies;
    using Application.Interfaces.Candidates;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Mapping;
    using Constants.Pages;
    using ViewModels.Applications;
    using Common.Models.Application;

    public class TraineeshipApplicationProvider : ITraineeshipApplicationProvider
    {
        private readonly ILogService _logger;
        private readonly ICandidateService _candidateService;
        private readonly IMapper _mapper;
        private readonly ITraineeshipVacancyDetailProvider _traineeshipVacancyDetailProvider;

        public TraineeshipApplicationProvider(IMapper mapper,
            ICandidateService candidateService,
            ITraineeshipVacancyDetailProvider traineeshipVacancyDetailProvider, ILogService logger)
        {
            _mapper = mapper;
            _candidateService = candidateService;
            _traineeshipVacancyDetailProvider = traineeshipVacancyDetailProvider;
            _logger = logger;
        }

        public TraineeshipApplicationViewModel GetApplicationViewModel(Guid candidateId, int vacancyId)
        {
            _logger.Debug(
                "Calling TraineeshipApplicationProvider to get the Application View Model for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            try
            {
                // Check if we've already applied for the vacancy
                if (_candidateService.GetTraineeshipApplication(candidateId, vacancyId) != null)
                {
                    return new TraineeshipApplicationViewModel("Traineeship already applied", ApplicationViewModelStatus.ApplicationInIncorrectState);
                }

                var applicationDetails = _candidateService.CreateTraineeshipApplication(candidateId, vacancyId);
                var applicationViewModel =
                    _mapper.Map<TraineeshipApplicationDetail, TraineeshipApplicationViewModel>(applicationDetails);
                
                return PatchWithVacancyDetail(candidateId, vacancyId, applicationViewModel);
            }
            catch (CustomException e)
            {
                var message =
                    string.Format(
                        "Unhandled custom exception while getting the Application View Model for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);
                _logger.Error(message, e);
                return new TraineeshipApplicationViewModel("Unhandled error", ApplicationViewModelStatus.Error);
            }
            catch (Exception e)
            {
                var message = string.Format("Get Application View Model failed for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);

                _logger.Error(message, e);

                return new TraineeshipApplicationViewModel(MyApplicationsPageMessages.CreateOrRetrieveApplicationFailed,
                    ApplicationViewModelStatus.Error);
            }
        }

        public TraineeshipApplicationViewModel SubmitApplication(Guid candidateId, int vacancyId,
            TraineeshipApplicationViewModel traineeshipApplicationViewModel)
        {
            _logger.Debug(
                "Calling TraineeeshipApplicationProvider to submit the traineeships application for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            try
            {
                var model = GetApplicationViewModel(candidateId, vacancyId);

                //TODO: check for error (traineeship already submitted)?

                var traineeshipApplicationDetails =
                    _mapper.Map<TraineeshipApplicationViewModel, TraineeshipApplicationDetail>(
                        traineeshipApplicationViewModel);

                _candidateService.SubmitTraineeshipApplication(candidateId, vacancyId, traineeshipApplicationDetails);

                _logger.Debug("Traineeship application submitted for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);

                return model;
            }
            catch (CustomException e)
            {
                var message =
                    string.Format(
                        "Unhandled custom exception while submitting the traineeship application for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);
                _logger.Error(message, e);

                return FailedApplicationViewModel(vacancyId, candidateId, "Submission of traineeship application",
                    ApplicationPageMessages.SubmitApplicationFailed, e);
            }
            catch (Exception e)
            {
                var message =
                    string.Format(
                        "Submit traineeship application failed for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);
                _logger.Error(message, e);

                return FailedApplicationViewModel(vacancyId, candidateId, "Submission of traineeship application",
                    ApplicationPageMessages.SubmitApplicationFailed, e);
            }
        }

        public WhatHappensNextViewModel GetWhatHappensNextViewModel(Guid candidateId, int vacancyId)
        {
            _logger.Debug(
                "Calling TraineeshipApplicationProvider to get the What Happens Next data for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            try
            {
                var vacancyDetailViewModel = _traineeshipVacancyDetailProvider.GetVacancyDetailViewModel(candidateId, vacancyId);

                if (vacancyDetailViewModel.HasError())
                {
                    return new WhatHappensNextViewModel(vacancyDetailViewModel.ViewModelMessage);
                }

                var candidate = _candidateService.GetCandidate(candidateId);

                return new WhatHappensNextViewModel
                {
                    VacancyReference = vacancyDetailViewModel.VacancyReference,
                    VacancyTitle = vacancyDetailViewModel.Title,
                    SentEmail = candidate.CommunicationPreferences.AllowEmail,
                    ProviderContactInfo = vacancyDetailViewModel.Contact
                };
            }
            catch (Exception e)
            {
                var message =
                    string.Format("Get What Happens Next View Model failed for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);

                _logger.Error(message, e);

                return new WhatHappensNextViewModel(MyApplicationsPageMessages.CreateOrRetrieveApplicationFailed);
            }
        }

        public TraineeshipApplicationViewModel PatchApplicationViewModel(Guid candidateId,
            TraineeshipApplicationViewModel savedModel, TraineeshipApplicationViewModel submittedModel)
        {
            _logger.Debug(
                "Calling TraineeshipApplicationProvider to patch the Application View Model for candidate ID: {0}.",
                candidateId);

            try
            {
                savedModel.Candidate.HasQualifications = submittedModel.Candidate.HasQualifications;
                savedModel.Candidate.Qualifications = submittedModel.Candidate.Qualifications;
                savedModel.Candidate.HasWorkExperience = submittedModel.Candidate.HasWorkExperience;
                savedModel.Candidate.WorkExperience = submittedModel.Candidate.WorkExperience;
                savedModel.Candidate.EmployerQuestionAnswers = submittedModel.Candidate.EmployerQuestionAnswers;

                return savedModel;
            }
            catch (Exception e)
            {
                var message =
                    string.Format(
                        "Patch traineeship application View Model failed for user {0}.", candidateId);
                _logger.Error(message, e);
                throw;
            }
        }

        private TraineeshipApplicationViewModel PatchWithVacancyDetail(Guid candidateId, int vacancyId,
            TraineeshipApplicationViewModel traineeshipApplicationViewModel)
        {
            // TODO: why have a patch method like this? should be done in mapper.
            var vacancyDetailViewModel = _traineeshipVacancyDetailProvider.GetVacancyDetailViewModel(candidateId, vacancyId);

            if (vacancyDetailViewModel == null || vacancyDetailViewModel.VacancyStatus == VacancyStatuses.Unavailable)
            {
                traineeshipApplicationViewModel.ViewModelMessage = MyApplicationsPageMessages.TraineeshipNoLongerAvailable;
                return traineeshipApplicationViewModel;
            }

            if (vacancyDetailViewModel.HasError())
            {
                traineeshipApplicationViewModel.ViewModelMessage = vacancyDetailViewModel.ViewModelMessage;
                return traineeshipApplicationViewModel;
            }

            traineeshipApplicationViewModel.VacancyDetail = vacancyDetailViewModel;
            traineeshipApplicationViewModel.Candidate.EmployerQuestionAnswers.SupplementaryQuestion1 =
                vacancyDetailViewModel.SupplementaryQuestion1;
            traineeshipApplicationViewModel.Candidate.EmployerQuestionAnswers.SupplementaryQuestion2 =
                vacancyDetailViewModel.SupplementaryQuestion2;

            return traineeshipApplicationViewModel;
        }

        private TraineeshipApplicationViewModel FailedApplicationViewModel(int vacancyId, Guid candidateId,
            string failure,
            string failMessage, Exception e)
        {
            var message = string.Format("{0} {1} failed for user {2}", failure, vacancyId, candidateId);
            _logger.Error(message, e);
            return new TraineeshipApplicationViewModel(failMessage, ApplicationViewModelStatus.Error);
        }
    }
}