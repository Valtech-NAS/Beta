namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    using Common.Constants;

    public class MediatorResponseMessage
    {
        public string Text { get; set; }

        public UserMessageLevel Level { get; set; }
    }
}