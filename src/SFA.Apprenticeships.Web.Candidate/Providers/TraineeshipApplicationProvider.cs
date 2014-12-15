namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using NLog;
    using SFA.Apprenticeships.Domain.Entities.Applications;
    using SFA.Apprenticeships.Domain.Entities.Exceptions;
    using SFA.Apprenticeships.Domain.Entities.Locations;
    using SFA.Apprenticeships.Domain.Entities.Users;
    using SFA.Apprenticeships.Domain.Entities.Vacancies.Traineeships;
    using SFA.Apprenticeships.Domain.Interfaces.Mapping;
    using SFA.Apprenticeships.Web.Candidate.Constants.Pages;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Applications;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch;
    using SFA.Apprenticeships.Web.Common.Models.Application;

    public class TraineeshipApplicationProvider : ITraineeshipApplicationProvider
    {
        private readonly IMapper _mapper;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public TraineeshipApplicationProvider(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TraineeshipApplicationViewModel GetApplicationViewModel(Guid candidateId, int vacancyId)
        {
            Logger.Debug("Calling ApprenticeshipApplicationProvider to get the Application View Model for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            try
            {
                // var applicationDetails = _candidateService.CreateApplication(candidateId, vacancyId);
                var applicationDetails = CreateDummyApplicationDetail(vacancyId);
                var applicationViewModel = _mapper.Map<TraineeshipApplicationDetail, TraineeshipApplicationViewModel>(applicationDetails);

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

        private TraineeshipApplicationDetail CreateDummyApplicationDetail(int vacancyId)
        {
            return new TraineeshipApplicationDetail
            {
                DateCreated = DateTime.Now.AddDays(-15),
                Status = ApplicationStatuses.Draft,
                CandidateDetails = new RegistrationDetails
                {
                    Address = new Address
                    {
                        AddressLine1 = "Cheylesmore House",
                        AddressLine2 = "5 Quinton Road"
                    },
                    DateOfBirth = DateTime.Now.AddYears(-20),
                    EmailAddress = "test_traineeships@gmail.com",
                    FirstName = "John",
                    LastName = "Doe",
                    PhoneNumber = "0123456789"
                },
                Vacancy = new TraineeshipSummary
                {
                    Id = vacancyId
                }
            };
        }

        public TraineeshipApplicationViewModel SubmitApplication(Guid candidateId, int vacancyId)
        {
            Logger.Debug("Calling TraineeeshipApplicationProvider to submit the traineeships application for candidate ID: {0}, vacancy ID: {1}.",
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
            Logger.Debug("Calling ApprenticeshipApplicationProvider to get the What Happens Next data for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            try
            {
                // var applicationDetails = _candidateService.GetApplication(candidateId, vacancyId);
                var applicationDetails = GetDummyApplication(candidateId, vacancyId);
                var model = _mapper.Map<TraineeshipApplicationDetail, TraineeshipApplicationViewModel>(applicationDetails);
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
                var message = string.Format("Get What Happens Next View Model failed for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);

                Logger.Error(message, e);

                return new WhatHappensNextViewModel(
                    MyApplicationsPageMessages.CreateOrRetrieveApplicationFailed);
            }
        }

        private static TraineeshipApplicationDetail GetDummyApplication(Guid candidateId, int vacancyId)
        {
            return new TraineeshipApplicationDetail
            {
                
            };
        }

        public TraineeshipApplicationViewModel ArchiveApplication(Guid candidateId, int vacancyId)
        {
            throw new NotImplementedException();
        }

        private TraineeshipApplicationViewModel PatchWithVacancyDetail(Guid candidateId, int vacancyId, TraineeshipApplicationViewModel apprenticheshipApplicationViewModel)
        {
            // TODO: why have a patch method like this? should be done in mapper.
            // var vacancyDetailViewModel = _vacancyDetailProvider.GetVacancyDetailViewModel(candidateId, vacancyId);
            var vacancyDetailViewModel = CreateDummyVacancyDetailViewModel(candidateId, vacancyId);

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
            apprenticheshipApplicationViewModel.Candidate.EmployerQuestionAnswers.SupplementaryQuestion1 = vacancyDetailViewModel.SupplementaryQuestion1;
            apprenticheshipApplicationViewModel.Candidate.EmployerQuestionAnswers.SupplementaryQuestion2 = vacancyDetailViewModel.SupplementaryQuestion2;

            return apprenticheshipApplicationViewModel;
        }

        private static TraineeshipApplicationViewModel FailedApplicationViewModel(int vacancyId, Guid candidateId, string failure,
            string failMessage, Exception e)
        {
            var message = string.Format("{0} {1} failed for user {2}", failure, vacancyId, candidateId);
            Logger.Error(message, e);
            return new TraineeshipApplicationViewModel(failMessage, ApplicationViewModelStatus.Error);
        }

        private VacancyDetailViewModel CreateDummyVacancyDetailViewModel(Guid candidateId, int vacancyId)
        {
            return new VacancyDetailViewModel
            {
                ApplicationInstructions = "Bacon ipsum dolor amet do filet mignon ea t-bone cupim elit pork loin salami et ullamco andouille spare ribs nostrud deserunt. Officia incididunt enim cillum sunt ex. Ham hock pork loin et jerky cillum. Excepteur shoulder turkey et in.",
                CandidateApplicationStatus = ApplicationStatuses.Draft,
                ApplyViaEmployerWebsite = false,
                ClosingDate = DateTime.Now.AddDays(45),
                Contact = "Bacon ipsum dolor amet do filet mignon ea t-bone cupim elit pork loin salami et ullamco andouille spare ribs nostrud deserunt. ",
                Description = "Meatball est t-bone boudin non shank doner jerky commodo anim nisi tempor chicken nulla. Ut sint ball tip bacon, fugiat ut duis cupim strip steak corned beef pig salami ut. Prosciutto frankfurter ea sausage do porchetta cillum exercitation voluptate eiusmod velit tenderloin. Laborum eu esse, sint sausage nisi pig incididunt magna chuck ball tip ad in. Sirloin turducken ipsum irure esse in ball tip dolor sunt ea kevin nulla landjaeger. Non landjaeger eiusmod chuck, kevin bacon commodo jowl ham incididunt boudin et nulla do.",
                EmployerDescription = "A very good employer",
                EmployerName = "Dummy employer",
                EmployerWebsite = "http://dummyemployer.com",
                ExpectedDuration = "6 months",
                FullDescription = "Ground round meatball corned beef rump, sed est sint non filet mignon leberkas pork belly qui minim chuck enim. Brisket ut capicola adipisicing esse veniam. Fatback id adipisicing, drumstick pastrami in ham hock bresaola laborum biltong landjaeger spare ribs. In sunt occaecat sint boudin voluptate ut hamburger rump flank brisket proident pariatur chuck. Beef ribs in enim sausage eu venison short ribs.",
                IsEmployerAnonymous = false,
                IsNasProvider = true,
                IsRecruitmentAgencyAnonymous = false,
                IsWellFormedEmployerWebsiteUrl = true,
                IsWellFormedVacancyUrl = true,
                ProviderName = "Dummy provider",
                QualificationRequired = "A",
                ProviderSectorPassRate = 69,
                RealityCheck = "Deserunt sunt sed velit, pork loin laborum ut occaecat andouille ea. Alcatra et elit chuck tempor, ad pancetta occaecat. Short ribs prosciutto sausage biltong, eu cow excepteur spare ribs. Est ad shankle picanha. Culpa picanha leberkas pig, sausage laborum officia duis beef ribs pork chop kevin jerky. Excepteur strip steak esse jerky hamburger.",
                RecruitmentAgency = "Dummy agency",
                SupplementaryQuestion1 = "Mollit est prosciutto venison tongue cupidatat non pork chop elit id bresaola ad cow kielbasa",
                SupplementaryQuestion2 = "Swine pork loin jowl, filet mignon dolor frankfurter aliquip ullamco do.",
                Wage = "100 pounds per week",
                VacancyReference = "DUMMY_VACACNY_REFERENCE",
                Title = "Dummy vacancy title",
                Id = vacancyId
            };
        }
    }
}