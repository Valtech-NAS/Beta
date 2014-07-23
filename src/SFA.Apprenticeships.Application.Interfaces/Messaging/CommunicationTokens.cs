namespace SFA.Apprenticeships.Application.Interfaces.Messaging
{
    using System;

    public enum CommunicationTokens
    {
        CandidateFirstName,
        CandidateLastName,
        ActivationCode,
        ActivationCodeExpiryDays,
        PasswordResetCode,
        PasswordResetCodeExpiryDays,
        AccountUnlockCode,
        ApplicationVacancyTitle,
        ApplicationVacancyReference
    }
}
