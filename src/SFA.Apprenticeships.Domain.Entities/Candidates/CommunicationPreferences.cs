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
            //todo: 1.6: MobileVerificationCode = string.Empty;
            //todo: 1.6: AllowMarketing = false;
        }

        public bool AllowEmail { get; set; }

        public bool AllowMobile { get; set; }

        //todo: 1.6: public string MobileVerificationCode { get; set; }

        public bool VerifiedMobile { get; set; }

        //todo: 1.6: public bool AllowMarketing { get; set; }

        public bool AllowTraineeshipPrompts { get; set; }
    }
}
