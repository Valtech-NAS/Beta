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

        [TestCase("krister.bone_@gmail.com")]
        [TestCase("krister_bone@gmail.com")]
        [TestCase("krister+bone@gmail.com")]
        [TestCase("krister*bone@gmail.number")]
        [TestCase("kristerbone@gmail.subdomain.number")]
        [TestCase("kristerbone@gmail.subsubdomain.subdomain.number")]
        public void ShouldNotHaveErrorWhenEmailAddressIsValid(string emailAddress)
        {
            var viewModel = new RegisterViewModel { EmailAddress = emailAddress };
            _viewModelClientValidator.ShouldNotHaveValidationErrorFor(x => x.EmailAddress, viewModel);
        }

        [TestCase("krister.bone_@gma_il.com")]
        [TestCase("krister.bone_@gmail.co7m")]
        [TestCase("krister.bone_@gmail.morethansevensuffix")]
        public void ShouldHaveErrorWhenEmailAddressIsInvalid(string emailAddress)
        {
            var viewModel = new RegisterViewModel { EmailAddress = emailAddress };
            _viewModelClientValidator.ShouldHaveValidationErrorFor(x => x.EmailAddress, viewModel);
        }
    }
}
