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
        [Test]
        public void ShouldNotHaveErrorsOnSaveWhenBlank()
        {
            var viewModel = new EducationViewModel();
            var viewModelSaveValidator = new EducationViewModelSaveValidator();
            viewModelSaveValidator.ShouldNotHaveValidationErrorFor(x => x.NameOfMostRecentSchoolCollege, viewModel);
            viewModelSaveValidator.ShouldNotHaveValidationErrorFor(x => x.FromYear, viewModel);
            viewModelSaveValidator.ShouldNotHaveValidationErrorFor(x => x.ToYear, viewModel);
        }

        [Test]
        public void ShouldHaveErrorsOnSaveWhenFromYearInTheFuture()
        {
            var viewModel = new EducationViewModel {FromYear = Convert.ToString(DateTime.Now.Year + 1)};
            new EducationViewModelSaveValidator().ShouldHaveValidationErrorFor(x => x.FromYear, viewModel);
        }

        [Test]
        public void ShouldHaveErrorsOnSaveWhenFromYearGreaterThanToYear()
        {
            var viewModel = new EducationViewModel { FromYear = "2000", ToYear = "1990" };
            new EducationViewModelSaveValidator().ShouldHaveValidationErrorFor(x => x.ToYear, viewModel);
        }

        [Test]
        public void ShouldHaveErrorsOnServerWhenBlank()
        {
            var viewModel = new EducationViewModel();
            var viewModelServerValidator = new EducationViewModelServerValidator();

            viewModelServerValidator.ShouldHaveValidationErrorFor(x => x.NameOfMostRecentSchoolCollege, viewModel);
            viewModelServerValidator.ShouldHaveValidationErrorFor(x => x.FromYear, viewModel);
            viewModelServerValidator.ShouldHaveValidationErrorFor(x => x.ToYear, viewModel);
        }

        [Test]
        public void ShouldHaveErrorsOnSaveWhenSchoolIsSet()
        {
            var viewModel = new EducationViewModel
            {
                NameOfMostRecentSchoolCollege = "A School"
            };
            var viewModelServerValidator = new EducationViewModelServerValidator();

            viewModelServerValidator.ShouldNotHaveValidationErrorFor(x => x.NameOfMostRecentSchoolCollege, viewModel);
            viewModelServerValidator.ShouldHaveValidationErrorFor(x => x.FromYear, viewModel);
            viewModelServerValidator.ShouldHaveValidationErrorFor(x => x.ToYear, viewModel);
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
            var viewModelServerValidator = new EducationViewModelServerValidator();

            viewModelServerValidator.ShouldNotHaveValidationErrorFor(x => x.NameOfMostRecentSchoolCollege, viewModel);
            viewModelServerValidator.ShouldHaveValidationErrorFor(x => x.FromYear, viewModel);
            viewModelServerValidator.ShouldHaveValidationErrorFor(x => x.ToYear, viewModel);
        }
    }
}
