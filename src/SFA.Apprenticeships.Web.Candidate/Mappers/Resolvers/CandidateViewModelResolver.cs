namespace SFA.Apprenticeships.Web.Candidate.Mappers.Resolvers
{
    using System.Linq;
    using SFA.Apprenticeships.Domain.Entities.Applications;
    using SFA.Apprenticeships.Web.Candidate.Mappers.Helpers;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Candidate;

    public static class CandidateViewModelResolver
    {
        public static T Resolve<T>(this T candidateViewModel, ApprenticeshipApplicationDetail apprenticeshipApplicationDetail) 
            where T : CandidateViewModelBase
        {
            candidateViewModel.Id = apprenticeshipApplicationDetail.CandidateId;
            candidateViewModel.EmailAddress = apprenticeshipApplicationDetail.CandidateDetails.EmailAddress;
            candidateViewModel.FirstName = apprenticeshipApplicationDetail.CandidateDetails.FirstName;
            candidateViewModel.LastName = apprenticeshipApplicationDetail.CandidateDetails.LastName;
            candidateViewModel.DateOfBirth = apprenticeshipApplicationDetail.CandidateDetails.DateOfBirth;
            candidateViewModel.PhoneNumber = apprenticeshipApplicationDetail.CandidateDetails.PhoneNumber;
            candidateViewModel.Address =
                ApplicationConverter.GetAddressViewModel(apprenticeshipApplicationDetail.CandidateDetails.Address);
            candidateViewModel.Qualifications =
                ApplicationConverter.GetQualificationsViewModels(apprenticeshipApplicationDetail.CandidateInformation.Qualifications);
            candidateViewModel.HasQualifications =
                ApplicationConverter.GetQualificationsViewModels(apprenticeshipApplicationDetail.CandidateInformation.Qualifications)
                    .Any();
            candidateViewModel.WorkExperience =
                ApplicationConverter.GetWorkExperiencesViewModels(apprenticeshipApplicationDetail.CandidateInformation.WorkExperience);
            candidateViewModel.HasWorkExperience =
                ApplicationConverter.GetWorkExperiencesViewModels(apprenticeshipApplicationDetail.CandidateInformation.WorkExperience)
                    .Any();
            candidateViewModel.EmployerQuestionAnswers = new EmployerQuestionAnswersViewModel
            {
                CandidateAnswer1 = apprenticeshipApplicationDetail.AdditionalQuestion1Answer,
                CandidateAnswer2 = apprenticeshipApplicationDetail.AdditionalQuestion2Answer
            };

            return candidateViewModel;
        }
    }
}