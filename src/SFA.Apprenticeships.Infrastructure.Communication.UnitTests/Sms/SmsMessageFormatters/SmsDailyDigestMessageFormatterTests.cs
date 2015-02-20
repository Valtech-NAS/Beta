namespace SFA.Apprenticeships.Infrastructure.Communication.UnitTests.Sms.SmsMessageFormatters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Communications;
    using Builder;
    using Domain.Entities.Communication;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class SmsDailyDigestMessageFormatterTests
    {
        private const string MessageTemplate = "You've {0} saved applications for apprenticeships that are due to close soon. They are:\n{1}\nYou can check the status of all your applications in the my applications section of your account.";
        private const char Pipe = '|';
        private const char Tilda = '~';
        private const int MaxDraftCount = 3;

        [Test]
        public void GivenSingleExpiringDraft()
        {
            var expiringDrafts = new ExpiringApprenticeshipApplicationDraftsBuilder().WithExpiringDrafts(1).Build();
            var smsRequest = new DailyDigestSmsRequestBuilder().WithExpiringDrafts(expiringDrafts).Build();
            var formatter = new SmsDailyDigestMessageFormatterBuilder().WithMessageTemplate(MessageTemplate).Build();

            var message = formatter.GetMessage(smsRequest.Tokens);

            int draftCount;
            int draftLineCount;
            var expectedMessage = GetExpectedMessage(expiringDrafts, out draftCount, out draftLineCount);
            message.Should().Be(expectedMessage);
            draftCount.Should().Be(1);
            draftLineCount.Should().Be(1);
        }

        [TestCase(2)]
        [TestCase(10)]
        [TestCase(11)]
        public void GivenMultipleExpiringDrafts(int noOfDrafts)
        {
            var expiringDrafts = new ExpiringApprenticeshipApplicationDraftsBuilder().WithExpiringDrafts(noOfDrafts).Build();
            var smsRequest = new DailyDigestSmsRequestBuilder().WithExpiringDrafts(expiringDrafts).Build();
            var formatter = new SmsDailyDigestMessageFormatterBuilder().WithMessageTemplate(MessageTemplate).Build();

            var message = formatter.GetMessage(smsRequest.Tokens);

            int draftCount;
            int draftLineCount;
            var expectedMessage = GetExpectedMessage(expiringDrafts, out draftCount, out draftLineCount);
            message.Should().Be(expectedMessage);
            draftCount.Should().Be(noOfDrafts);
            draftLineCount.Should().BeLessOrEqualTo(MaxDraftCount);
        }

        [TestCase(2)]
        [TestCase(10)]
        [TestCase(11)]
        public void GivenMultipleExpiringDraftsSpecialCharacters(int noOfDrafts)
        {
            var expiringDrafts = new ExpiringApprenticeshipApplicationDraftsBuilder().WithSpecialCharacterExpiringDrafts(noOfDrafts).Build();
            var smsRequest = new DailyDigestSmsRequestBuilder().WithExpiringDrafts(expiringDrafts).Build();
            var formatter = new SmsDailyDigestMessageFormatterBuilder().WithMessageTemplate(MessageTemplate).Build();

            var message = formatter.GetMessage(smsRequest.Tokens);

            int draftCount;
            int draftLineCount;
            var expectedMessage = GetExpectedMessage(expiringDrafts, out draftCount, out draftLineCount);
            message.Should().Be(expectedMessage);
            draftCount.Should().Be(noOfDrafts);
            draftLineCount.Should().BeLessOrEqualTo(MaxDraftCount);
        }

        [Test]
        public void GivenMultipleExpiringDrafts_ThenOrderedByClosingDate()
        {
            var expiringDrafts = new ExpiringApprenticeshipApplicationDraftsBuilder().WithExpiringDrafts(3).Build();
            expiringDrafts[0].ClosingDate = new DateTime(2015, 02, 01);
            expiringDrafts[1].ClosingDate = new DateTime(2015, 01, 01);
            expiringDrafts[2].ClosingDate = new DateTime(2015, 04, 01);
            var smsRequest = new DailyDigestSmsRequestBuilder().WithExpiringDrafts(expiringDrafts).Build();

            //Assert the ascending ordering by ClosingDate of apprenticeships about to expire
            if (smsRequest.Tokens.Count() > 1)
            {
                var orderedList = ConvertToExpiringApprenticeshipApplicationDraftModel(smsRequest);

                Assert.That(orderedList, Is.Ordered.By("ClosingDate"));
            }
        }

        private static List<ExpiringApprenticeshipApplicationDraft> ConvertToExpiringApprenticeshipApplicationDraftModel(SmsRequest request)
        {
            var drafts = request.Tokens.First(t => t.Key == CommunicationTokens.ExpiringDrafts).Value;

            var list = new List<ExpiringApprenticeshipApplicationDraft>();
            foreach (var draft in drafts.Split(Tilda))
            {
                var closingDate = draft.Split(Pipe)[3];
                list.Add(new ExpiringApprenticeshipApplicationDraft { ClosingDate = Convert.ToDateTime(closingDate) });
            }

            return list;
        }

        private static string GetExpectedMessage(IEnumerable<ExpiringApprenticeshipApplicationDraft> expiringDrafts, out int draftCount, out int draftLineCount)
        {
            var lineItems = new List<string>();
            draftCount = 0;
            draftLineCount = 0;
            foreach (var expiringDraft in expiringDrafts)
            {
                draftCount++;
                if (draftCount <= MaxDraftCount)
                {
                    draftLineCount++;
                    var lineItem = string.Format("{0}) With {1}, closing date {2}", draftCount, expiringDraft.EmployerName, expiringDraft.ClosingDate.ToLongDateString());
                    lineItems.Add(lineItem);
                }
            }

            var expiringDraftsMessage = string.Join("\n", lineItems);
            return string.Format(MessageTemplate, draftCount, expiringDraftsMessage);
        }
    }
}