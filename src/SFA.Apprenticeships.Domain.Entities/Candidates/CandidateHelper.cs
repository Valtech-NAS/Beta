namespace SFA.Apprenticeships.Domain.Entities.Candidates
{
    public static class CandidateHelper
    {
        public static bool MobileVerificationRequired(this Candidate candidate)
        {
            var communicationPreferences = candidate.CommunicationPreferences;
            return (communicationPreferences.AllowMobile || communicationPreferences.AllowMobileMarketing) && !communicationPreferences.VerifiedMobile;
        }
    }
}