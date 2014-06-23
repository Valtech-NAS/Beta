namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Validators
{
    using System.Web.Mvc;
    using FluentAssertions;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Candidate.Validators;
    using Candidate.ViewModels.VacancySearch;

    [TestFixture]
    public class VacancySearchValidatorTests
    {
        private VacancySearchValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new VacancySearchValidator();
        }

        [Test]
        public void ShouldHaveErrorWhenLocationIsEmpty()
        {
            var viewModel = new VacancySearchViewModel { Location = "", Latitude = 0.1d, Longitude = 1.0d };
            _validator.ShouldHaveValidationErrorFor(x => x.Location, viewModel);
        }

        [Test]
        public void ShouldHaveErrorWhenLocationIsUnder3Characters()
        {
            var viewModel = new VacancySearchViewModel { Location = "xx", Latitude = 0.1d, Longitude = 1.0d };
            _validator.ShouldHaveValidationErrorFor(x => x.Location, viewModel);
        }

        [Test]
        public void ShouldHaveErrorWhenLocationIsNotMatched()
        {
            var viewModel = new VacancySearchViewModel { Location = "Test" };
            _validator.ShouldHaveValidationErrorFor(x => x.Latitude, viewModel);
            _validator.ShouldHaveValidationErrorFor(x => x.Longitude, viewModel);
        }


        [Test]
        public void ShouldNotHaveErrorWhenLocationMatched()
        {
            var viewModel = new VacancySearchViewModel {Location = "Test", Latitude = 0.1d, Longitude = 1.0d};
            _validator.ShouldNotHaveValidationErrorFor(x => x.Location, viewModel);
        }

        [Test]
        public void ValidateShouldReturnFalseWhenNoMatch()
        {
            var viewModel = new VacancySearchViewModel { Location = "", Latitude = 0.1d, Longitude = 1.0d };
            var test = _validator.Validate(viewModel, new ModelStateDictionary());

            test.Should().BeFalse();
        }

        [Test]
        public void ValidateShouldReturnTrueWhenMatch()
        {
            var viewModel = new VacancySearchViewModel { Location = "Test", Latitude = 0.1d, Longitude = 1.0d };
            var test = _validator.Validate(viewModel, new ModelStateDictionary());

            test.Should().BeTrue();
        }
    }
}
