namespace SFA.Apprenticeships.Domain.Entities.Applications
{
    public enum ApplicationStatuses
    {
        Unknown = 0,
        Draft = 10,         // created but not yet submitted
        Expired = 15,       // draft that passed the vacancy closing date and cannot be submitted
        Submitting = 20,    // user has submitted to be processed
        Submitted = 30,     // submitted and awaiting vacancy manager review
        InProgress = 40,    // reviewed by vacancy manager and being processed along with other applications
        Successful = 80,    // processed by vacancy manager and marked as successful
        Unsuccessful = 90   // processed by vacancy manager and marked as unsuccessful
    }
}
