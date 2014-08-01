namespace SFA.Apprenticeships.Web.Candidate.Mappers.Resolvers
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Locations;
    using Domain.Entities.Vacancies;
    using Helpers;
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
                    Address = ApplicationConverter.GetAddress(model.Candidate.Address),
                    DateOfBirth = model.Candidate.DateOfBirth,
                    PhoneNumber = model.Candidate.PhoneNumber,
                    FirstName = model.Candidate.FirstName,
                    LastName = model.Candidate.LastName,
                    MiddleNames = model.Candidate.MiddleName
                },
                CandidateInformation = new ApplicationTemplate
                {
                    AboutYou = ApplicationConverter.GetAboutYou(model.Candidate.AboutYou),
                    EducationHistory = ApplicationConverter.GetEducation(model.Candidate.Education),
                    Qualifications = ApplicationConverter.GetQualifications(model.Candidate.Qualifications),
                    WorkExperience = ApplicationConverter.GetWorkExperiences(model.Candidate.WorkExperience),
                },
                AdditionalQuestion1Answer = model.Candidate.EmployerQuestionAnswers.CandidateAnswer1,
                AdditionalQuestion2Answer = model.Candidate.EmployerQuestionAnswers.CandidateAnswer2
            };

            return application;
        }
    }
}