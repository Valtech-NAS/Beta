namespace SFA.Apprenticeships.Web.Candidate.Mappers.Resolvers
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Locations;
    using Domain.Entities.Vacancies;
    using ViewModels.Applications;
    using ViewModels.Candidate;
    using ViewModels.Locations;

    public class ApplicationViewModelToApplicationDetailResolver :
        ITypeConverter<ApplicationViewModel, ApplicationDetail>
    {
        public ApplicationDetail Convert(ResolutionContext context)
        {
            var model = (ApplicationViewModel) context.SourceValue;

            var application = new ApplicationDetail
            {
                CandidateId = model.Candidate.Id,
                Vacancy = new VacancySummary
                {
                    Id = model.VacancyDetail.Id,
                    ClosingDate = model.VacancyDetail.ClosingDate,
                    Description = model.VacancyDetail.Description,
                    EmployerName = model.VacancyDetail.EmployerName,
                    Location = new GeoPoint
                    {
                        Longitude = model.VacancyDetail.VacancyAddress.GeoPoint.Longitude,
                        Latitude = model.VacancyDetail.VacancyAddress.GeoPoint.Latitude
                    },
                    Title = model.VacancyDetail.Title,
                },
                CandidateDetails = new RegistrationDetails
                {
                    EmailAddress = model.Candidate.EmailAddress,
                    Address = GetAddress(model.Candidate.Address),
                    DateOfBirth = model.Candidate.DateOfBirth,
                    PhoneNumber = model.Candidate.PhoneNumber,
                    FirstName = model.Candidate.FirstName,
                    LastName = model.Candidate.LastName,
                    MiddleNames = model.Candidate.MiddleName
                },
                CandidateInformation = new ApplicationTemplate
                {
                    AboutYou = GetAboutYou(model.Candidate.AboutYou),
                    EducationHistory = GetEducationHistory(model.Candidate.Education),
                    Qualifications = GetQualifications(model.Candidate.Qualifications),
                    WorkExperience = GetWorkExperience(model.Candidate.WorkExperience),
                },
            };

            return application;
        }

        #region Helpers
     
        private static Address GetAddress(AddressViewModel model)
        {
            return new Address
            {
                AddressLine1 = model.AddressLine1,
                AddressLine2 = model.AddressLine2,
                AddressLine3 = model.AddressLine3,
                AddressLine4 = model.AddressLine4,
                GeoPoint = new GeoPoint
                {
                    Longitude = model.GeoPoint.Longitude,
                    Latitude = model.GeoPoint.Latitude
                },
                Postcode = model.Postcode,
                Uprn = model.Uprn
            };
        }

        private static AboutYou GetAboutYou(AboutYouViewModel model)
        {
            return new AboutYou
            {
                HobbiesAndInterests = model.WhatAreYourHobbiesInterests,
                Improvements = model.WhatDoYouFeelYouCouldImprove,
                Strengths = model.WhatAreYourStrengths,
                Support = model.AnythingWeCanDoToSupportYourInterview
            };
        }

        private static Education GetEducationHistory(EducationViewModel model)
        {
            return new Education
            {
                FromYear = GetIntValue(model.FromYear),
                Institution = model.NameOfMostRecentSchoolCollege,
                ToYear = GetIntValue(model.ToYear)
            };
        }

        private static IList<WorkExperience> GetWorkExperience(IEnumerable<WorkExperienceViewModel> workExperienceViewModels)
        {
            return workExperienceViewModels.Select(model => new WorkExperience
            {
                Description = model.Description,
                Employer = model.Employer,
                JobTitle = model.JobTitle,
                FromYear = model.FromYear,
                ToYear = model.ToYear
            }).ToList();
        }

        private static IList<Qualification> GetQualifications(IEnumerable<QualificationsViewModel> qualifications)
        {
            return qualifications.Select(model => new Qualification
            {
                Grade = model.Grade,
                IsPredicted = model.IsPredicted,
                QualificationType = model.QualificationType,
                Subject = model.Subject,
                Year = model.Year
            }).ToList();
        }

        private static int GetIntValue(string stringYear)
        {
            int year;
            int.TryParse(stringYear, out year);
            return year;
        }

        #endregion
    }
}