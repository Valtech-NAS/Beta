namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Applications
{
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Candidate;
    using SFA.Apprenticeships.Web.Common.Models.Application;

    public class TraineeshipApplicationViewModel : ApplicationViewModelBase
    {
        public TraineeshipCandidateViewModel Candidate { get; set; }

        public TraineeshipApplicationViewModel(string message, ApplicationViewModelStatus viewModelStatus)
            : base(message, viewModelStatus)
        {
        }

        public TraineeshipApplicationViewModel(string message) : base(message)
        {
        }

        public TraineeshipApplicationViewModel(ApplicationViewModelStatus viewModelStatus) : base(viewModelStatus)
        {
        }

        public TraineeshipApplicationViewModel()
        {
        }
    }
}