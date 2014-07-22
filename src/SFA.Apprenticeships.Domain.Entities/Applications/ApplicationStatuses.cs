namespace SFA.Apprenticeships.Domain.Entities.Applications
{
    public enum ApplicationStatuses
    {
        Unknown = 0,
        Draft = 10,         // created but not yet submitted
        Expired = 15,       // draft that passed the vacancy closing date and cannot be submitted
        Submitted = 20,     // submitted and awaiting vacancy manager review
        InProgress = 25,    // reviewed by vacancy manager and being processed along with other applications
        Successful = 90,    // processed by vacancy manager and marked as successful
        Unsuccessful = 95   // processed by vacancy manager and marked as unsuccessful
    }
}
