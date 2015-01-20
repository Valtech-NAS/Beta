namespace SFA.Apprenticeships.Application.Interfaces.Messaging
{
    public enum MessageTypes
    {
        SendActivationCode,
        SendPasswordResetCode,
        SendAccountUnlockCode,
        ApprenticeshipApplicationSubmitted,
        TraineeshipApplicationSubmitted,
        PasswordChanged,
        DailyDigest
    }
}
