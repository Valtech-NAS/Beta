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
            var viewModel = new SettingsViewModelBuilder().AllowEmailComms(allowEmailComms).AllowSmsComms(allowSmsComms).Build();

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
    }
}