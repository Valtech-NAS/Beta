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
      
        [SetUp]
        public void Setup()
        {
            _viewModelClientValidator = new RegisterViewModelClientValidator();
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
    }
}
