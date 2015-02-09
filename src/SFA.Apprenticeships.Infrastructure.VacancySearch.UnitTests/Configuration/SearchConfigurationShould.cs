namespace SFA.Apprenticeships.Infrastructure.VacancySearch.UnitTests.Configuration
{
    using Common.IoC;
    using FluentAssertions;
    using IoC;
    using Logging.IoC;
    using NUnit.Framework;
    using StructureMap;
    using VacancySearch.Configuration;

    [TestFixture]
    public class SearchConfigurationShould
    {
        [Test]
        public void LoadWithValuesSetFromConfig()
        {
            var config = SearchConfiguration.Instance;

            config.Should().NotBeNull();
            config.UseJobTitleTerms.Should().BeTrue();
            config.SearchJobTitleField.Should().BeFalse();
            config.SearchDescriptionField.Should().BeFalse();
            config.SearchEmployerNameField.Should().BeTrue();

            config.SearchTermParameters.Should().NotBeNull();
            var jobFactors = config.SearchTermParameters.JobTitleFactors;
            jobFactors.Should().NotBeNull();
            jobFactors.Boost.Should().Be(1.1d);
            jobFactors.Fuzziness.Should().Be(1);
            jobFactors.FuzzyPrefix.Should().Be(2);
            jobFactors.MatchAllKeywords.Should().BeTrue();
            jobFactors.PhraseProximity.Should().Be(3);
            jobFactors.MinimumMatch.Should().Be("10");

            var descriptionFactors = config.SearchTermParameters.DescriptionFactors;
            descriptionFactors.Should().NotBeNull();
            descriptionFactors.Boost.Should().Be(1.2d);
            descriptionFactors.Fuzziness.Should().Be(4);
            descriptionFactors.FuzzyPrefix.Should().Be(5);
            descriptionFactors.MatchAllKeywords.Should().BeFalse();
            descriptionFactors.PhraseProximity.Should().Be(6);
            descriptionFactors.MinimumMatch.Should().Be("2<50");

            //Not in config so should all be defaults
            var employerFactors = config.SearchTermParameters.EmployerFactors;
            employerFactors.Should().NotBeNull();
            employerFactors.Boost.Should().NotHaveValue();
            employerFactors.Fuzziness.Should().NotHaveValue();
            employerFactors.FuzzyPrefix.Should().NotHaveValue();
            employerFactors.MatchAllKeywords.Should().BeFalse();
            employerFactors.PhraseProximity.Should().NotHaveValue();
            employerFactors.MinimumMatch.Should().BeNullOrWhiteSpace();
        }
    }
}
