namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Application.Interfaces.Candidates;
    using Common.Providers;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Mapping;
    using Mappers.Helpers;
    using ViewModels.Applications;
    using ViewModels.Candidate;
    using ViewModels.Locations;

    internal class ApplicationProvider : IApplicationProvider
    {
        private const string ApplicationContextName = "Application.Context";

        private readonly IVacancyDetailProvider _vacancyDetailProvider;
        private readonly ICandidateService _candidateService;
        private readonly IUserServiceProvider _userServiceProvider;
        private readonly IMapper _mapper;

        public ApplicationProvider(
            IVacancyDetailProvider vacancyDetailProvider,
            ICandidateService candidateService,
            IUserServiceProvider userServiceProvider,
            IMapper mapper)
        {
            _vacancyDetailProvider = vacancyDetailProvider;
            _candidateService = candidateService;
            _userServiceProvider = userServiceProvider;
            _mapper = mapper;
        }

        public ApplicationViewModel GetApplicationViewModel(int vacancyId, Guid candidateId)
        {
            var applicationDetails = _candidateService.CreateApplication(candidateId, vacancyId);

            // TODO: US354: AG: set entity cookie, refactor into separate function.
            var viewModelId = Guid.NewGuid();
            var httpContext = new HttpContextWrapper(HttpContext.Current);
            
            _userServiceProvider.SetEntityContextCookie(
                httpContext, applicationDetails.EntityId, viewModelId, ApplicationContextName);

            var vacancyViewModel = _vacancyDetailProvider.GetVacancyDetailViewModel(vacancyId);

            if (vacancyViewModel == null)
            {
                return null;
            }

            var candidate = _candidateService.GetCandidate(candidateId);

            // TODO: should call the application creation operation on the candidate service (sprint 15)
            var candidateViewModel = new CandidateViewModel
            {
                Id = candidateId,
                FirstName = candidate.RegistrationDetails.FirstName,
                LastName = candidate.RegistrationDetails.LastName,
                EmailAddress = candidate.RegistrationDetails.EmailAddress,
                DateOfBirth = candidate.RegistrationDetails.DateOfBirth,
                PhoneNumber = candidate.RegistrationDetails.PhoneNumber,
                Address = new AddressViewModel
                {
                    AddressLine1 = candidate.RegistrationDetails.Address.AddressLine1,
                    AddressLine2 = candidate.RegistrationDetails.Address.AddressLine2,
                    AddressLine3 = candidate.RegistrationDetails.Address.AddressLine3,
                    AddressLine4 = candidate.RegistrationDetails.Address.AddressLine4,
                    Postcode = candidate.RegistrationDetails.Address.Postcode
                },
                AboutYou = new AboutYouViewModel(),
                Qualifications = new List<QualificationsViewModel>(),
                WorkExperience = new List<WorkExperienceViewModel>(),
                EmployerQuestionAnswers = new EmployerQuestionAnswersViewModel
                {
                    SupplementaryQuestion1 = vacancyViewModel.SupplementaryQuestion1,
                    SupplementaryQuestion2 = vacancyViewModel.SupplementaryQuestion2
                }
            };

            var applicationViewModel = new ApplicationViewModel
            {
                Candidate = candidateViewModel,
                VacancyDetail = vacancyViewModel,
            };

            return applicationViewModel;
        }

        public ApplicationViewModel MergeApplicationViewModel(int vacancyId, Guid candidateId,
            ApplicationViewModel applicationViewModel)
        {
            var candidate = _candidateService.GetCandidate(candidateId);

            if (candidate == null)
            {
                throw new CustomException("Candidate not found", ErrorCodes.UnknownCandidateError);
            }

            SaveApplication(applicationViewModel);

            candidate.ApplicationTemplate = new ApplicationTemplate
            {
                AboutYou = ApplicationConverter.GetAboutYou(applicationViewModel.Candidate.AboutYou),
                EducationHistory = ApplicationConverter.GetEducation(applicationViewModel.Candidate.Education),
                Qualifications = ApplicationConverter.GetQualifications(applicationViewModel.Candidate.Qualifications),
                WorkExperience = ApplicationConverter.GetWorkExperiences(applicationViewModel.Candidate.WorkExperience),
            };

            _candidateService.SaveCandidate(candidate);
          
            return applicationViewModel;
        }

        public void SaveApplication(ApplicationViewModel applicationViewModel)
        {
            var applicationEntityId = GetApplicationEntityIdFromContext(applicationViewModel.ApplicationViewId);
            var application = _mapper.Map<ApplicationViewModel, ApplicationDetail>(applicationViewModel);
            application.EntityId = applicationEntityId;

            _candidateService.SaveApplication(application);
        }

        public void SubmitApplication(Guid applicationViewId)
        {
            var applicationEntityId = GetApplicationEntityIdFromContext(applicationViewId);
            _candidateService.SubmitApplication(applicationEntityId);
        }

        private Guid GetApplicationEntityIdFromContext(Guid applicationViewId)
        {
            var httpContext = new HttpContextWrapper(HttpContext.Current);
            var applicationContext = _userServiceProvider.GetEntityContextCookie(httpContext, ApplicationContextName);

            if (applicationContext != null && applicationContext.ViewModelId == applicationViewId)
            {
                return applicationContext.EntityId;
            }

            throw new CustomException("ApplicationViewId is not in context", ErrorCodes.ApplicationViewIdNotFound);
        }
    }
}