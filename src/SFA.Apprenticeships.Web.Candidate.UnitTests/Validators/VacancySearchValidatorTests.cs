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
        private VacancySearchViewModelClientValidator _viewModelClientValidator;
        private VacancySearchViewModelLocationValidator _viewModelLocationValidator;

        [SetUp]
        public void Setup()
        {
            _viewModelClientValidator = new VacancySearchViewModelClientValidator();
            _viewModelLocationValidator = new VacancySearchViewModelLocationValidator();
        }

        [Test]
        public void ShouldHaveErrorWhenLocationIsEmpty()
        {
            var viewModel = new VacancySearchViewModel { Location = "", Latitude = 0.1d, Longitude = 1.0d };
            _viewModelClientValidator.ShouldHaveValidationErrorFor(x => x.Location, viewModel);
        }

        [Test]
        public void ShouldHaveErrorWhenLocationIsUnder3Characters()
        {
            var viewModel = new VacancySearchViewModel { Location = "xx", Latitude = 0.1d, Longitude = 1.0d };
            _viewModelClientValidator.ShouldHaveValidationErrorFor(x => x.Location, viewModel);
        }

        [Test]
        public void ShouldHaveErrorWhenLocationIsNotMatched()
        {
            var viewModel = new VacancySearchViewModel { Location = "Test" };
            _viewModelLocationValidator.ShouldHaveValidationErrorFor(x => x.Location, viewModel);
        }


        [Test]
        public void ShouldNotHaveErrorWhenLocationMatched()
        {
            var viewModel = new VacancySearchViewModel {Location = "Test", Latitude = 0.1d, Longitude = 1.0d};
            _viewModelClientValidator.ShouldNotHaveValidationErrorFor(x => x.Location, viewModel);
        }

        [Test]
        public void ValidateShouldReturnFalseWhenNoMatch()
        {
            var viewModel = new VacancySearchViewModel { Location = "", Latitude = 0.1d, Longitude = 1.0d };
            var test = _viewModelClientValidator.Validate(viewModel);

            test.IsValid.Should().BeFalse();
        }

        [Test]
        public void ValidateShouldReturnTrueWhenMatch()
        {
            var viewModel = new VacancySearchViewModel { Location = "Test", Latitude = 0.1d, Longitude = 1.0d };
            var test = _viewModelClientValidator.Validate(viewModel);

            test.IsValid.Should().BeTrue();
        }
    }
}
