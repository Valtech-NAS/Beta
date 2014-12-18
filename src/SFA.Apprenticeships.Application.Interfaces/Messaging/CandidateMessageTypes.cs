namespace SFA.Apprenticeships.Application.Interfaces.Messaging
{
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
