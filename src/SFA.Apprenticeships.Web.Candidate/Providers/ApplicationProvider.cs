namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Collections.Generic;
    using Application.Interfaces.Candidates;
    using ViewModels.Applications;
    using ViewModels.Candidate;
    using ViewModels.Locations;

    internal class ApplicationProvider : IApplicationProvider
    {
        private readonly IVacancyDetailProvider _vacancyDetailProvider;
        private readonly ICandidateService _candidateService;

        public ApplicationProvider(
            IVacancyDetailProvider vacancyDetailProvider,
            ICandidateService candidateService)
        {
            _candidateService = candidateService;
            _vacancyDetailProvider = vacancyDetailProvider;
        }

        public ApplicationViewModel GetApplicationViewModel(int vacancyId, Guid candidateId)
        {
            var vacancyViewModel = _vacancyDetailProvider.GetVacancyDetailViewModel(vacancyId);

            if (vacancyViewModel == null)
            {
                return null;
            }

            var candidate = _candidateService.GetCandidate(candidateId);

            // TODO: create mapper.
            var candidateViewModel = new CandidateViewModel
            {
                Id = candidateId,
                FullName = candidate.RegistrationDetails.FullName,
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

        public ApplicationViewModel MergeApplicationViewModel(int vacancyId, Guid candidateId, ApplicationViewModel applicationViewModel)
        {
            var existingApplicationViewModel = GetApplicationViewModel(vacancyId, candidateId);

            applicationViewModel.VacancyDetail = existingApplicationViewModel.VacancyDetail;
            applicationViewModel.Candidate.Id = existingApplicationViewModel.Candidate.Id;
            applicationViewModel.Candidate.FullName = existingApplicationViewModel.Candidate.FullName;
            applicationViewModel.Candidate.EmailAddress = existingApplicationViewModel.Candidate.EmailAddress;
            applicationViewModel.Candidate.DateOfBirth = existingApplicationViewModel.Candidate.DateOfBirth;
            applicationViewModel.Candidate.Address = existingApplicationViewModel.Candidate.Address;

            if (!string.IsNullOrWhiteSpace(existingApplicationViewModel.VacancyDetail.SupplementaryQuestion1) ||
                !string.IsNullOrWhiteSpace(existingApplicationViewModel.VacancyDetail.SupplementaryQuestion2))
            {
                applicationViewModel.Candidate.EmployerQuestionAnswers.SupplementaryQuestion1 = existingApplicationViewModel.VacancyDetail.SupplementaryQuestion1;
                applicationViewModel.Candidate.EmployerQuestionAnswers.SupplementaryQuestion2 = existingApplicationViewModel.VacancyDetail.SupplementaryQuestion2;    
            }

            return applicationViewModel;
        }

        public void SaveApplication(ApplicationViewModel applicationViewModel)
        {
            throw new NotImplementedException();
        }
    }
}
