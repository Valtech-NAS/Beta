namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Collections.Generic;
    using Application.Interfaces.Candidates;
    using Domain.Interfaces.Mapping;
    using ViewModels.Applications;
    using ViewModels.Candidate;
    using ViewModels.Locations;

    internal class ApplicationProvider : IApplicationProvider
    {
        private readonly IVacancyDetailProvider _vacancyDetailProvider;
        private readonly ICandidateService _candidateService;
        private IMapper _mapper;

        public ApplicationProvider(
            IVacancyDetailProvider vacancyDetailProvider,
            ICandidateService candidateService,
            IMapper mapper)
        {
            _mapper = mapper;
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
                FullName = candidate.RegistrationDetails.FirstName + " " + candidate.RegistrationDetails.LastName,
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
            throw new System.NotImplementedException();
        }

        private CandidateViewModel GetDummyCandidateViewModel(int dummyProfileId)
        {
            CandidateViewModel candidateView;
            switch (dummyProfileId)
            {
                case 1:
                    candidateView = BuildCandidateViewModel("John Doe", 
                                                    "john.doe@test-apprentice.com",
                                                    new DateTime(1997, 01, 01), 
                                                    "077-JOHN-DOE",
                                                    "10 Johndoe Road",
                                                    "MissingInAction Town",
                                                    "JD1 MIA"
                                                    );
                    break;
                case 2:
                    candidateView = BuildCandidateViewModel("Susan Defoe", 
                                                    "susan.defoe@test-apprentice.com", 
                                                    new DateTime(1996, 02, 02), 
                                                    "076-SUS-DEFOE",
                                                    "11 William Road",
                                                    "Deerstown",
                                                    "WD2 V1T");
                    candidateView.Education = BuildEducationViewModel(2010, 2012, "Defoe Agricultural College");
                    break;
                case 3:
                    candidateView = BuildCandidateViewModel("Mike Snow", 
                                                    "mike.snow@test-apprentice.com", 
                                                    new DateTime(1996, 03, 03), 
                                                    "075-MIKE-SNOW",
                                                    "12 Salted Drive",
                                                    "Downhill Avenue",
                                                    "MS3 9VT");
                    candidateView.Education = BuildEducationViewModel(2000, 2001, "Ski Schkool");
                    candidateView.AboutYou = BuildAboutYouViewModel("Strong at skiing", "Could improve my bobsleding", "Golf is a hobbie", "Coach me at skiing");
                    break;
                case 4:
                    candidateView = BuildCandidateViewModel("Barley Mow", 
                                                    "barley.mow@test-apprentice.com", 
                                                    new DateTime(1996, 04, 04), 
                                                    "075-BARLEY-MOW",
                                                    "13 Wheaty fields",
                                                    "Oatsville",
                                                    "BW12 1OT");
                    candidateView.AboutYou = BuildAboutYouViewModel("Strong at sorting the wheat from the chaff", "Could improve hay bailing", "In my spare time I love to shear sheep", "");
                    break;
                default:
                    return null;
            }

            return candidateView;
        }

        private CandidateViewModel BuildCandidateViewModel(string fullName, 
                                            string emailAddress, 
                                            DateTime dateOfBirth, 
                                            string phoneNumber, 
                                            string addressLine1, 
                                            string town, 
                                            string postCode)
        {
            var candidate = new CandidateViewModel()
            {
                FullName = fullName,
                EmailAddress = emailAddress,
                DateOfBirth = dateOfBirth,
                PhoneNumber = phoneNumber,
                Address = new AddressViewModel { AddressLine1 = addressLine1, AddressLine4 = town, Postcode = postCode },
                AboutYou = new AboutYouViewModel(),
                Qualifications = new List<QualificationsViewModel>(),
                WorkExperience = new List<WorkExperienceViewModel>()
            };

            return candidate;
        }

        private EducationViewModel BuildEducationViewModel(int fromYear, int toYear, string nameOfSchool)
        {
            return new EducationViewModel()
            {
                FromYear = fromYear.ToString(),
                ToYear = toYear.ToString(),
                NameOfMostRecentSchoolCollege = nameOfSchool
            };
        }

        private AboutYouViewModel BuildAboutYouViewModel(string strengths, string improve, string hobbies, string support)
        {
            return new AboutYouViewModel()
            {
                WhatAreYourStrengths = strengths,
                WhatDoYouFeelYouCouldImprove = improve,
                WhatAreYourHobbiesInterests = hobbies,
                AnythingWeCanDoToSupportYourInterview = support
            };
        }
    }
}