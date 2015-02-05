namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Validators
{
    using System;
    using Candidate.ViewModels.Candidate;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Candidate.Validators;

    [TestFixture]
    public class EducationViewModelValidatorTests
    {
        private EducationViewModelServerValidator _viewModelServerValidator;
        private EducationViewModelSaveValidator _viewModelSaveValidator;

        [SetUp]
        public void Setup()
        {
            _viewModelSaveValidator = new EducationViewModelSaveValidator();
            _viewModelServerValidator = new EducationViewModelServerValidator();
        }

        [Test]
        public void ShouldNotHaveErrorsOnSaveWhenBlank()
        {
            var viewModel = new EducationViewModel();
            _viewModelSaveValidator.ShouldNotHaveValidationErrorFor(x => x.NameOfMostRecentSchoolCollege, viewModel);
            _viewModelSaveValidator.ShouldNotHaveValidationErrorFor(x => x.FromYear, viewModel);
            _viewModelSaveValidator.ShouldNotHaveValidationErrorFor(x => x.ToYear, viewModel);
        }

        [Test]
        public void ShouldHaveErrorsOnSaveWhenFromYearInTheFuture()
        {
            var viewModel = new EducationViewModel {FromYear = (DateTime.Now.Year + 1).ToString()};
            _viewModelSaveValidator.ShouldHaveValidationErrorFor(x => x.FromYear, viewModel);
        }

        [Test]
        public void ShouldHaveErrorsOnSaveWhenFromYearGreaterThanToYear()
        {
            var viewModel = new EducationViewModel() { FromYear = "2000", ToYear = "1990" };
            _viewModelSaveValidator.ShouldHaveValidationErrorFor(x => x.ToYear, viewModel);
        }

        [Test]
        public void ShouldHaveErrorsOnServerWhenBlank()
        {
            var viewModel = new EducationViewModel();
            _viewModelServerValidator.ShouldHaveValidationErrorFor(x => x.NameOfMostRecentSchoolCollege, viewModel);
            _viewModelServerValidator.ShouldHaveValidationErrorFor(x => x.FromYear, viewModel);
            _viewModelServerValidator.ShouldHaveValidationErrorFor(x => x.ToYear, viewModel);
        }

        [Test]
        public void ShouldHaveErrorsOnSaveWhenSchoolIsSet()
        {
            var viewModel = new EducationViewModel
            {
                NameOfMostRecentSchoolCollege = "A School"
            };
            _viewModelServerValidator.ShouldNotHaveValidationErrorFor(x => x.NameOfMostRecentSchoolCollege, viewModel);
            _viewModelServerValidator.ShouldHaveValidationErrorFor(x => x.FromYear, viewModel);
            _viewModelServerValidator.ShouldHaveValidationErrorFor(x => x.ToYear, viewModel);
        }

        [Test]
        public void ShouldHaveErrorsOnSaveWhenDatesAreZero()
        {
            var viewModel = new EducationViewModel
            {
                NameOfMostRecentSchoolCollege = "A School",
                FromYear = "0",
                ToYear = "0"
            };
            _viewModelServerValidator.ShouldNotHaveValidationErrorFor(x => x.NameOfMostRecentSchoolCollege, viewModel);
            _viewModelServerValidator.ShouldHaveValidationErrorFor(x => x.FromYear, viewModel);
            _viewModelServerValidator.ShouldHaveValidationErrorFor(x => x.ToYear, viewModel);
        }
    }
}
