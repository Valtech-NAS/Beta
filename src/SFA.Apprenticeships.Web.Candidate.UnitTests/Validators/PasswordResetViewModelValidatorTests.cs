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
            var viewModel = new PasswordResetViewModel { Password = password, ConfirmPassword = password};
            var viewModelClientValidator = new PasswordResetViewModelClientValidator();

            viewModelClientValidator.ShouldNotHaveValidationErrorFor(x => x.Password, viewModel);
        }

        [TestCase("abc")]
        [TestCase("123")]
        [TestCase("%^$£&^$%123aadff01sdaf*&^")]
        [TestCase("password1")]
        public void ShouldHaveErrorsWhenPasswordComplexitySatisfied(string password)
        {
            var viewModel = new PasswordResetViewModel { Password = password, ConfirmPassword = password};
            var viewModelClientValidator = new PasswordResetViewModelClientValidator();

            viewModelClientValidator.ShouldHaveValidationErrorFor(x => x.Password, viewModel);
        }

        [TestCase("?Password01!", "?Password02!")]
        public void ShouldHaveErrorWhenPasswordsDoNotMatch(string password, string confirmPassword)
        {
            var viewModel = new PasswordResetViewModel
            {
                Password = password,
                ConfirmPassword = confirmPassword
            };
            var viewModelServerValidator = new PasswordResetViewModelServerValidator();

            viewModelServerValidator.ShouldHaveValidationErrorFor(x => x.Password, viewModel);
        }

        [TestCase("?Password01!")]
        public void ShouldNotHaveErrorWhenPasswordsMatch(string password)
        {
            var viewModel = new PasswordResetViewModel
            {
                Password = password,
                ConfirmPassword = password
            };
            var viewModelServerValidator = new PasswordResetViewModelServerValidator();

            viewModelServerValidator.ShouldNotHaveValidationErrorFor(x => x.Password, viewModel);
        }
    }
}
