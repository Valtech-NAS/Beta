namespace SFA.Apprenticeships.Application.Interfaces.Communications
{
    public enum MessageTypes
    {
        SendActivationCode,
        SendPasswordResetCode,
        SendAccountUnlockCode,
        SendMobileVerificationCode,
        ApprenticeshipApplicationSubmitted,
        TraineeshipApplicationSubmitted,
        PasswordChanged,
        DailyDigest,
        CandidateContactMessage
    }
}
