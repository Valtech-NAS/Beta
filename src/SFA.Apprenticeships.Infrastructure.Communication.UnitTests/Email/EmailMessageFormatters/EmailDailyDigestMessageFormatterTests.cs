namespace SFA.Apprenticeships.Infrastructure.Communication.UnitTests.Email.EmailMessageFormatters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Communications;
    using Builder;
    using Communication.Email.EmailMessageFormatters;
    using Domain.Entities.Communication;
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
        private const char Pipe = '|';
        private const char Tilda = '~';

        [Test]
        public void GivenSingleExpiringDraft()
        {
            var expiringDrafts = new ExpiringApprenticeshipApplicationDraftsBuilder().WithExpiringDrafts(1).Build();
            var emailRequest = new DailyDigestEmailRequestBuilder().WithExpiringDrafts(expiringDrafts).Build();
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
            var expiringDrafts = new ExpiringApprenticeshipApplicationDraftsBuilder().WithExpiringDrafts(noOfDrafts).Build();
            var emailRequest = new DailyDigestEmailRequestBuilder().WithExpiringDrafts(expiringDrafts).Build();
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
            var expiringDrafts = new ExpiringApprenticeshipApplicationDraftsBuilder().WithSpecialCharacterExpiringDrafts(noOfDrafts).Build();
            var emailRequest = new DailyDigestEmailRequestBuilder().WithExpiringDrafts(expiringDrafts).Build();
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
            var expiringDrafts = new ExpiringApprenticeshipApplicationDraftsBuilder().WithSpecialCharacterExpiringDrafts(noOfDrafts).Build();
            var emailRequest = new DailyDigestEmailRequestBuilder().WithExpiringDrafts(expiringDrafts).Build();
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
            var expiringDrafts = new ExpiringApprenticeshipApplicationDraftsBuilder().WithSpecialCharacterExpiringDrafts(noOfDrafts).Build();
            var emailRequest = new DailyDigestEmailRequestBuilder().WithExpiringDrafts(expiringDrafts).Build();
            List<SendGridMessageSubstitution> sendGridMessageSubstitutions;
            var sendGridMessage = GetSendGridMessage(out sendGridMessageSubstitutions);

            var emailMessageFormatter = new EmailDailyDigestMessageFormatter();
            emailMessageFormatter.PopulateMessage(emailRequest, sendGridMessage.Object);

            sendGridMessageSubstitutions.Any(s => s.ReplacementTag == ExpiryVacanciesCountTag).Should().BeTrue();
        }

        [Test]
        public void GivenMultipleExpiringDrafts_ThenOrderedByClosingDate()
        {
            var expiringDrafts = new ExpiringApprenticeshipApplicationDraftsBuilder().WithExpiringDrafts(3).Build();
            expiringDrafts[0].ClosingDate = new DateTime(2015, 02, 01);
            expiringDrafts[1].ClosingDate = new DateTime(2015, 01, 01);
            expiringDrafts[2].ClosingDate = new DateTime(2015, 04, 01);
            var emailRequest = new DailyDigestEmailRequestBuilder().WithExpiringDrafts(expiringDrafts).Build();

            //Assert the ascending ordering by ClosingDate of apprenticeships about to expire
            if (emailRequest.Tokens.Count() > 1)
            {
                var orderedList = ConvertToExpiringApprenticeshipApplicationDraftModel(emailRequest);

                Assert.That(orderedList, Is.Ordered.By("ClosingDate"));
            }
        }

        private static List<ExpiringApprenticeshipApplicationDraft> ConvertToExpiringApprenticeshipApplicationDraftModel(EmailRequest request)
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

        private static string GetExpectedInfoSubstitution(int noOfDrafts)
        {
            var drafts = new ExpiringApprenticeshipApplicationDraftsBuilder().WithExpiringDrafts(noOfDrafts).Build();
            return GetExpectedInfoSubstitution(drafts);
        }

        private static string GetExpectedInfoSubstitution(IEnumerable<ExpiringApprenticeshipApplicationDraft> expiringDrafts)
        {
            var lineItems = expiringDrafts.Select(d => string.Format("<li><a href=\"https://www.findapprenticeship.service.gov.uk/account/apprenticeshipvacancydetails/{0}\">{1} with {2}</a><br>Closing date: {3}</li>",d.VacancyId, d.Title, d.EmployerName, d.ClosingDate.ToLongDateString()));
            return string.Format("<ul>{0}</ul>", string.Join("", lineItems));
        }
    }
}
