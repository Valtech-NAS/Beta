namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.Settings
{
    using Builders;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class MarketingOptInTests
    {
        [TestCase(false, false)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(true, true)]
        public void US519_AC2_AC3_MarketingPreferences(bool allowEmailMarketing, bool allowSmsMarketing)
        {
            var viewModel = new SettingsViewModelBuilder().SmsEnabled(true).AllowEmailMarketing(allowEmailMarketing).AllowSmsMarketing(allowSmsMarketing).Build();

            var result = new SettingsViewBuilder().With(viewModel).Render();

            var allowEmailMarketingCheckBox = result.GetElementbyId("AllowEmailMarketing");
            var allowSmsMarketingCheckBox = result.GetElementbyId("AllowSmsMarketing");

            allowEmailMarketingCheckBox.Should().NotBeNull();
            allowSmsMarketingCheckBox.Should().NotBeNull();

            allowEmailMarketingCheckBox.ParentNode.InnerText.Should().Be("Email");
            allowSmsMarketingCheckBox.ParentNode.InnerText.Should().Be("Text");

            if (allowEmailMarketing)
            {
                allowEmailMarketingCheckBox.Attributes["checked"].Should().NotBeNull();
            }
            else
            {
                allowEmailMarketingCheckBox.Attributes["checked"].Should().BeNull();
            }

            if (allowSmsMarketing)
            {
                allowSmsMarketingCheckBox.Attributes["checked"].Should().NotBeNull();
            }
            else
            {
                allowSmsMarketingCheckBox.Attributes["checked"].Should().BeNull();
            }
        }

        [TestCase(true)]
        [TestCase(false)]
        public void SmsEnabledFeatureToggle(bool smsEnabled)
        {
            var viewModel = new SettingsViewModelBuilder().SmsEnabled(smsEnabled).Build();

            var result = new SettingsViewBuilder().With(viewModel).Render();

            var allowEmailMarketingCheckBox = result.GetElementbyId("AllowEmailMarketing");
            var allowSmsMarketingCheckBox = result.GetElementbyId("AllowSmsMarketing");

            allowEmailMarketingCheckBox.Should().NotBeNull();
            allowEmailMarketingCheckBox.ParentNode.InnerText.Should().Be("Email");

            if (smsEnabled)
            {
                allowSmsMarketingCheckBox.Should().NotBeNull();
                allowSmsMarketingCheckBox.ParentNode.InnerText.Should().Be("Text");
            }
            else
            {
                allowSmsMarketingCheckBox.Should().BeNull();
            }
        }
    }
}