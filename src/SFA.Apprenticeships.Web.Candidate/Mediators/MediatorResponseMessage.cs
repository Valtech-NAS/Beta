namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    using Common.Constants;

    public class MediatorResponseMessage
    {
        public UserMessageLevel Level { get; set; }

        public string Message { get; set; }
    }
}