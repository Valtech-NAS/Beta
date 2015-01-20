namespace SFA.Apprenticeships.Infrastructure.Communication.Email
{
    using System;
    using Application.Interfaces.Messaging;

    public static class SendGridTokenManager
    {
        const string TemplateTokenDelimiter = "-";

        public static string GetEmailTemplateTokenForCommunicationToken(CommunicationTokens key)
        {
            string emailTemplateToken;
            switch (key)
            {
                case CommunicationTokens.CandidateFirstName:
                    emailTemplateToken = "Candidate.FirstName";
                    break;
                case CommunicationTokens.CandidateLastName:
                    emailTemplateToken = "Candidate.LastName";
                    break;
                case CommunicationTokens.Username:
                    emailTemplateToken = "Candidate.EmailAddress";
                    break;
                case CommunicationTokens.ActivationCode:
                    emailTemplateToken = "Candidate.ActivationCode";
                    break;
                case CommunicationTokens.ActivationCodeExpiryDays:
                    emailTemplateToken = "Candidate.ActivationCodeExpiryDays";
                    break;
                case CommunicationTokens.PasswordResetCode:
                    emailTemplateToken = "Candidate.PasswordResetCode";
                    break;
                case CommunicationTokens.PasswordResetCodeExpiryDays:
                    emailTemplateToken = "Candidate.PasswordResetCodeExpiryDays";
                    break;
                case CommunicationTokens.AccountUnlockCode:
                    emailTemplateToken = "Candidate.AccountUnlockCode";
                    break;
                case CommunicationTokens.ApplicationVacancyTitle:
                    emailTemplateToken = "Candidate.ApplicationVacancyTitle";
                    break;
                case CommunicationTokens.ApplicationVacancyReference:
                    emailTemplateToken = "Candidate.ApplicationVacancyReference";
                    break;
                case CommunicationTokens.ApplicationId:
                    emailTemplateToken = "Candidate.ApplicationId";
                    break; 
                case CommunicationTokens.AccountUnlockCodeExpiryDays:
                    emailTemplateToken = "Candidate.AccountUnlockCodeExpiryDays";
                    break;
                case CommunicationTokens.ProviderContact:
                    emailTemplateToken = "Provider.Contact";
                    break;
                case CommunicationTokens.TotalItems:
                    emailTemplateToken = "Expiry.Vacancies.Count";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("key");
            }

            return string.Format("{0}{1}{0}", TemplateTokenDelimiter, emailTemplateToken);
        }

        public static string VacancyAboutToExpireVacanciesInfoHtmlToken
        {
            get
            {
                return string.Format("{0}Expiry.Vacancies.Info.Html{0}", TemplateTokenDelimiter);
            }
        }

        public static string VacancyAboutToExpireVacanciesInfoTextToken
        {
            get
            {
                return string.Format("{0}Expiry.Vacancies.Info.Text{0}", TemplateTokenDelimiter);
            }
        }
    }
}