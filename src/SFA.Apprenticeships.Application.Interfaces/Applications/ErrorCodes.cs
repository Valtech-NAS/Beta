namespace SFA.Apprenticeships.Application.Interfaces.Applications
{
    public static class ErrorCodes
    {
        //todo: use meaningful strings here!
        public const string ApplicationNotFoundError = "Application001";
        public const string ApplicationInIncorrectStateError = "Application002";
        public const string ApplicationViewIdNotFound = "Application004";
        public const string ApplicationCreationError = "Application005";
        public const string ApplicationGatewayCreationError = "Application007";
        public const string ApplicationTypeMismatch = "Application008";
        public const string ApplicationDuplicatedError = "Application.Duplicated";
    }
}