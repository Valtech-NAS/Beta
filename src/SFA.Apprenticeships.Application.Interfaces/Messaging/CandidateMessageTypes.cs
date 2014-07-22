namespace SFA.Apprenticeships.Application.Interfaces.Messaging
{
    using System;

    public enum CandidateMessageTypes
    {
        SendActivationCode,
        SendPasswordResetCode,
        SendAccountUnlockCode,
        ApplicationSubmitted,
        PasswordChanged
    }
}
