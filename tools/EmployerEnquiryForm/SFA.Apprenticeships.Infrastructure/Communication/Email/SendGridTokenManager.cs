namespace SFA.Apprenticeships.Infrastructure.Communication.Email
{
    using System;
    using Application.Interfaces.Communications;

    public static class SendGridTokenManager
    {
        const string TemplateTokenDelimiter = "-";

        public static string GetEmailTemplateTokenForCommunicationToken(CommunicationTokens key)
        {
            string emailTemplateToken;
            switch (key)
            {
                case CommunicationTokens.FullName:
                    emailTemplateToken = "FullName";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("key");
            }

            return string.Format("{0}{1}{0}", TemplateTokenDelimiter, emailTemplateToken);
        }
    }
}