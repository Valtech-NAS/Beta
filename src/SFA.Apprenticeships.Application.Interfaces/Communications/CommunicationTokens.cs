namespace SFA.Apprenticeships.Application.Interfaces.Communications
{
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
        ProviderContact,
        CandidateEmailAddress,
        CandidateMobileNumber,
        TotalItems, //todo: these tokens are *email* substitution tokens and not data items - consider moving to dispatcher project
        Item1,
        Item2,
        Item3,
        Item4,
        Item5,
        Item6,
        Item7,
        Item8,
        Item9,
        Item10,
    }
}
