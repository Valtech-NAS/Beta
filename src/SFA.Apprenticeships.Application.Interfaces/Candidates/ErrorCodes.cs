namespace SFA.Apprenticeships.Application.Interfaces.Candidates
{
    public static class ErrorCodes
    {
        //todo: use meaningful/consistent error code strings
        public const string UnknownCandidateError = "Candidate.UnknownCandidateError";
        public const string CandidateCreationError = "Candidate.CandidateCreationError";
        public const string LegacyCandidateStateError = "Candidate.LegacyCandidateStateError";
        public const string LegacyCandidateNotFoundError = "Candidate.LegacyCandidateNotFoundError";
    }
}
