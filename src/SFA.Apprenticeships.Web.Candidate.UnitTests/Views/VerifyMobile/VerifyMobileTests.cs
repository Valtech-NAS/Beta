namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.VerifyMobile
{
    using Builders;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class VerifyMobileTests : ViewUnitTest
    {
        [TestCase(true)]
        [TestCase(false)]
        public void ShowFindTraineeshipLink(bool shouldShow)
        {
            var viewModel = new VerifyMobileViewModelBuilder().ShowTraineeshipsLink(shouldShow).ShowTraineeshipsPrompt(false).Build();

            var result = new VerifyMobileViewBuilder().With(viewModel).Render();

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

        [Test]
        public void US616_AC5_VerifyMobilePageElements()
        {
            var viewModel = new VerifyMobileViewModelBuilder().Build();

            var result = new VerifyMobileViewBuilder().With(viewModel).Render();

            var phoneNumber = result.GetElementbyId("phoneNumber");
            var verifyMobileCode = result.GetElementbyId("VerifyMobileCode");

            phoneNumber.Should().NotBeNull();
            verifyMobileCode.Should().NotBeNull();
        }
    }
}