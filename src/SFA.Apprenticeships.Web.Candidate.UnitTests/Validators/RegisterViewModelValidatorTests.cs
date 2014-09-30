namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Validators
{
    using Candidate.ViewModels.Register;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Candidate.Validators;

    [TestFixture]
    public class RegisterViewModelValidatorTests
    {
        private RegisterViewModelClientValidator _viewModelClientValidator;
        private RegisterViewModelServerValidator _viewModelServerValidator;

        [SetUp]
        public void Setup()
        {
            _viewModelClientValidator = new RegisterViewModelClientValidator();
            _viewModelServerValidator = new RegisterViewModelServerValidator();
        }

        [TestCase("Password1")]
        [TestCase("Password1$%")]
        public void ShouldNotHaveErrorsWhenPasswordComplexitySatisfied(string password)
        {
            var viewModel = new RegisterViewModel
            {
                Password = password,
                ConfirmPassword = password
            };

            _viewModelClientValidator.ShouldNotHaveValidationErrorFor(x => x.Password, viewModel);
        }

        [TestCase("abc")]
        [TestCase("123")]
        [TestCase("%^$£&^$%123aadff01sdaf*&^")]
        [TestCase("password1")]
        public void ShouldHaveErrorsWhenPasswordComplexitySatisfied(string password)
        {
            var viewModel = new RegisterViewModel
            {
                Password = password,
                ConfirmPassword = password
            };

            _viewModelClientValidator.ShouldHaveValidationErrorFor(x => x.Password, viewModel);
        }

        [TestCase("krister.bone_@gmail.com", "?Password01!")]
        [TestCase("krister_bone@gmail.com", "?Password01!")]
        [TestCase("krister+bone@gmail.com", "?Password01!")]
        [TestCase("krister*bone@gmail.number", "?Password01!")]
        [TestCase("kristerbone@gmail.subdomain.number", "?Password01!")]
        [TestCase("kristerbone@gmail.subsubdomain.subdomain.number", "?Password01!")]
        public void ShouldNotHaveErrorWhenEmailAddressIsValid(string emailAddress, string password)
        {
            var viewModel = new RegisterViewModel
            {
                EmailAddress = emailAddress,
                Password = password,
                ConfirmPassword = password
            };

            _viewModelClientValidator.ShouldNotHaveValidationErrorFor(x => x.EmailAddress, viewModel);
        }

        [TestCase("krister.bone_@gma_il.com", "?Password01!")]
        [TestCase("krister.bone_@gmail.co7m", "?Password01!")]
        [TestCase("krister.bone_@gmail.morethansevensuffix", "?Password01!")]
        public void ShouldHaveErrorWhenEmailAddressIsInvalid(string emailAddress, string password)
        {
            var viewModel = new RegisterViewModel
            {
                EmailAddress = emailAddress ,
                Password = password,
                ConfirmPassword = password
            };

            _viewModelClientValidator.ShouldHaveValidationErrorFor(x => x.EmailAddress, viewModel);
        }

        [TestCase("?Password01!", "?Password02!")]
        public void ShouldHaveErrorWhenPasswordsDoNotMatch(string password, string confirmPassword)
        {
            var viewModel = new RegisterViewModel
            {
                Password = password,
                ConfirmPassword = confirmPassword
            };

            _viewModelServerValidator.ShouldHaveValidationErrorFor(x => x.Password, viewModel);
        }

        [TestCase("?Password01!")]
        public void ShouldNotHaveErrorWhenPasswordsMatch(string password)
        {
            var viewModel = new RegisterViewModel
            {
                Password = password,
                ConfirmPassword = password
            };

            _viewModelServerValidator.ShouldNotHaveValidationErrorFor(x => x.Password, viewModel);
        }
    }
}
