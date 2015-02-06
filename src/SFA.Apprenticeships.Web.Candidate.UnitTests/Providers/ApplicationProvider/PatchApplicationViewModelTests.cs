namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.ApplicationProvider
{
    using System;
    using Builders;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class PatchApplicationViewModelTests : ApprenticeshipApplicationProviderTestsBase
    {
        [Test]
        public void GivenNullViewModels_ThenExceptionIsThrown()
        {
            Action patchApplicationViewModelAction = () => ApprenticeshipApplicationProvider.PatchApplicationViewModel(Guid.NewGuid(), null, null);
            patchApplicationViewModelAction.ShouldThrow<Exception>();
        }

        [Test]
        public void GivenCandidateDoesNotRequireSupportForInterview_ThenSupportMessageIsBlanked()
        {
            var savedModel = new ApprenticeshipApplicationViewModelBuilder().Build();
            var submittedModel = new ApprenticeshipApplicationViewModelBuilder()
                .DoesNotRequireSupportForInterview()
                .CanBeSupportedAtInterviewBy("Should be blanked")
                .Build();

            var patchedViewModel = ApprenticeshipApplicationProvider.PatchApplicationViewModel(Guid.NewGuid(), savedModel, submittedModel);
            patchedViewModel.Candidate.AboutYou.AnythingWeCanDoToSupportYourInterview.Should().BeNullOrEmpty();
        }

        [Test]
        public void GivenRequiresSupportForInterview_ThenSupportMessageIsRetained()
        {
            var savedModel = new ApprenticeshipApplicationViewModelBuilder().Build();
            const string anythingWeCanDoToSupportYourInterview = "Should be retained";
            var submittedModel = new ApprenticeshipApplicationViewModelBuilder()
                .RequiresSupportForInterview()
                .CanBeSupportedAtInterviewBy(anythingWeCanDoToSupportYourInterview)
                .Build();

            var patchedViewModel = ApprenticeshipApplicationProvider.PatchApplicationViewModel(Guid.NewGuid(), savedModel, submittedModel);
            patchedViewModel.Candidate.AboutYou.AnythingWeCanDoToSupportYourInterview.Should().Be(anythingWeCanDoToSupportYourInterview);
        }
    }
}