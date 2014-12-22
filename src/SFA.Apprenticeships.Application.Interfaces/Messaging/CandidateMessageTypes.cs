namespace SFA.Apprenticeships.Application.Interfaces.Messaging
{
    //todo: shouldn't be in this project - this is process specific so should be in the application layer
    public enum CandidateMessageTypes
    {
        SendActivationCode,
        SendPasswordResetCode,
        SendAccountUnlockCode,
        ApprenticeshipApplicationSubmitted,
        TraineeshipApplicationSubmitted,
        PasswordChanged
    }
}
