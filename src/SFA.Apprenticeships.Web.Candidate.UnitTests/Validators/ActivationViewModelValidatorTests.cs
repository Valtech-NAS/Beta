namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Validators
{
    using Candidate.Validators;
    using Candidate.ViewModels.Register;
    using FluentValidation.TestHelper;
    using NUnit.Framework;

    [TestFixture]
    public class ActivationViewModelValidatorTests
    {
        [TestCase("")]
        [TestCase(null)]
        [TestCase("ABC12")]
        [TestCase("<SCRIP")]

        public void ShouldHaveErrorWhenInvalidActivationCodeIsSpecified(string activationCode)
        {
            // Arrange.
            var viewModel = new ActivationViewModel
            {
                ActivationCode = activationCode
            };

            var validator = new ActivationViewModelClientValidator();

            // Act.
            validator.Validate(viewModel);

            // Assert.
            validator.ShouldHaveValidationErrorFor(x => x.ActivationCode, viewModel);
        }

        [TestCase("ABC123")]
        [TestCase("123abe")]
        [TestCase("123456")]
        [TestCase("abcdef")]
        public void ShouldNotHaveErrorWhenValidActivationCodeIsSpecified(string activationCode)
        {
            // Arrange.
            var viewModel = new ActivationViewModel
            {
                ActivationCode = activationCode
            };

            var validator = new ActivationViewModelClientValidator();

            // Act.
            validator.Validate(viewModel);

            // Assert.
            validator.ShouldNotHaveValidationErrorFor(x => x.ActivationCode, viewModel);
        }
    }
}
