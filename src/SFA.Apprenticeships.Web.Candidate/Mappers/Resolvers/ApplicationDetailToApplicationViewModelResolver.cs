namespace SFA.Apprenticeships.Web.Candidate.Mappers.Resolvers
{
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
                    MiddleName = application.CandidateDetails.MiddleNames,
                    LastName = application.CandidateDetails.LastName,
                    DateOfBirth = application.CandidateDetails.DateOfBirth,
                    PhoneNumber = application.CandidateDetails.PhoneNumber,
                    Address = ApplicationConverter.GetAddressViewModel(application.CandidateDetails.Address),
                    AboutYou = ApplicationConverter.GetAboutYouViewModel(application.CandidateInformation.AboutYou),
                    Education =
                        ApplicationConverter.GetEducationViewModel(application.CandidateInformation.EducationHistory),
                    Qualifications =
                        ApplicationConverter.GetQualificationsViewModels(application.CandidateInformation.Qualifications),
                    WorkExperience =
                        ApplicationConverter.GetWorkExperiencesViewModels(application.CandidateInformation.WorkExperience),
                    EmployerQuestionAnswers = new EmployerQuestionAnswersViewModel()
                }
            };

            return model;
        }
            if (educationHistory == null)
            {
                return null;
            }

    }
}