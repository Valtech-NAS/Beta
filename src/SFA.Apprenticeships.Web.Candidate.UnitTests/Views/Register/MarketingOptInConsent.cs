namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.Register
{
    using Builders;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class MarketingOptInConsent
    {
        [Test]
        public void MarketingOptInPresent()
        {
            var viewModel = new RegisterViewModelBuilder().Build();
            var result = new IndexViewBuilder().With(viewModel).Render();

            var acceptUpdatesCheckBox = result.GetElementbyId("AcceptUpdates");
            acceptUpdatesCheckBox.Should().NotBeNull();
            acceptUpdatesCheckBox.Attributes["checked"].Should().NotBeNull();
            acceptUpdatesCheckBox.ParentNode.InnerText.Should().Be("I would like to receive the latest careers news and updates");
        }
    }
}