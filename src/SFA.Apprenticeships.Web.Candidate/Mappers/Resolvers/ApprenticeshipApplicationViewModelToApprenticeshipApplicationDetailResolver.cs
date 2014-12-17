namespace SFA.Apprenticeships.Web.Candidate.Mappers.Resolvers
{
    using System.Collections.Generic;
    using AutoMapper;
    using SFA.Apprenticeships.Domain.Entities.Applications;
    using SFA.Apprenticeships.Domain.Entities.Candidates;
    using SFA.Apprenticeships.Domain.Entities.Locations;
    using SFA.Apprenticeships.Domain.Entities.Users;
    using SFA.Apprenticeships.Domain.Entities.Vacancies.Apprenticeships;
    using SFA.Apprenticeships.Web.Candidate.Mappers.Helpers;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Applications;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Candidate;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch;

    public class ApprenticeshipApplicationViewModelToApprenticeshipApplicationDetailResolver :
        ITypeConverter<ApprenticheshipApplicationViewModel, ApprenticeshipApplicationDetail>
    {
        public ApprenticeshipApplicationDetail Convert(ResolutionContext context)
        {
            var model = (ApprenticheshipApplicationViewModel) context.SourceValue;

            var application = new ApprenticeshipApplicationDetail
            {
                CandidateId = model.Candidate.Id,
                Vacancy = GetVacancy(model.VacancyDetail),
                CandidateDetails = GetCandidateDetails(model.Candidate),
                CandidateInformation = GetCandidateInformation(model.Candidate),
                AdditionalQuestion1Answer =
                    model.Candidate.EmployerQuestionAnswers != null
                        ? model.Candidate.EmployerQuestionAnswers.CandidateAnswer1
                        : string.Empty,
                AdditionalQuestion2Answer =
                    model.Candidate.EmployerQuestionAnswers != null
                        ? model.Candidate.EmployerQuestionAnswers.CandidateAnswer2
                        : string.Empty
            };

            return application;
        }

        private static ApplicationTemplate GetCandidateInformation(ApprenticeshipCandidateViewModel modelBase)
        {
            return new ApplicationTemplate
            {
                AboutYou = ApplicationConverter.GetAboutYou(modelBase.AboutYou),
                EducationHistory = ApplicationConverter.GetEducation(modelBase.Education),
                Qualifications =
                    modelBase.HasQualifications
                        ? ApplicationConverter.GetQualifications(modelBase.Qualifications)
                        : new List<Qualification>(),
                WorkExperience =
                    modelBase.HasWorkExperience
                        ? ApplicationConverter.GetWorkExperiences(modelBase.WorkExperience)
                        : new List<WorkExperience>(),
            };
        }

        private static RegistrationDetails GetCandidateDetails(CandidateViewModelBase modelBase)
        {
            return new RegistrationDetails
            {
                EmailAddress = modelBase.EmailAddress,
                Address = ApplicationConverter.GetAddress(modelBase.Address),
                DateOfBirth = modelBase.DateOfBirth,
                PhoneNumber = modelBase.PhoneNumber,
                FirstName = modelBase.FirstName,
                LastName = modelBase.LastName,
            };
        }

        private static ApprenticeshipSummary GetVacancy(VacancyDetailViewModel model)
        {
            return new ApprenticeshipSummary
            {
                Id = model.Id,
                ClosingDate = model.ClosingDate,
                Description = model.Description,
                EmployerName = model.EmployerName,
                Location = new GeoPoint
                {
                    Longitude = model.VacancyAddress.GeoPoint.Longitude,
                    Latitude = model.VacancyAddress.GeoPoint.Latitude
                },
                Title = model.Title,
            };
        }
    }
}