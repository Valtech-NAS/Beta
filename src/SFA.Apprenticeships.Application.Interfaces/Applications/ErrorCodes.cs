namespace SFA.Apprenticeships.Application.Interfaces.Applications
{
    public static class ErrorCodes
    {
        public const string ApplicationNotFoundError = "ApplicationNotFound";
        public const string ApplicationInIncorrectStateError = "ApplicationInIncorrectState";
        public const string ApplicationGatewayCreationError = "ApplicationGatewayCreation";
        public const string ApplicationTypeMismatch = "ApplicationTypeMismatch";
        public const string ApplicationDuplicatedError = "ApplicationDuplicated";
    }
}
