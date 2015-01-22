namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators
{
    using Candidate.Mediators;
    using Common.Providers;
    using Domain.Interfaces.Configuration;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SearchMediatorBaseTests : SearchMediatorBase
    {
        public SearchMediatorBaseTests() : base(new Mock<IConfigurationManager>().Object, new Mock<IUserDataProvider>().Object)
        {
        }

        [TestCase(null, 0, false)]
        [TestCase("", 0, false)]
        [TestCase(" ", 0, false)]
        [TestCase("491802", 491802, true)]
        [TestCase("491802 ", 491802, true)]
        [TestCase(" 491802", 0, false)]
        [TestCase("491802(Matt)", 491802, true)]
        [TestCase("484887(matt0)", 484887, true)]
        [TestCase("494651(both)", 494651, true)]
        [TestCase("482229 clsoing date March 2015", 482229, true)]
        [TestCase("484439yt", 484439, true)]
        [TestCase("491684--", 491684, true)]
        [TestCase("VAC000547307", 0, false)]
        [TestCase("488832apprenticeships", 488832, true)]
        [TestCase("447888f", 447888, true)]
        [TestCase("48830310 day weather forecast for lichfield", 48830310, true)]
        public void TryParseVacancyIdTests(string vacancyIdString, int expectedVacancyId, bool expectSuccess)
        {
            int vacancyId;
            var success = TryParseVacancyId(vacancyIdString, out vacancyId);
            Assert.AreEqual(expectedVacancyId, vacancyId);
            Assert.AreEqual(expectSuccess, success);
        }
    }
}