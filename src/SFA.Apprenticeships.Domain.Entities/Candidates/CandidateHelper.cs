namespace SFA.Apprenticeships.Domain.Entities.Candidates
{
    public static class CandidateHelper
    {
        public static bool MobileVerificationRequired(this Candidate candidate)
        {
            return candidate.CommunicationPreferences.AllowMobile && !candidate.CommunicationPreferences.VerifiedMobile;
        }
    }
}