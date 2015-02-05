namespace SFA.Apprenticeships.Application.Interfaces.Candidates
{
    public static class ErrorCodes
    {
        //todo: use meaningful/consistent error code strings
        public const string UnknownCandidateError = "UnknownCandidate";
        public const string CandidateCreationError = "CandidateCreation";
        public const string LegacyCandidateStateError = "LegacyCandidateState";
        public const string LegacyCandidateNotFoundError = "LegacyCandidateNotFound";
    }
}
