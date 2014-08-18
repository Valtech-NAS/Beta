namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Validators
{
    using Candidate.ViewModels.Candidate;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Candidate.Validators;

    [TestFixture]
    public class AboutYouViewModelValidatorTests
    {
        private AboutYouViewModelClientValidator _viewModelClientValidator;
        private AboutYouViewModelServerValidator _viewModelServerValidator;
        private AboutYouViewModelSaveValidator _viewModelSaveValidator;

        [SetUp]
        public void Setup()
        {
            _viewModelClientValidator = new AboutYouViewModelClientValidator();
            _viewModelSaveValidator = new AboutYouViewModelSaveValidator();
            _viewModelServerValidator = new AboutYouViewModelServerValidator();
        }

        [Test]
        public void ShouldNotHaveErrorsOnSaveWhenBlank()
        {
            var viewModel = new AboutYouViewModel();
            _viewModelSaveValidator.ShouldNotHaveValidationErrorFor(x => x.WhatAreYourStrengths, viewModel);
            _viewModelSaveValidator.ShouldNotHaveValidationErrorFor(x => x.WhatDoYouFeelYouCouldImprove, viewModel);
            _viewModelSaveValidator.ShouldNotHaveValidationErrorFor(x => x.WhatAreYourHobbiesInterests, viewModel);
            _viewModelSaveValidator.ShouldNotHaveValidationErrorFor(x => x.AnythingWeCanDoToSupportYourInterview, viewModel);
        }

        [Test]
        public void ShouldHaveErrorsOnServerWhenBlank()
        {
            var viewModel = new AboutYouViewModel { RequiresSupportForInterview = true };
            _viewModelServerValidator.ShouldHaveValidationErrorFor(x => x.WhatAreYourStrengths, viewModel);
            _viewModelServerValidator.ShouldHaveValidationErrorFor(x => x.WhatDoYouFeelYouCouldImprove, viewModel);
            _viewModelServerValidator.ShouldHaveValidationErrorFor(x => x.WhatAreYourHobbiesInterests, viewModel);
            _viewModelServerValidator.ShouldHaveValidationErrorFor(x => x.AnythingWeCanDoToSupportYourInterview, viewModel);
        }
    }
}
