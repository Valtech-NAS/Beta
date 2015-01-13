namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Validators
{
    using Candidate.Validators;
    using Candidate.ViewModels.VacancySearch;
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
    }
}