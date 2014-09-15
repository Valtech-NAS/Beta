namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Validators
{
    using Candidate.ViewModels.Register;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Candidate.Validators;

    [TestFixture]
    public class PasswordResetViewModelValidatorTests
    {
        private PasswordResetViewModelClientValidator _viewModelClientValidator;

        [SetUp]
        public void Setup()
        {
            _viewModelClientValidator = new PasswordResetViewModelClientValidator();
        }

        [TestCase("Password1")]
        [TestCase("Password1$%")]
        public void ShouldNotHaveErrorsWhenPasswordComplexitySatisfied(string password)
        {
            var viewModel = new PasswordResetViewModel { Password = password, ConfirmPassword = password};
            _viewModelClientValidator.ShouldNotHaveValidationErrorFor(x => x.Password, viewModel);
        }

        [TestCase("abc")]
        [TestCase("123")]
        [TestCase("%^$£&^$%123aadff01sdaf*&^")]
        [TestCase("password1")]
        public void ShouldHaveErrorsWhenPasswordComplexitySatisfied(string password)
        {
            var viewModel = new PasswordResetViewModel { Password = password, ConfirmPassword = password};
            _viewModelClientValidator.ShouldHaveValidationErrorFor(x => x.Password, viewModel);
        }
    }
}
