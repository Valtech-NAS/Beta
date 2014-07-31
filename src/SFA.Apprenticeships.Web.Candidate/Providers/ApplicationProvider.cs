namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Application.Interfaces.Candidates;
    using Common.Providers;
    using Domain.Interfaces.Mapping;
    using ViewModels.Applications;
    using ViewModels.Candidate;
    using ViewModels.Locations;

    internal class ApplicationProvider : IApplicationProvider
    {
        private readonly ICandidateService _candidateService;
        private readonly IMapper _mapper;
        private readonly IUserServiceProvider _userServiceProvider;
        private readonly IVacancyDetailProvider _vacancyDetailProvider;
        private const string ApplicationContextName = "Application.Context";

        public ApplicationProvider(
            IVacancyDetailProvider vacancyDetailProvider,
            ICandidateService candidateService, IMapper mapper, IUserServiceProvider userServiceProvider)
        {
            _candidateService = candidateService;
            _mapper = mapper;
            _userServiceProvider = userServiceProvider;
            _vacancyDetailProvider = vacancyDetailProvider;
        }

        public ApplicationViewModel GetApplicationViewModel(int vacancyId, Guid candidateId)
        {
            //TODO Push EntityId for the application into context and use a random guid for ViewModelId
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
            var existingApplicationViewModel = GetApplicationViewModel(vacancyId, candidateId);

            applicationViewModel.VacancyDetail = existingApplicationViewModel.VacancyDetail;
            applicationViewModel.Candidate.Id = existingApplicationViewModel.Candidate.Id;
            applicationViewModel.Candidate.FirstName = existingApplicationViewModel.Candidate.FirstName;
            applicationViewModel.Candidate.LastName = existingApplicationViewModel.Candidate.LastName;
            applicationViewModel.Candidate.EmailAddress = existingApplicationViewModel.Candidate.EmailAddress;
            applicationViewModel.Candidate.DateOfBirth = existingApplicationViewModel.Candidate.DateOfBirth;
            applicationViewModel.Candidate.Address = existingApplicationViewModel.Candidate.Address;

            if (!string.IsNullOrWhiteSpace(existingApplicationViewModel.VacancyDetail.SupplementaryQuestion1) ||
                !string.IsNullOrWhiteSpace(existingApplicationViewModel.VacancyDetail.SupplementaryQuestion2))
            {
                applicationViewModel.Candidate.EmployerQuestionAnswers.SupplementaryQuestion1 =
                    existingApplicationViewModel.VacancyDetail.SupplementaryQuestion1;
                applicationViewModel.Candidate.EmployerQuestionAnswers.SupplementaryQuestion2 =
                    existingApplicationViewModel.VacancyDetail.SupplementaryQuestion2;
            }

            return applicationViewModel;
        }

        public void SaveApplication(ApplicationViewModel applicationViewModel)
        {
            //var application = _mapper.Map<ApplicationViewModel, ApplicationDetail>(applicationViewModel);
            throw new NotImplementedException();
        }

        public void SubmitApplication(Guid applicationViewId)
        {
            var httpContext = new HttpContextWrapper(HttpContext.Current);
            var applicationContext = _userServiceProvider.GetEntityContextCookie(httpContext, ApplicationContextName);

            _candidateService.SubmitApplication(applicationContext.EntityId);
        }
    }
}
