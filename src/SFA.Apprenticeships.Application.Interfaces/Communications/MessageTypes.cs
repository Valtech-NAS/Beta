namespace SFA.Apprenticeships.Application.Interfaces.Communications
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
