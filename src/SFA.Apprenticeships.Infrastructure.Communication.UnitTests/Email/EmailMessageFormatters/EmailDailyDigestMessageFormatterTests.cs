namespace SFA.Apprenticeships.Infrastructure.Communication.UnitTests.Email.EmailMessageFormatters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Communications;
    using Application.Interfaces.Communications;
    using Communication.Email.EmailMessageFormatters;
    using Domain.Entities.Candidates;
    using Domain.Entities.Communication;
    using FizzWare.NBuilder;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using SendGrid;

    [TestFixture]
    public class EmailDailyDigestMessageFormatterTests
    {
        private const string ExpiryVacanciesCountTag = "-Expiry.Vacancies.Count-";
        private const string ExpiryVacanciesInfoTag = "-Expiry.Vacancies.Info-";
        private const string CandidateFirstNameTag = "-Candidate.FirstName-";
        private const string Pipe = "|";
        private const char Tilda = '~';

        [Test]
        public void GivenSingleExpiringDraft()
        {
            var emailRequest = GetEmailRequest(1);
            List<SendGridMessageSubstitution> sendGridMessageSubstitutions;
            var sendGridMessage = GetSendGridMessage(out sendGridMessageSubstitutions);

            var emailMessageFormatter = new EmailDailyDigestMessageFormatter();
            emailMessageFormatter.PopulateMessage(emailRequest, sendGridMessage.Object);

            var countSubstitution = sendGridMessageSubstitutions.Single(s => s.ReplacementTag == ExpiryVacanciesCountTag);
            countSubstitution.SubstitutionValues.Count.Should().Be(1);
            countSubstitution.SubstitutionValues.Single().Should().Be(EmailDailyDigestMessageFormatter.OneSavedApplicationAboutToExpire);
            sendGridMessageSubstitutions.Count(s => s.ReplacementTag == ExpiryVacanciesInfoTag).Should().Be(1);
            var infoSubstitution = sendGridMessageSubstitutions.Single(s => s.ReplacementTag == ExpiryVacanciesInfoTag);
            infoSubstitution.SubstitutionValues.Single().Should().Be(GetExpectedInfoSubstitution(1));
        }

        [TestCase(2)]
        [TestCase(10)]
        [TestCase(11)]
        public void GivenMultipleExpiringDrafts(int noOfDrafts)
        {
            var emailRequest = GetEmailRequest(noOfDrafts);
            List<SendGridMessageSubstitution> sendGridMessageSubstitutions;
            var sendGridMessage = GetSendGridMessage(out sendGridMessageSubstitutions);

            var emailMessageFormatter = new EmailDailyDigestMessageFormatter();
            emailMessageFormatter.PopulateMessage(emailRequest, sendGridMessage.Object);

            var countSubstitution = sendGridMessageSubstitutions.Single(s => s.ReplacementTag == ExpiryVacanciesCountTag);
            countSubstitution.SubstitutionValues.Count.Should().Be(1);
            countSubstitution.SubstitutionValues.Single().Should().Be(EmailDailyDigestMessageFormatter.MoreThanOneSaveApplicationAboutToExpire);
            sendGridMessageSubstitutions.Count(s => s.ReplacementTag == ExpiryVacanciesInfoTag).Should().Be(1);
            var infoSubstitution = sendGridMessageSubstitutions.Single(s => s.ReplacementTag == ExpiryVacanciesInfoTag);
            infoSubstitution.SubstitutionValues.Single().Should().Be(GetExpectedInfoSubstitution(noOfDrafts));
        }

        [TestCase(2)]
        [TestCase(10)]
        [TestCase(11)]
        public void GivenMultipleExpiringDraftsSpecialCharacters(int noOfDrafts)
        {
            var expiringDrafts = GetExpiringDraftsSpecialCharacters(noOfDrafts);
            var emailRequest = GetEmailRequest(expiringDrafts);
            List<SendGridMessageSubstitution> sendGridMessageSubstitutions;
            var sendGridMessage = GetSendGridMessage(out sendGridMessageSubstitutions);

            var emailMessageFormatter = new EmailDailyDigestMessageFormatter();
            emailMessageFormatter.PopulateMessage(emailRequest, sendGridMessage.Object);

            var countSubstitution = sendGridMessageSubstitutions.Single(s => s.ReplacementTag == ExpiryVacanciesCountTag);
            countSubstitution.SubstitutionValues.Count.Should().Be(1);
            countSubstitution.SubstitutionValues.Single().Should().Be(EmailDailyDigestMessageFormatter.MoreThanOneSaveApplicationAboutToExpire);
            sendGridMessageSubstitutions.Count(s => s.ReplacementTag == ExpiryVacanciesInfoTag).Should().Be(1);
            var infoSubstitution = sendGridMessageSubstitutions.Single(s => s.ReplacementTag == ExpiryVacanciesInfoTag);
            infoSubstitution.SubstitutionValues.Single().Should().Be(GetExpectedInfoSubstitution(expiringDrafts));
        }

        [TestCase(1)]
        [TestCase(2)]
        public void ShouldContainCandidateFirstNameSubstitution(int noOfDrafts)
        {
            var expiringDrafts = GetExpiringDraftsSpecialCharacters(noOfDrafts);
            var emailRequest = GetEmailRequest(expiringDrafts);
            List<SendGridMessageSubstitution> sendGridMessageSubstitutions;
            var sendGridMessage = GetSendGridMessage(out sendGridMessageSubstitutions);

            var emailMessageFormatter = new EmailDailyDigestMessageFormatter();
            emailMessageFormatter.PopulateMessage(emailRequest, sendGridMessage.Object);

            sendGridMessageSubstitutions.Any(s => s.ReplacementTag == CandidateFirstNameTag).Should().BeTrue();
        }

        [TestCase(1)]
        [TestCase(2)]
        public void ShouldContainExpiryVacanciesCountSubstitution(int noOfDrafts)
        {
            var expiringDrafts = GetExpiringDraftsSpecialCharacters(noOfDrafts);
            var emailRequest = GetEmailRequest(expiringDrafts);
            List<SendGridMessageSubstitution> sendGridMessageSubstitutions;
            var sendGridMessage = GetSendGridMessage(out sendGridMessageSubstitutions);

            var emailMessageFormatter = new EmailDailyDigestMessageFormatter();
            emailMessageFormatter.PopulateMessage(emailRequest, sendGridMessage.Object);

            sendGridMessageSubstitutions.Any(s => s.ReplacementTag == ExpiryVacanciesCountTag).Should().BeTrue();
        }

        public void GivenMultipleExpiringDrafts_ThenOrderedByClosingDate()
        {
            var drafts = GetExpiringDrafts(3);
            //todo:modify dates
            drafts[0].ClosingDate = new DateTime(2015, 02, 1);
            drafts[1].ClosingDate = new DateTime(2015, 01, 1);
            drafts[2].ClosingDate = new DateTime(2015, 04, 1);
            var emailRequest = GetEmailRequest(drafts);

            //Assert the asc ordering by ClosingDate of apprenticeships about to expire
            if (emailRequest.Tokens.Count() > 1)
            {
                var orderedList = ConvertToExpiringApprenticeshipApplicationDraftModel(emailRequest);

                Assert.That(orderedList, Is.Ordered.By("ClosingDate"));
            }
        }

        private List<ExpiringApprenticeshipApplicationDraft> ConvertToExpiringApprenticeshipApplicationDraftModel(EmailRequest request)
        {
            var drafts = request.Tokens.First(t => t.Key == CommunicationTokens.ExpiringDrafts).Value;

            List<ExpiringApprenticeshipApplicationDraft> list = new List<ExpiringApprenticeshipApplicationDraft>();
            foreach (var draft in drafts.Split(Tilda))
            {
                string closingDate;
                ExtractVacancyDataFrom(draft, out closingDate);
                list.Add(new ExpiringApprenticeshipApplicationDraft { ClosingDate = Convert.ToDateTime(closingDate) });
            }

            return list;
        }

        private void ExtractVacancyDataFrom(string line, out string closingDate)
        {
            closingDate = line.Split(new[] { Pipe }, StringSplitOptions.RemoveEmptyEntries)[2];
        }

        private static Mock<ISendGrid> GetSendGridMessage(out List<SendGridMessageSubstitution> sendGridMessageSubstitutions)
        {
            var sendGridMessage = new Mock<ISendGrid>();
            var substitutions = new List<SendGridMessageSubstitution>();
            sendGridMessage.Setup(m => m.AddSubstitution(It.IsAny<string>(), It.IsAny<List<string>>()))
                .Callback<string, List<string>>(
                    (rt, sv) => substitutions.Add(new SendGridMessageSubstitution(rt, sv)));

            sendGridMessageSubstitutions = substitutions;
            return sendGridMessage;
        }

        private string GetExpectedInfoSubstitution(int noOfDrafts)
        {
            var drafts = GetExpiringDrafts(noOfDrafts);
            return GetExpectedInfoSubstitution(drafts);
        }

        private static string GetExpectedInfoSubstitution(IEnumerable<ExpiringApprenticeshipApplicationDraft> expiringDrafts)
        {
            var lineItems = expiringDrafts.Select(d => string.Format("<li><a href=\"https://www.findapprenticeship.service.gov.uk/account/apprenticeshipvacancydetails/{0}\">{1} with {2}</a><br>Closing date: {3}</li>",d.VacancyId, d.Title, d.EmployerName, d.ClosingDate.ToLongDateString()));
            return string.Format("<ul>{0}</ul>", string.Join("", lineItems));
        }

        private static EmailRequest GetEmailRequest(int noOfDrafts)
        {
            var drafts = GetExpiringDrafts(noOfDrafts);
            return GetEmailRequest(drafts);
        }

        private static EmailRequest GetEmailRequest(List<ExpiringApprenticeshipApplicationDraft> expiringDrafts)
        {
            var candidate = new Candidate();
            var communicationRequest = CommunicationRequestFactory.GetCommunicationMessage(candidate, expiringDrafts);
            var emailRequest = new EmailRequest
            {
                MessageType = MessageTypes.DailyDigest,
                ToEmail = "test@test.com",
                Tokens = communicationRequest.Tokens
            };
            return emailRequest;
        }

        private static List<ExpiringApprenticeshipApplicationDraft> GetExpiringDrafts(int noOfDrafts)
        {
            var drafts = Builder<ExpiringApprenticeshipApplicationDraft>
                .CreateListOfSize(noOfDrafts)
                .All()
                .With(ed => ed.ClosingDate = new DateTime(2015, 01, 31))
                .Build().OrderBy(p => p.ClosingDate).ToList();
            return drafts;
        }

        private static List<ExpiringApprenticeshipApplicationDraft> GetExpiringDraftsSpecialCharacters(int noOfDrafts)
        {
            var drafts = Builder<ExpiringApprenticeshipApplicationDraft>
                .CreateListOfSize(noOfDrafts)
                .All()
                .With(ed => ed.Title = "Tit|e with sp~cial ch@r$ in \"t")
                .With(ed => ed.EmployerName = "\"Emp|ov~r N@m€\"")
                .With(ed => ed.ClosingDate = new DateTime(2015, 01, 31))
                .Build().ToList();
            return drafts;
        }

        private class SendGridMessageSubstitution
        {
            public SendGridMessageSubstitution(string replacementTag, List<string> substitutionValues)
            {
                ReplacementTag = replacementTag;
                SubstitutionValues = substitutionValues;
            }

            public string ReplacementTag { get; private set; }

            public List<string> SubstitutionValues { get; private set; }
        }
    }
}
