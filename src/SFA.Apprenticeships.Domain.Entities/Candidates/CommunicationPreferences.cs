namespace SFA.Apprenticeships.Domain.Entities.Candidates
{
    public class CommunicationPreferences
    {
        public CommunicationPreferences()
        {
            AllowMobile = false;
            AllowEmail = true;
            VerifiedMobile = false;
        }
        public bool AllowEmail { get; set; }

        public bool AllowMobile { get; set; }

        public bool VerifiedMobile { get; set; }
    }
}
