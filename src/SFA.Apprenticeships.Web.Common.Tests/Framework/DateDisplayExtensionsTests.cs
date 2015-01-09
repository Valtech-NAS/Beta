namespace SFA.Apprenticeships.Web.Common.Tests.Framework
{
    using System;
    using Common.Framework;
    using NUnit.Framework;

    [TestFixture]
    public class DateDisplayExtensionsTests
    {
        [TestCase(0, "today")]
        [TestCase(1, "tomorrow")]
        [TestCase(2, "in 2 days")]
        [TestCase(3, "in 3 days")]
        [TestCase(4, "in 4 days")]
        [TestCase(5, "in 5 days")]
        [TestCase(6, "in 6 days")]
        [TestCase(7, "in 7 days")]
        public void FriendlyClosingWeekWithinTheWeek(int daysToClosing, string expectedOutput)
        {
            var closingDate = DateTime.Now.Date.AddDays(daysToClosing);
            var friendlyClosingDate = closingDate.ToFriendlyClosingWeek();
            Assert.AreEqual(friendlyClosingDate, expectedOutput);
        }

        [Test]
        public void FriendlyClosingWeekGreaterThanAWeekInTheFuture()
        {
            var closingDate = DateTime.Now.Date.AddDays(10);
            var friendlyClosingDate = closingDate.ToFriendlyClosingWeek();
            Assert.AreEqual(friendlyClosingDate, closingDate.ToString("dd MMM yyyy"));
        }

        [Test]
        public void FriendlyClosingWeekInThePast()
        {
            var closingDate = DateTime.Now.Date.AddDays(-10);
            var friendlyClosingDate = closingDate.ToFriendlyClosingWeek();
            Assert.AreEqual(friendlyClosingDate, closingDate.ToString("dd MMM yyyy"));
        }

        [TestCase(-20, false)]
        [TestCase(-1, false)]
        [TestCase(0, true)]
        [TestCase(1, false)]
        [TestCase(20, false)]
        public void FriendlyClosingToday(int daysToClosing, bool showToday)
        {
            var closingDate = DateTime.Now.Date.AddDays(daysToClosing);
            var friendlyClosingDate = closingDate.ToFriendlyClosingToday();
            Assert.AreEqual(friendlyClosingDate, showToday ? "today" : closingDate.ToString("dd MMM yyyy"));
        }
    }
}
