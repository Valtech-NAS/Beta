namespace SFA.Apprenticeships.Web.Candidate.ViewModels.MyApplications
{
    public class TraineeshipPromptViewModel
    {
        public bool TraineeshipsFeatureActive { get; set; }

        public int UnsuccessfulApplicationsToShowTraineeshipsPrompt { get; set; }
 
        public bool AllowTraineeshipPrompts { get; set; }
    }
}