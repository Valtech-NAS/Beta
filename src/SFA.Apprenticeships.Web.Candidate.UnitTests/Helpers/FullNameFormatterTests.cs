namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Helpers
{
    using FluentAssertions;
    using NUnit.Framework;
    using Candidate.Helpers;

    [TestFixture]
    public class FullNameFormatterTests
    {
        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase(" ", " ")]
        [TestCase("Animal Technology (Lantra)", "Animal Technology")]
        [TestCase("Creative and Digital Media (Skillset)", "Creative and Digital Media")]
        public void TestFullNameFormatter(string fullName, string expectedFullName)
        {
            var formatted = FullNameFormatter.Format(fullName);
            formatted.Should().Be(expectedFullName);
        }
    }
}