namespace SFA.Apprenticeships.Domain.Entities.Applications
{
    public class ApprenticeshipApplicationSummary : ApplicationSummary
    {
        public ApplicationStatuses Status { get; set; }

        public string UnsuccessfulReason { get; set; }
    }
}