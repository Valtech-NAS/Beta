namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.Settings
{
    using Builders;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class SettingsTests : ViewUnitTest
    {
        [TestCase(true)]
        [TestCase(false)]
        public void ShowFindTraineeshipLink(bool shouldShow)
        {
            var viewModel = new SettingsViewModelBuilder().ShowTraineeshipsLink(shouldShow).ShowTraineeshipsPrompt(false).Build();

            var result = new SettingsViewBuilder().With(viewModel).Render();

            var findTraineeshipLink = result.GetElementbyId("find-traineeship-link");

            if (shouldShow)
            {
                findTraineeshipLink.Should().NotBeNull();
            }
            else
            {
                findTraineeshipLink.Should().BeNull();
            }
        }
    }
}