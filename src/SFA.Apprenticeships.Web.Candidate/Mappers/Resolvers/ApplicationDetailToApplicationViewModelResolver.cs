namespace SFA.Apprenticeships.Web.Candidate.Mappers.Resolvers
{
    using System.Linq;
    using AutoMapper;
    using Domain.Entities.Applications;
    using Helpers;
    using ViewModels.Applications;
    using ViewModels.Candidate;

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
                    LastName = application.CandidateDetails.LastName,
                    DateOfBirth = application.CandidateDetails.DateOfBirth,
                    PhoneNumber = application.CandidateDetails.PhoneNumber,
                    Address = ApplicationConverter.GetAddressViewModel(application.CandidateDetails.Address),
                    AboutYou = ApplicationConverter.GetAboutYouViewModel(application.CandidateInformation.AboutYou),
                    Education = ApplicationConverter.GetEducationViewModel(application.CandidateInformation.EducationHistory),
                    Qualifications = ApplicationConverter.GetQualificationsViewModels(application.CandidateInformation.Qualifications),
                    HasQualifications = ApplicationConverter.GetQualificationsViewModels(application.CandidateInformation.Qualifications).Any(),
                    WorkExperience = ApplicationConverter.GetWorkExperiencesViewModels(application.CandidateInformation.WorkExperience),
                    HasWorkExperience = ApplicationConverter.GetWorkExperiencesViewModels(application.CandidateInformation.WorkExperience).Any(),
                    EmployerQuestionAnswers = new EmployerQuestionAnswersViewModel
                    {
                        CandidateAnswer1 = application.AdditionalQuestion1Answer,
                        CandidateAnswer2 = application.AdditionalQuestion2Answer
                    }
                },
                DateUpdated = application.DateUpdated,
                VacancyId = application.Vacancy.Id
            };

            return model;
        }
    }
}