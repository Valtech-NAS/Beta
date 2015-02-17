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
            //todo: 1.6: AllowEmailMarketing = false;
            //todo: 1.6: AllowMobileMarketing = false;
        }

        public bool AllowEmail { get; set; }

        public bool AllowMobile { get; set; }

        public string MobileVerificationCode { get; set; }

        public bool VerifiedMobile { get; set; }

        //todo: 1.6: public bool AllowEmailMarketing { get; set; }
        
        //todo: 1.6: public bool AllowMobileMarketing { get; set; }

        public bool AllowTraineeshipPrompts { get; set; }
    }
}
