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
            MobileVerificationCode = string.Empty;
            AllowEmailMarketing = false;
            AllowMobileMarketing = false;
        }

        public bool AllowEmail { get; set; }

        public bool AllowMobile { get; set; }

        public string MobileVerificationCode { get; set; }

        public bool VerifiedMobile { get; set; }

        public bool AllowEmailMarketing { get; set; }
        
        public bool AllowMobileMarketing { get; set; }

        public bool AllowTraineeshipPrompts { get; set; }
    }
}
