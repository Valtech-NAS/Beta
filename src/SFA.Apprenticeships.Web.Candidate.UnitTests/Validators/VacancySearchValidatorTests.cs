namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Validators
{
    using FluentAssertions;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Candidate.Validators;
    using Candidate.ViewModels.VacancySearch;

    [TestFixture]
    public class VacancySearchValidatorTests
    {
        [Test]
        public void ShouldHaveErrorWhenLocationIsEmpty()
        {
            var viewModel = new ApprenticeshipSearchViewModel { Location = "", Latitude = 0.1d, Longitude = 1.0d };
            var viewModelClientValidator = new ApprenticeshipSearchViewModelClientValidator();

            viewModelClientValidator.ShouldHaveValidationErrorFor(x => x.Location, viewModel);
        }

        [Test]
        public void ShouldHaveErrorWhenLocationIsUnder2Characters()
        {
            var viewModel = new ApprenticeshipSearchViewModel { Location = "x", Latitude = 0.1d, Longitude = 1.0d };
            var viewModelClientValidator = new ApprenticeshipSearchViewModelClientValidator();

            viewModelClientValidator.ShouldHaveValidationErrorFor(x => x.Location, viewModel);
        }

        [Test]
        public void ShouldHaveErrorWhenLocationIsNotMatched()
        {
            var viewModel = new ApprenticeshipSearchViewModel { Location = "Test" };
            var viewModelLocationValidator = new ApprenticeshipSearchViewModelLocationValidator();

            viewModelLocationValidator.ShouldHaveValidationErrorFor(x => x.Location, viewModel);
        }


        [Test]
        public void ShouldNotHaveErrorWhenLocationMatched()
        {
            var viewModel = new ApprenticeshipSearchViewModel {Location = "Test", Latitude = 0.1d, Longitude = 1.0d};
            var viewModelClientValidator = new ApprenticeshipSearchViewModelClientValidator();

            viewModelClientValidator.ShouldNotHaveValidationErrorFor(x => x.Location, viewModel);
        }

        [Test]
        public void ValidateShouldReturnFalseWhenNoMatch()
        {
            var viewModel = new ApprenticeshipSearchViewModel { Location = "", Latitude = 0.1d, Longitude = 1.0d };
            var viewModelClientValidator = new ApprenticeshipSearchViewModelClientValidator();

            var test = viewModelClientValidator.Validate(viewModel);

            test.IsValid.Should().BeFalse();
        }

        [Test]
        public void ValidateShouldReturnTrueWhenMatch()
        {
            var viewModel = new ApprenticeshipSearchViewModel { Location = "Test", Latitude = 0.1d, Longitude = 1.0d };
            var viewModelClientValidator = new ApprenticeshipSearchViewModelClientValidator();

            var test = viewModelClientValidator.Validate(viewModel);

            test.IsValid.Should().BeTrue();
        }
    }
}
