namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Validators
{
    using Builders;
    using Candidate.Validators;
    using FluentValidation.TestHelper;
    using NUnit.Framework;

    [TestFixture]
    public class SettingsViewModelServerValidatorTests
    {
        [TestCase(false, false, null, false)]
        [TestCase(false, false, "", false)]
        [TestCase(false, false, "0123456789", true)]
        [TestCase(true, false, null, false)]
        [TestCase(true, false, "", false)]
        [TestCase(true, false, "0123456789", true)]
        [TestCase(false, true, null, false)]
        [TestCase(false, true, "", false)]
        [TestCase(false, true, "0123456789", true)]
        [TestCase(true, true, null, false)]
        [TestCase(true, true, "", false)]
        [TestCase(true, true, "0123456789", true)]
        public void US616_AC4_PhoneNumberRequired(bool allowEmailComms, bool allowSmsComms, string phoneNumber, bool expectValid)
        {
            var viewModel = new SettingsViewModelBuilder().PhoneNumber(phoneNumber).AllowEmailComms(allowEmailComms).AllowSmsComms(allowSmsComms).Build();

            var validator = new SettingsViewModelServerValidator();

            if (expectValid)
            {
                validator.ShouldNotHaveValidationErrorFor(vm => vm.PhoneNumber, viewModel);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(vm => vm.PhoneNumber, viewModel);
            }
        }
    }
}