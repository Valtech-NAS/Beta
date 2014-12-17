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
    using ErrorCodes = SFA.Apprenticeships.Domain.Entities.Exceptions.ErrorCodes;

    public class TraineeshipApplicationProvider : ITraineeshipApplicationProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ICandidateService _candidateService;
        private readonly IMapper _mapper;
        private readonly IVacancyDetailProvider _vacancyDetailProvider;

        public TraineeshipApplicationProvider(IMapper mapper,
            ICandidateService candidateService,
            IVacancyDetailProvider vacancyDetailProvider)
        {
            _mapper = mapper;
            _candidateService = candidateService;
            _vacancyDetailProvider = vacancyDetailProvider;
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
                //if (e.Code == ErrorCodes.ApplicationInIncorrectStateError)
                //{
                //    Logger.Info(e.Message, e);
                //    return new ApprenticheshipApplicationViewModel(MyApplicationsPageMessages.ApplicationInIncorrectState,
                //        ApplicationViewModelStatus.ApplicationInIncorrectState);
                //}

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

        public TraineeshipApplicationViewModel SubmitApplication(Guid candidateId, int vacancyId)
        {
            Logger.Debug(
                "Calling TraineeeshipApplicationProvider to submit the traineeships application for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            var model = new TraineeshipApplicationViewModel();

            try
            {
                model = GetApplicationViewModel(candidateId, vacancyId);

                if (model.HasError())
                {
                    return model;
                }

                // _candidateService.SubmitApplication(candidateId, vacancyId);

                Logger.Debug("Traineeship application submitted for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);

                return model;
            }
            catch (CustomException e)
            {
                if (e.Code == ErrorCodes.ApplicationInIncorrectStateError)
                {
                    Logger.Info(e.Message, e);
                    return new TraineeshipApplicationViewModel(ApplicationViewModelStatus.ApplicationInIncorrectState)
                    {
                        Status = model.Status
                    };
                }

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
                // var applicationDetails = _candidateService.GetApplication(candidateId, vacancyId);
                var applicationDetails = GetDummyApplication();
                var model =
                    _mapper.Map<TraineeshipApplicationDetail, TraineeshipApplicationViewModel>(applicationDetails);
                var patchedModel = PatchWithVacancyDetail(candidateId, vacancyId, model);

                if (patchedModel.HasError())
                {
                    return new WhatHappensNextViewModel(patchedModel.ViewModelMessage);
                }

                return new WhatHappensNextViewModel
                {
                    VacancyReference = patchedModel.VacancyDetail.VacancyReference,
                    VacancyTitle = patchedModel.VacancyDetail.Title,
                    Status = patchedModel.Status
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

        public TraineeshipApplicationViewModel ArchiveApplication(Guid candidateId, int vacancyId)
        {
            throw new NotImplementedException();
        }

        private static TraineeshipApplicationDetail GetDummyApplication()
        {
            return new TraineeshipApplicationDetail();
        }

        private TraineeshipApplicationViewModel PatchWithVacancyDetail(Guid candidateId, int vacancyId,
            TraineeshipApplicationViewModel apprenticheshipApplicationViewModel)
        {
            // TODO: why have a patch method like this? should be done in mapper.
            var vacancyDetailViewModel = _vacancyDetailProvider.GetVacancyDetailViewModel(candidateId, vacancyId);

            if (vacancyDetailViewModel == null)
            {
                apprenticheshipApplicationViewModel.ViewModelMessage = MyApplicationsPageMessages.DraftExpired;
                apprenticheshipApplicationViewModel.Status = ApplicationStatuses.ExpiredOrWithdrawn;

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