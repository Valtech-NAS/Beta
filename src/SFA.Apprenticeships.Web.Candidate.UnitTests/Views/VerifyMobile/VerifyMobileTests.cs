namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.VerifyMobile
{
    using Builders;
    using Candidate.ViewModels.Account;
    using FluentAssertions;
    using NUnit.Framework;

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

        //[TestCase(true)]
        //[TestCase(false)]
        //public void US616_AC4_PhoneVerifiedIndication(bool verifiedMobile)
        //{
        //    var viewModel = new VerifyMobileViewModelBuilder().Build();

        //    var result = new VerifyMobileViewBuilder().With(viewModel).Render();

        //    var allowEmailCommsCheckBox = result.GetElementbyId("verifyContainer");

        //    if (verifiedMobile)
        //    {
        //        allowEmailCommsCheckBox.Should().NotBeNull();
        //        allowEmailCommsCheckBox.ChildNodes["span"].InnerText.Should().Be("Verified");
        //    }
        //    else
        //    {
        //        allowEmailCommsCheckBox.Should().BeNull();
        //    }
        //}
    }
}