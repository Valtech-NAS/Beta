namespace SFA.Apprenticeships.Web.Candidate.Mappers.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Domain.Entities.Candidates;
    using Domain.Entities.Locations;
    using ViewModels.Candidate;
    using ViewModels.Locations;

    public static class ApplicationConverter
    {
        #region Entity To Model Converters

        public static AddressViewModel GetAddressViewModel(Address address)
        {
            var addressViewModel = new AddressViewModel
            {
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                AddressLine3 = address.AddressLine3,
                AddressLine4 = address.AddressLine4,
                Postcode = address.Postcode,
                Uprn = address.Uprn
            };

            if (address.GeoPoint != null)
            {
                addressViewModel.GeoPoint = new GeoPointViewModel
                {
                    Longitude = address.GeoPoint.Longitude,
                    Latitude = address.GeoPoint.Latitude
                };
            }

            return addressViewModel;
        }

        public static IEnumerable<QualificationsViewModel> GetQualificationsViewModels(
            IEnumerable<Qualification> qualifications)
        {
            return qualifications.Select(q => new QualificationsViewModel
            {
                Grade = q.Grade,
                IsPredicted = q.IsPredicted,
                QualificationType = q.QualificationType,
                Subject = q.Subject,
                Year = ConvertYearIntToString(q.Year)
            }).AsEnumerable();
        }

        public static IEnumerable<WorkExperienceViewModel> GetWorkExperiencesViewModels(
            IEnumerable<WorkExperience> workExperiences)
        {
            return workExperiences.Select(m => new WorkExperienceViewModel
            {
                Description = m.Description,
                Employer = m.Employer,
                JobTitle = m.JobTitle,
                FromMonth = m.FromDate.Month,
                FromYear = ConvertYearIntToString(m.FromDate.Year),
                ToMonth = m.ToDate.Month,
                ToYear = ConvertYearIntToString(m.ToDate.Year)
            }).AsEnumerable();
        }

        public static EducationViewModel GetEducationViewModel(Education educationHistory)
        {
            if (educationHistory == null)
            {
                return null;
            }

            return new EducationViewModel
            {
                FromYear = educationHistory.FromYear,
                NameOfMostRecentSchoolCollege = educationHistory.Institution,
                ToYear = educationHistory.ToYear
            };
        }

        public static AboutYouViewModel GetAboutYouViewModel(AboutYou aboutYou)
        {
            return new AboutYouViewModel
            {
                AnythingWeCanDoToSupportYourInterview = aboutYou.Support,
                RequiresSupportForInterview = !string.IsNullOrEmpty(aboutYou.Support),
                WhatAreYourHobbiesInterests = aboutYou.HobbiesAndInterests,
                WhatAreYourStrengths = aboutYou.Strengths,
                WhatDoYouFeelYouCouldImprove = aboutYou.Improvements
            };
        }

        #endregion

        #region ViewModel To Entity Converters

        public static Address GetAddress(AddressViewModel model)
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

        public static AboutYou GetAboutYou(AboutYouViewModel model)
        {
            return new AboutYou
            {
                HobbiesAndInterests = model.WhatAreYourHobbiesInterests,
                Improvements = model.WhatDoYouFeelYouCouldImprove,
                Strengths = model.WhatAreYourStrengths,
                Support = model.RequiresSupportForInterview ? model.AnythingWeCanDoToSupportYourInterview : string.Empty
            };
        }

        public static Education GetEducation(EducationViewModel model)
        {
            if (model == null)
            {
                return null;
            }

            return new Education
            {
// ReSharper disable once PossibleInvalidOperationException
                FromYear = model.FromYear.Value,
                Institution = model.NameOfMostRecentSchoolCollege,
// ReSharper disable once PossibleInvalidOperationException
                ToYear = model.ToYear.Value
            };
        }

        public static IList<WorkExperience> GetWorkExperiences(
            IEnumerable<WorkExperienceViewModel> workExperienceViewModels)
        {
            if (workExperienceViewModels == null)
            {
                return new List<WorkExperience>();
            }

            return workExperienceViewModels.Select(model => new WorkExperience
            {
                Description = model.Description,
                Employer = model.Employer,
                JobTitle = model.JobTitle,
                FromDate = new DateTime(ConvertYearStringToInt(model.FromYear), model.FromMonth, 1),
                ToDate =
                    ConvertYearStringToInt(model.ToYear) != 0
                        ? new DateTime(ConvertYearStringToInt(model.ToYear), model.ToMonth, 1)
                        : DateTime.MinValue
            }).ToList();
        }

        public static IList<Qualification> GetQualifications(
            IEnumerable<QualificationsViewModel> qualifications)
        {
            if (qualifications == null)
            {
                return new List<Qualification>();
            }

            return qualifications.Select(model => new Qualification
            {
                Grade = model.Grade,
                IsPredicted = model.IsPredicted,
                QualificationType = model.QualificationType,
                Subject = model.Subject,
                Year = ConvertYearStringToInt(model.Year)
            }).ToList();
        }

        private static int ConvertYearStringToInt(string stringYear)
        {
            int year;
            int.TryParse(stringYear, out year);
            return year;
        }

        private static string ConvertYearIntToString(int intYear)
        {
            //Using 1 because we using DateTime.MinValue
            return intYear > 1 ? intYear.ToString(CultureInfo.InvariantCulture) : string.Empty;
        }

        #endregion
    }
}