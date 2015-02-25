namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Validators
{
    using Candidate.ViewModels.Login;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Candidate.Validators;

    [TestFixture]
    public class AccountUnlockViewModelValidatorTests
    {
        [TestCase("")]
        [TestCase(null)]
        
        public void ShouldHaveErrorWhenEmailAddressIsNotSpecified(string emailAddress)
        {
            // Arrange.
            var viewModel = new AccountUnlockViewModel
            {
                EmailAddress = emailAddress
            };

            var validator = new AccountUnlockViewModelClientValidator();

            // Act.
            validator.Validate(viewModel);

            // Assert.
            validator.ShouldHaveValidationErrorFor(x => x.EmailAddress, viewModel);
        }

        [TestCase("catalan.dave@buhby.es")]

        public void ShouldNotHaveErrorWhenEmailAddressIsSpecified(string emailAddress)
        {
            // Arrange.
            var viewModel = new AccountUnlockViewModel
            {
                EmailAddress = emailAddress
            };

            var validator = new AccountUnlockViewModelClientValidator();

            // Act.
            validator.Validate(viewModel);

            // Assert.
            validator.ShouldNotHaveValidationErrorFor(x => x.EmailAddress, viewModel);
        }

        [TestCase("ABC123")]
        [TestCase("123abe")]
        [TestCase("123456")]
        [TestCase("abcdef")]
        public void ShouldNotHaveErrorWhenValidAccountUnlockCodeIsSpecified(string accountUnlockCode)
        {
            // Arrange.
            var viewModel = new AccountUnlockViewModel
            {
                AccountUnlockCode = accountUnlockCode
            };

            var validator = new AccountUnlockViewModelClientValidator();

            // Act.
            validator.Validate(viewModel);

            // Assert.
            validator.ShouldNotHaveValidationErrorFor(x => x.AccountUnlockCode, viewModel);
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("abc12")]
        [TestCase("<SCRIP")]
        public void ShouldHaveErrorWhenAccountUnlockCodeIsInvalid(string accountUnlockCode)
        {
            // Arrange.
            var viewModel = new AccountUnlockViewModel
            {
                AccountUnlockCode = accountUnlockCode
            };

            var validator = new AccountUnlockViewModelClientValidator();

            // Act.
            validator.Validate(viewModel);

            // Assert.
            validator.ShouldHaveValidationErrorFor(x => x.AccountUnlockCode, viewModel);
        }
    }
}
