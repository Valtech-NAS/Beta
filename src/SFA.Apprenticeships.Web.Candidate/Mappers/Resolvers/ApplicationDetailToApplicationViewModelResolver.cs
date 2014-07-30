namespace SFA.Apprenticeships.Web.Candidate.Mappers.Resolvers
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using AutoMapper;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Locations;
    using ViewModels.Applications;
    using ViewModels.Candidate;
    using ViewModels.Locations;

    public class ApplicationDetailToApplicationViewModelResolver :
        ITypeConverter<ApplicationDetail, ApplicationViewModel>
    {
        public ApplicationViewModel Convert(ResolutionContext context)
        {
            var application = (ApplicationDetail) context.SourceValue;

            var model = new ApplicationViewModel
            {
                Candidate = new CandidateViewModel
                {
                    Id = application.CandidateId,
                    EmailAddress = application.CandidateDetails.EmailAddress,
                    FirstName = application.CandidateDetails.FirstName,
                    MiddleName = application.CandidateDetails.MiddleNames,
                    LastName = application.CandidateDetails.LastName,
                    DateOfBirth = application.CandidateDetails.DateOfBirth,
                    PhoneNumber = application.CandidateDetails.PhoneNumber,
                    Address = GetAddressViewModel(application.CandidateDetails.Address),
                    AboutYou = GetAboutYou(application.CandidateInformation.AboutYou),
                    Education = GetEducation(application.CandidateInformation.EducationHistory),
                    Qualifications = GetQualifications(application.CandidateInformation.Qualifications),
                    WorkExperience = GetWorkExperience(application.CandidateInformation.WorkExperience),
                    EmployerQuestionAnswers = new EmployerQuestionAnswersViewModel()
                }
            };

            return model;
        }

        #region Helpers

        private static AddressViewModel GetAddressViewModel(Address address)
        {
            return new AddressViewModel
            {
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                AddressLine3 = address.AddressLine3,
                AddressLine4 = address.AddressLine4,
                Postcode = address.Postcode,
                GeoPoint = new GeoPointViewModel
                {
                    Longitude = address.GeoPoint.Longitude,
                    Latitude = address.GeoPoint.Latitude
                },
                Uprn = address.Uprn
            };
        }

        private static IEnumerable<QualificationsViewModel> GetQualifications(IEnumerable<Qualification> qualifications)
        {
            return qualifications.Select(q => new QualificationsViewModel()
            {
                Grade = q.Grade,
                IsPredicted = q.IsPredicted,
                QualificationType = q.QualificationType,
                Subject = q.Subject,
                Year = q.Year
            }).AsEnumerable();
        }

        private static IEnumerable<WorkExperienceViewModel> GetWorkExperience(
            IEnumerable<WorkExperience> workExperiences)
        {
            return workExperiences.Select(m => new WorkExperienceViewModel
            {
                Description = m.Description,
                Employer = m.Employer,
                JobTitle = m.JobTitle,
                FromYear = m.FromYear,
                ToYear = m.ToYear
            }).AsEnumerable();
        }

        private static EducationViewModel GetEducation(Education educationHistory)
        {
            return new EducationViewModel
            {
                FromYear = educationHistory.FromYear.ToString(CultureInfo.InvariantCulture),
                NameOfMostRecentSchoolCollege = educationHistory.Institution,
                ToYear = educationHistory.ToYear.ToString(CultureInfo.InvariantCulture)
            };
        }

        private static AboutYouViewModel GetAboutYou(AboutYou aboutYou)
        {
            return new AboutYouViewModel
            {
                AnythingWeCanDoToSupportYourInterview = aboutYou.Support,
                WhatAreYourHobbiesInterests = aboutYou.HobbiesAndInterests,
                WhatAreYourStrengths = aboutYou.Strengths,
                WhatDoYouFeelYouCouldImprove = aboutYou.Improvements
            };
        }

        #endregion
    }
}