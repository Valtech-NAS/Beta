namespace SFA.Apprenticeships.Web.Candidate.Mappers.Resolvers
{
    using System.Linq;
    using SFA.Apprenticeships.Domain.Entities.Applications;
    using SFA.Apprenticeships.Web.Candidate.Mappers.Helpers;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Candidate;

    public static class CandidateViewModelResolver
    {
        public static T Resolve<T>(this T candidateViewModel, ApplicationDetail applicationDetail) 
            where T : CandidateViewModelBase
        {
            candidateViewModel.Id = applicationDetail.CandidateId;
            candidateViewModel.EmailAddress = applicationDetail.CandidateDetails.EmailAddress;
            candidateViewModel.FirstName = applicationDetail.CandidateDetails.FirstName;
            candidateViewModel.LastName = applicationDetail.CandidateDetails.LastName;
            candidateViewModel.DateOfBirth = applicationDetail.CandidateDetails.DateOfBirth;
            candidateViewModel.PhoneNumber = applicationDetail.CandidateDetails.PhoneNumber;
            candidateViewModel.Address =
                ApplicationConverter.GetAddressViewModel(applicationDetail.CandidateDetails.Address);
            candidateViewModel.Qualifications =
                ApplicationConverter.GetQualificationsViewModels(applicationDetail.CandidateInformation.Qualifications);
            candidateViewModel.HasQualifications =
                ApplicationConverter.GetQualificationsViewModels(applicationDetail.CandidateInformation.Qualifications)
                    .Any();
            candidateViewModel.WorkExperience =
                ApplicationConverter.GetWorkExperiencesViewModels(applicationDetail.CandidateInformation.WorkExperience);
            candidateViewModel.HasWorkExperience =
                ApplicationConverter.GetWorkExperiencesViewModels(applicationDetail.CandidateInformation.WorkExperience)
                    .Any();
            candidateViewModel.EmployerQuestionAnswers = new EmployerQuestionAnswersViewModel
            {
                CandidateAnswer1 = applicationDetail.AdditionalQuestion1Answer,
                CandidateAnswer2 = applicationDetail.AdditionalQuestion2Answer
            };

            return candidateViewModel;
        }
    }
}