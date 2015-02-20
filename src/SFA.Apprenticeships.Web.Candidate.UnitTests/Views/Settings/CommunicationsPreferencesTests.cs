namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.Settings
{
    using Builders;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class CommunicationsPreferencesTests
    {
        [TestCase(false, false)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(true, true)]
        public void US616_AC2_AC3_CommunicationPreferences(bool allowEmailComms, bool allowSmsComms)
        {
            var viewModel = new SettingsViewModelBuilder().SmsEnabled(true).AllowEmailComms(allowEmailComms).AllowSmsComms(allowSmsComms).Build();

            var result = new SettingsViewBuilder().With(viewModel).Render();

            var allowEmailCommsCheckBox = result.GetElementbyId("AllowEmailComms");
            var allowSmsCommsCheckBox = result.GetElementbyId("AllowSmsComms");

            allowEmailCommsCheckBox.Should().NotBeNull();
            allowSmsCommsCheckBox.Should().NotBeNull();

            allowEmailCommsCheckBox.ParentNode.InnerText.Should().Be("Email");
            allowSmsCommsCheckBox.ParentNode.InnerText.Should().Be("Text");

            if (allowEmailComms)
            {
                allowEmailCommsCheckBox.Attributes["checked"].Should().NotBeNull();
            }
            else
            {
                allowEmailCommsCheckBox.Attributes["checked"].Should().BeNull();
            }

            if (allowSmsComms)
            {
                allowSmsCommsCheckBox.Attributes["checked"].Should().NotBeNull();
            }
            else
            {
                allowSmsCommsCheckBox.Attributes["checked"].Should().BeNull();
            }
        }

        [TestCase(true)]
        [TestCase(false)]
        public void US616_AC4_PhoneVerifiedIndication(bool verifiedMobile)
        {
            var viewModel = new SettingsViewModelBuilder().SmsEnabled(true).VerifiedMobile(verifiedMobile).Build();

            var result = new SettingsViewBuilder().With(viewModel).Render();

            var allowEmailCommsCheckBox = result.GetElementbyId("verifyContainer");

            if (verifiedMobile)
            {
                allowEmailCommsCheckBox.Should().NotBeNull();
                allowEmailCommsCheckBox.ChildNodes["span"].InnerText.Should().Be("Verified");
            }
            else
            {
                allowEmailCommsCheckBox.Should().BeNull();
            }
        }

        [Test]
        public void US616_EmailRadioButtonsRemoved()
        {
            var viewModel = new SettingsViewModelBuilder().SmsEnabled(true).Build();

            var result = new SettingsViewBuilder().With(viewModel).Render();

            var allowEmailCommsRadioButtonYes = result.GetElementbyId("AllowEmailComms-yes");
            var allowEmailCommsRadioButtonNo = result.GetElementbyId("AllowEmailComms-no");

            allowEmailCommsRadioButtonYes.Should().BeNull();
            allowEmailCommsRadioButtonNo.Should().BeNull();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void SmsEnabledFeatureToggle(bool smsEnabled)
        {
            var viewModel = new SettingsViewModelBuilder().SmsEnabled(smsEnabled).Build();

            var result = new SettingsViewBuilder().With(viewModel).Render();

            var allowEmailCommsCheckBox = result.GetElementbyId("AllowEmailComms");
            var allowSmsCommsCheckBox = result.GetElementbyId("AllowSmsComms");

            allowEmailCommsCheckBox.Should().NotBeNull();
            allowEmailCommsCheckBox.ParentNode.InnerText.Should().Be("Email");

            if (smsEnabled)
            {
                allowSmsCommsCheckBox.Should().NotBeNull();
                allowSmsCommsCheckBox.ParentNode.InnerText.Should().Be("Text");
            }
            else
            {
                allowSmsCommsCheckBox.Should().BeNull();
            }
        }
    }
}