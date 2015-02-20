namespace SFA.Apprenticeships.Infrastructure.Communication.Sms.SmsMessageFormatters
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using Application.Interfaces.Communications;

    public class SmsDailyDigestMessageFormatter : SmsMessageFormatter
    {
        public const string TemplateName = "MessageTypes.DailyDigest";

        private const string ExpiringDraftSummaryFormat = "{0}) With {1}, closing date {2}";
        private const char Pipe = '|';
        private const char Tilda = '~';
        private const int MaxDraftCount = 3;

        public SmsDailyDigestMessageFormatter(ITwillioConfiguration configuration)
            : base(configuration)
        {
            Message = GetTemplateConfiguration(TemplateName).Message;
        }

        public override string GetMessage(IEnumerable<CommunicationToken> communicationTokens)
        {
            var tokens = communicationTokens as IList<CommunicationToken> ?? communicationTokens.ToList();

            var expiringDraftsCount = tokens.First(t => t.Key == CommunicationTokens.ExpiringDraftsCount).Value;
            var expiringDraftsString = tokens.First(t => t.Key == CommunicationTokens.ExpiringDrafts).Value;

            var sb = new StringBuilder();

            var expiringDrafts = expiringDraftsString.Split(Tilda);
            for (var i = 0; i < expiringDrafts.Length && i < MaxDraftCount; i++)
            {
                var expiringDraft = expiringDrafts[i];

                var expiringDraftSummary = GetExpiringDraftSummary(i + 1, expiringDraft);

                sb.Append(expiringDraftSummary);
                if (i < expiringDrafts.Length - 1 && i < MaxDraftCount - 1)
                {
                    sb.Append("\n");
                }
            }

            return string.Format(Message, expiringDraftsCount, sb);
        }

        private string GetExpiringDraftSummary(int count, string expiringDraft)
        {
            var expiringDraftComponents = expiringDraft.Split(Pipe);
            var companyName = WebUtility.UrlDecode(expiringDraftComponents[2]);
            var closingDate = expiringDraftComponents[3];
            return string.Format(ExpiringDraftSummaryFormat, count, companyName, closingDate);
        }
    }
}