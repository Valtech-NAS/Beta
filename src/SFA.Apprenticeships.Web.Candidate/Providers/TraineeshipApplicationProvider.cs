namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using NLog;
    using SFA.Apprenticeships.Application.Interfaces.Candidates;
    using SFA.Apprenticeships.Domain.Entities.Applications;
    using SFA.Apprenticeships.Domain.Entities.Exceptions;
    using SFA.Apprenticeships.Domain.Interfaces.Mapping;
    using SFA.Apprenticeships.Web.Candidate.Constants.Pages;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Applications;
    using SFA.Apprenticeships.Web.Common.Models.Application;

    public class TraineeshipApplicationProvider : ITraineeshipApplicationProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ICandidateService _candidateService;
        private readonly IMapper _mapper;
        private readonly ITraineeshipVacancyDetailProvider _traineeshipVacancyDetailProvider;

        public TraineeshipApplicationProvider(IMapper mapper,
            ICandidateService candidateService,
            ITraineeshipVacancyDetailProvider traineeshipVacancyDetailProvider)
        {
            _mapper = mapper;
            _candidateService = candidateService;
            _traineeshipVacancyDetailProvider = traineeshipVacancyDetailProvider;
        }

        public TraineeshipApplicationViewModel GetApplicationViewModel(Guid candidateId, int vacancyId)
        {
            Logger.Debug(
                "Calling ApprenticeshipApplicationProvider to get the Application View Model for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            try
            {
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
                Logger.Error(message, e);
                return new TraineeshipApplicationViewModel("Unhandled error", ApplicationViewModelStatus.Error);
            }
            catch (Exception e)
            {
                var message = string.Format("Get Application View Model failed for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);

                Logger.Error(message, e);

                return new TraineeshipApplicationViewModel(MyApplicationsPageMessages.CreateOrRetrieveApplicationFailed,
                    ApplicationViewModelStatus.Error);
            }
        }

        public TraineeshipApplicationViewModel SubmitApplication(Guid candidateId, int vacancyId,
            TraineeshipApplicationViewModel traineeshipApplicationViewModel)
        {
            Logger.Debug(
                "Calling TraineeeshipApplicationProvider to submit the traineeships application for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            try
            {
                var model = GetApplicationViewModel(candidateId, vacancyId);

                var traineeshipApplicationDetails =
                    _mapper.Map<TraineeshipApplicationViewModel, TraineeshipApplicationDetail>(
                        traineeshipApplicationViewModel);

                _candidateService.SubmitTraineeshipApplication(candidateId, vacancyId, traineeshipApplicationDetails);

                Logger.Debug("Traineeship application submitted for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);

                return model;
            }
            catch (CustomException e)
            {
                var message =
                    string.Format(
                        "Unhandled custom exception while submitting the traineeship application for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);
                Logger.Error(message, e);

                return FailedApplicationViewModel(vacancyId, candidateId, "Submission of traineeship application",
                    ApplicationPageMessages.SubmitApplicationFailed, e);
            }
            catch (Exception e)
            {
                var message =
                    string.Format(
                        "Submit traineeship application failed for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);
                Logger.Error(message, e);

                return FailedApplicationViewModel(vacancyId, candidateId, "Submission of traineeship application",
                    ApplicationPageMessages.SubmitApplicationFailed, e);
            }
        }

        public WhatHappensNextViewModel GetWhatHappensNextViewModel(Guid candidateId, int vacancyId)
        {
            Logger.Debug(
                "Calling ApprenticeshipApplicationProvider to get the What Happens Next data for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            try
            {
                var patchedModel = GetApplicationViewModel(candidateId, vacancyId);

                if (patchedModel.HasError())
                {
                    return new WhatHappensNextViewModel(patchedModel.ViewModelMessage);
                }

                return new WhatHappensNextViewModel
                {
                    VacancyReference = patchedModel.VacancyDetail.VacancyReference,
                    VacancyTitle = patchedModel.VacancyDetail.Title
                };
            }
            catch (Exception e)
            {
                var message =
                    string.Format("Get What Happens Next View Model failed for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);

                Logger.Error(message, e);

                return new WhatHappensNextViewModel(
                    MyApplicationsPageMessages.CreateOrRetrieveApplicationFailed);
            }
        }

        public TraineeshipApplicationViewModel PatchApplicationViewModel(Guid candidateId,
            TraineeshipApplicationViewModel savedModel, TraineeshipApplicationViewModel submittedModel)
        {
            Logger.Debug(
                "Calling ApprenticeshipApplicationProvider to patch the Application View Model for candidate ID: {0}.",
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
                Logger.Error(message, e);
                throw;
            }
        }

        public TraineeshipApplicationViewModel ArchiveApplication(Guid candidateId, int vacancyId)
        {
            throw new NotImplementedException();
        }

        private TraineeshipApplicationViewModel PatchWithVacancyDetail(Guid candidateId, int vacancyId,
            TraineeshipApplicationViewModel apprenticheshipApplicationViewModel)
        {
            // TODO: why have a patch method like this? should be done in mapper.
            var vacancyDetailViewModel = _traineeshipVacancyDetailProvider.GetVacancyDetailViewModel(candidateId,
                vacancyId);

            if (vacancyDetailViewModel == null)
            {
                apprenticheshipApplicationViewModel.ViewModelMessage = MyApplicationsPageMessages.DraftExpired;

                return apprenticheshipApplicationViewModel;
            }

            if (vacancyDetailViewModel.HasError())
            {
                apprenticheshipApplicationViewModel.ViewModelMessage = vacancyDetailViewModel.ViewModelMessage;

                return apprenticheshipApplicationViewModel;
            }

            apprenticheshipApplicationViewModel.VacancyDetail = vacancyDetailViewModel;
            apprenticheshipApplicationViewModel.Candidate.EmployerQuestionAnswers.SupplementaryQuestion1 =
                vacancyDetailViewModel.SupplementaryQuestion1;
            apprenticheshipApplicationViewModel.Candidate.EmployerQuestionAnswers.SupplementaryQuestion2 =
                vacancyDetailViewModel.SupplementaryQuestion2;

            return apprenticheshipApplicationViewModel;
        }

        private static TraineeshipApplicationViewModel FailedApplicationViewModel(int vacancyId, Guid candidateId,
            string failure,
            string failMessage, Exception e)
        {
            var message = string.Format("{0} {1} failed for user {2}", failure, vacancyId, candidateId);
            Logger.Error(message, e);
            return new TraineeshipApplicationViewModel(failMessage, ApplicationViewModelStatus.Error);
        }
    }
}