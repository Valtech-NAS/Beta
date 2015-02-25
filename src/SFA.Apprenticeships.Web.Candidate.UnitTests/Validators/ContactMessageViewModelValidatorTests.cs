namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Validators
{
    using Candidate.ViewModels.Home;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Candidate.Validators;

    [TestFixture]
    public class ContactMessageViewModelValidatorTests
    {
        [Test]
        public void ShouldHaveErrorsOnSendWhenBlank()
        {
            var viewModel = new ContactMessageViewModel();
            var viewModelValidator = new ContactMessageClientViewModelValidator();

            viewModelValidator.ShouldNotHaveValidationErrorFor(x => x.Details, viewModel);
            viewModelValidator.ShouldHaveValidationErrorFor(x => x.Name, viewModel);
            viewModelValidator.ShouldHaveValidationErrorFor(x => x.Enquiry, viewModel);
            viewModelValidator.ShouldHaveValidationErrorFor(x => x.Email, viewModel);
        }

        [Test]
        public void ShouldNotHaveErrorsOnSendWhenFilled()
        {
            const string aString = "SomeString";
            const string anEmail = "email@gmail.com";

            var viewModel = new ContactMessageViewModel
            {
                Name = aString,
                Email = anEmail,
                Enquiry = aString
            };

            var viewModelValidator = new ContactMessageClientViewModelValidator();

            viewModelValidator.ShouldNotHaveValidationErrorFor(x => x.Details, viewModel);
            viewModelValidator.ShouldNotHaveValidationErrorFor(x => x.Name, viewModel);
            viewModelValidator.ShouldNotHaveValidationErrorFor(x => x.Enquiry, viewModel);
            viewModelValidator.ShouldNotHaveValidationErrorFor(x => x.Email, viewModel);
        }
    }
}
