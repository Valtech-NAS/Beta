namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Applications
{
    using System;
    using System.Configuration;
    using SFA.Apprenticeships.Domain.Entities.Applications;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Candidate;
    using SFA.Apprenticeships.Web.Common.Models.Application;

    [Serializable]
    public class ApprenticheshipApplicationViewModel : ApplicationViewModelBase
    {
        //Constants used on application form
        public string AutoSaveTimeInMiutes = ConfigurationManager.AppSettings["AutoSaveTimeInMinutes"];

        public ApplicationStatuses Status { get; set; }

        public ApprenticeshipCandidateViewModel Candidate { get; set; }

        public ApprenticheshipApplicationViewModel(string message, ApplicationViewModelStatus viewModelStatus)
            : base(message, viewModelStatus)
        {
        }

        public ApprenticheshipApplicationViewModel(string message) : base(message)
        {
        }

        public ApprenticheshipApplicationViewModel(ApplicationViewModelStatus viewModelStatus) : base(viewModelStatus)
        {
        }

        public ApprenticheshipApplicationViewModel()
        {
        }
    }
}