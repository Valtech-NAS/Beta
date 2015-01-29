namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.UnitTests.ReferenceData
{
    using FluentAssertions;
    using LegacyWebServices.ReferenceData;
    using NUnit.Framework;

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