namespace SFA.Apprenticeships.Domain.Entities.Candidates
{
    public class CommunicationPreferences
    {
        public CommunicationPreferences()
        {
            AllowMobile = false;
            AllowEmail = true;
            VerifiedMobile = false;
            AllowTraineeshipPrompts = true;
        }
        public bool AllowEmail { get; set; }

        public bool AllowMobile { get; set; }

        //todo: mobile verification number here?

        public bool VerifiedMobile { get; set; }

        public bool AllowTraineeshipPrompts { get; set; }

        //todo: marketing opt-in (for v1.6)
    }
}
