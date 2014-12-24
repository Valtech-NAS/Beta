namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    using Common.Constants;

    public class MediatorResponseMessage
    {
        public string Message { get; set; }

        public UserMessageLevel Level { get; set; }
    }
}