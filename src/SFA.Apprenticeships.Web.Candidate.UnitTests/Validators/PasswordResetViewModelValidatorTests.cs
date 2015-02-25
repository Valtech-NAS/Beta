namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Validators
{
    using Candidate.ViewModels.Register;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Candidate.Validators;

    [TestFixture]
    public class PasswordResetViewModelValidatorTests
    {
        [TestCase("Password1")]
        [TestCase("Password1$%")]
        public void ShouldNotHaveErrorsWhenPasswordComplexitySatisfied(string password)
        {
            // Arrange.
            var viewModel = new PasswordResetViewModel { Password = password, ConfirmPassword = password };
            var validator = new PasswordResetViewModelClientValidator();

            // Act.
            validator.Validate(viewModel);

            // Assert.
            validator.ShouldNotHaveValidationErrorFor(x => x.Password, viewModel);
        }

        [TestCase("abc")]
        [TestCase("123")]
        [TestCase("%^$£&^$%123aadff01sdaf*&^")]
        [TestCase("password1")]
        public void ShouldHaveErrorsWhenPasswordComplexityNotSatisfied(string password)
        {
            // Arrange.
            var viewModel = new PasswordResetViewModel { Password = password, ConfirmPassword = password};
            var validator = new PasswordResetViewModelClientValidator();

            // Act.
            validator.Validate(viewModel);

            // Assert.
            validator.ShouldHaveValidationErrorFor(x => x.Password, viewModel);
        }

        [TestCase("?Password01!", "?Password02!")]
        public void ShouldHaveErrorWhenPasswordsDoNotMatch(string password, string confirmPassword)
        {
            // Arrange.
            var viewModel = new PasswordResetViewModel
            {
                Password = password,
                ConfirmPassword = confirmPassword
            };

            var validator = new PasswordResetViewModelServerValidator();

            // Act.
            validator.Validate(viewModel);

            // Assert.
            validator.ShouldHaveValidationErrorFor(x => x.Password, viewModel);
        }

        [TestCase("?Password01!")]
        public void ShouldNotHaveErrorWhenPasswordsMatch(string password)
        {
            // Arrange.
            var viewModel = new PasswordResetViewModel
            {
                Password = password,
                ConfirmPassword = password
            };

            var validator = new PasswordResetViewModelServerValidator();

            validator.Validate(viewModel);

            // Assert.
            validator.ShouldNotHaveValidationErrorFor(x => x.Password, viewModel);
        }

        [TestCase("ABC123")]
        [TestCase("123abc")]
        [TestCase("123456")]
        [TestCase("abcdef")]
        public void ShouldNotHaveErrorWhenValidPasswordResetCodeIsSpecified(string passwordResetCode)
        {
            // Arrange.
            var viewModel = new PasswordResetViewModel
            {
                PasswordResetCode = passwordResetCode
            };

            var validator = new PasswordResetViewModelClientValidator();

            // Act.
            validator.Validate(viewModel);

            // Assert.
            validator.ShouldNotHaveValidationErrorFor(x => x.PasswordResetCode, viewModel);
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("ABC12")]
        [TestCase("ABC1234")]
        [TestCase("<SCRIP")]
        public void ShouldHaveErrorWhenInvalidPasswordResetCodeIsSpecified(string passwordResetCode)
        {
            // Arrange.
            var viewModel = new PasswordResetViewModel
            {
                PasswordResetCode = passwordResetCode
            };

            var validator = new PasswordResetViewModelClientValidator();

            // Act.
            validator.Validate(viewModel);

            // Assert.
            validator.ShouldHaveValidationErrorFor(x => x.PasswordResetCode, viewModel);
        }
    }
}
