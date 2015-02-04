namespace SFA.Apprenticeships.Application.Interfaces.Candidates
{
    public static class ErrorCodes
    {
        //todo: use meaningful strings here!
        public const string UnknownCandidateError = "Candidate001";
        public const string CandidateCreationError = "Candidate002";
        public const string ActivateUserFailed = "ActivateUser.Failed";
        public const string ActivateUserInvalidCode = "ActivateUser.InvalidCode";
        public const string LegacyCandidateStateError = "LegacyCandidateState.Error";
        public const string LegacyCandidateNotFoundError = "Candidate003";
    }
}
