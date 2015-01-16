namespace SFA.Apprenticeships.Infrastructure.Communication.IntegrationTests
{
    using System.Collections.Generic;
    using Application.Interfaces.Messaging;

    public static class TokenGenerator
    {

        private const string TestActivationCode = "ABC123";
        private const string TestToEmail = "valtechnas@gmail.com";

        public static IEnumerable<KeyValuePair<CommunicationTokens, string>> CreateActivationEmailTokens()
        {
            return new[]
            {
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.CandidateFirstName, "FirstName"),
                new KeyValuePair<CommunicationTokens, string>(
                    CommunicationTokens.ActivationCode, TestActivationCode),
                new KeyValuePair<CommunicationTokens, string>(
                    CommunicationTokens.Username, TestToEmail),
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.ActivationCodeExpiryDays,
                    " 30 days")
            };
        }

        public static IEnumerable<KeyValuePair<CommunicationTokens, string>> CreateAccountUnlockCodeTokens()
        {
            return new[]
            {
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.CandidateFirstName, "FirstName"),
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.Username, TestToEmail),
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.AccountUnlockCode, TestActivationCode),
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.AccountUnlockCodeExpiryDays,
                    " 1 day")
            };
        }

        public static IEnumerable<KeyValuePair<CommunicationTokens, string>> CreatePasswordResetConfirmationTokens()
        {
            return new[]
            {
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.CandidateFirstName, "FirstName"),
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.Username, TestToEmail)
            };
        }

        public static IEnumerable<KeyValuePair<CommunicationTokens, string>> CreatePasswordResetTokens()
        {
            return new[]
            {
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.CandidateFirstName, "FirstName"),
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.Username, TestToEmail),
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.PasswordResetCode, TestActivationCode),
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.PasswordResetCodeExpiryDays, "1 day")
            };
        }

        public static IEnumerable<KeyValuePair<CommunicationTokens, string>> CreateApprenticeshipApplicationSubmittedTokens()
        {
            return new[]
            {
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.CandidateFirstName, "FirstName"),
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.ApplicationVacancyTitle,
                    "Application Vacancy Title"),
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.ApplicationVacancyReference,
                    "Application Vacancy Reference")
            };
        }

        public static IEnumerable<KeyValuePair<CommunicationTokens, string>> CreateTraineeshipApplicationSubmittedTokens()
        {
            return new[]
            {
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.CandidateFirstName, "FirstName"),
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.ApplicationVacancyTitle,
                    "Application Vacancy Title"),
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.ApplicationVacancyReference,
                    "Application Vacancy Reference"),
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.ProviderContact,
                    "Provider Contact")
            };
        }
    }
}