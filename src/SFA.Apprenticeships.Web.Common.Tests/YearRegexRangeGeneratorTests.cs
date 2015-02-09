namespace SFA.Apprenticeships.Web.Common.Tests
{
    using System.Text.RegularExpressions;
    using FluentAssertions;
    using NUnit.Framework;
    using SFA.Apprenticeships.Web.Common.Constants;

    [TestFixture]
    public class YearRegexRangeGeneratorTests
    {
        [Test]
        public void GetsRegexFor2014()
        {
            var regex = YearRegexRangeGenerator.GetRegex("2014");

            Regex.Match("1913", regex).Success.Should().BeFalse();
            Regex.Match("2015", regex).Success.Should().BeFalse();
            Regex.Match("1989", regex).Success.Should().BeTrue();
        }

        [Test]
        public void GetsRegexFor2024()
        {
            var regex = YearRegexRangeGenerator.GetRegex("2024");

            Regex.Match("1923", regex).Success.Should().BeFalse();
            Regex.Match("2025", regex).Success.Should().BeFalse();
            Regex.Match("2015", regex).Success.Should().BeTrue();
            Regex.Match("1989", regex).Success.Should().BeTrue();
        }
    }
}