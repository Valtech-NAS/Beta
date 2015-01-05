namespace SFA.Apprenticeships.Application.Interfaces.Messaging
{
    using System;

    public enum CommunicationTokens
    {
        CandidateFirstName,
        CandidateLastName,
        Username,
        ActivationCode,
        ActivationCodeExpiryDays,
        PasswordResetCode,
        PasswordResetCodeExpiryDays,
        AccountUnlockCode,
        AccountUnlockCodeExpiryDays,
        ApplicationVacancyTitle,
        ApplicationVacancyReference,
        ApplicationId,
        ProviderContact
    }
}
