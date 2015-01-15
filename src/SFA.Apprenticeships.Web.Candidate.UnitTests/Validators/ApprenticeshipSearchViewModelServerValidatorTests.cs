namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Validators
{
    using Candidate.Validators;
    using Candidate.ViewModels.VacancySearch;
    using FluentAssertions;
    using FluentValidation.TestHelper;
    using NUnit.Framework;

    [TestFixture]
    public class ApprenticeshipSearchViewModelServerValidatorTests
    {
        [TestCase("")]
        [TestCase("b")]
        [TestCase("cv")]
        [TestCase(null)]
        public void LocationValidationFailedTests(string location)
        {
            var validator = new ApprenticeshipSearchViewModelServerValidator();
            var viewModel = new ApprenticeshipSearchViewModel { Location = location };
            validator.ShouldHaveValidationErrorFor(x => x.Location, viewModel);
        }
        
        [TestCase("b1")]
        [TestCase("cv1")]
        [TestCase("london")]
        public void LocationValidationSuccessfulTests(string location)
        {
            var validator = new ApprenticeshipSearchViewModelServerValidator();
            var viewModel = new ApprenticeshipSearchViewModel { Location = location };
            validator.ShouldNotHaveValidationErrorFor(x => x.Location, viewModel);
        }

        [TestCase("VAC000123456", true)]
        [TestCase("000123456", true)]
        [TestCase("123456", true)]
        [TestCase("VRN000123456", false)]
        [TestCase("chef", false)]
        [TestCase("12345", false)]
        public void LocationNotRequiredIfKeywordIsVacancyReferenceNumber(string keywords, bool expectIsValid)
        {
            var validator = new ApprenticeshipSearchViewModelServerValidator();
            var viewModel = new ApprenticeshipSearchViewModel { Keywords = keywords };
            if (expectIsValid)
            {
                validator.Validate(viewModel).IsValid.Should().BeTrue();
                validator.ShouldNotHaveValidationErrorFor(x => x.Location, viewModel);
                validator.ShouldNotHaveValidationErrorFor(x => x.Keywords, viewModel);
            }
            else
            {
                validator.Validate(viewModel).IsValid.Should().BeFalse();
                validator.ShouldHaveValidationErrorFor(x => x.Location, viewModel);
                validator.ShouldNotHaveValidationErrorFor(x => x.Keywords, viewModel);
            }
        }
    }
}