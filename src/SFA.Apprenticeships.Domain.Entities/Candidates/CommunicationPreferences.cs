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

        //todo: 1.6: mobile verification number here?

        public bool VerifiedMobile { get; set; }

        public bool AllowTraineeshipPrompts { get; set; }

        //todo: 1.6: marketing opt-in
    }
}
