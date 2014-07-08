namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Collections.Generic;
    using ViewModels.Applications;
    using ViewModels.Candidate;
    using ViewModels.Locations;

    internal class ApplicationProvider : IApplicationProvider
    {
        private readonly IVacancyDetailProvider _vacancyDetailProvider;

        public ApplicationProvider(IVacancyDetailProvider vacancyDetailProvider)
        {
            _vacancyDetailProvider = vacancyDetailProvider;
        }

        public ApplicationViewModel GetApplicationViewModel(int vacancyId, int mockProfileId)
        {
            var vacancy = _vacancyDetailProvider.GetVacancyDetailViewModel(vacancyId);

            if (vacancy == null)
            {
                return null;
            }

            var profile = GetDummyCandidateViewModel(mockProfileId);
            profile.Id = mockProfileId;
            profile.EmployerQuestionAnswers = new EmployerQuestionAnswersViewModel
            {
                SupplementaryQuestion1 = vacancy.SupplementaryQuestion1,
                SupplementaryQuestion2 = vacancy.SupplementaryQuestion2
            };

            var appViewModel = new ApplicationViewModel
            {
                Candidate = profile,
                VacancyDetail = vacancy,
            };

            return appViewModel;
        }

        public ApplicationViewModel MergeApplicationViewModel(int vacancyId, int mockProfileId, ApplicationViewModel userApplicationViewModel)
        {
            var existingApplicationViewModel = GetApplicationViewModel(vacancyId, mockProfileId);

            userApplicationViewModel.VacancyDetail = existingApplicationViewModel.VacancyDetail;
            userApplicationViewModel.Candidate.Id = existingApplicationViewModel.Candidate.Id;
            userApplicationViewModel.Candidate.FullName = existingApplicationViewModel.Candidate.FullName;
            userApplicationViewModel.Candidate.EmailAddress = existingApplicationViewModel.Candidate.EmailAddress;
            userApplicationViewModel.Candidate.DateOfBirth = existingApplicationViewModel.Candidate.DateOfBirth;
            userApplicationViewModel.Candidate.Address = existingApplicationViewModel.Candidate.Address;

            if (!string.IsNullOrWhiteSpace(existingApplicationViewModel.VacancyDetail.SupplementaryQuestion1) ||
                !string.IsNullOrWhiteSpace(existingApplicationViewModel.VacancyDetail.SupplementaryQuestion2))
            {
                userApplicationViewModel.Candidate.EmployerQuestionAnswers.SupplementaryQuestion1 = existingApplicationViewModel.VacancyDetail.SupplementaryQuestion1;
                userApplicationViewModel.Candidate.EmployerQuestionAnswers.SupplementaryQuestion2 = existingApplicationViewModel.VacancyDetail.SupplementaryQuestion2;    
            }

            return userApplicationViewModel;
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