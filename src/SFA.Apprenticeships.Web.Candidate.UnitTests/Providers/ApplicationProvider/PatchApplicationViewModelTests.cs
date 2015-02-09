namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.ApplicationProvider
{
    using System;
    using Builders;
    using Candidate.ViewModels.Applications;
    using Candidate.ViewModels.Candidate;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class PatchApplicationViewModelTests
    {
        [Test]
        public void GivenNullViewModels_ThenExceptionIsThrown()
        {
            Action patchApplicationViewModelAction = () => new ApprenticeshipApplicationProviderBuilder().Build().PatchApplicationViewModel(Guid.NewGuid(), null, null);
            patchApplicationViewModelAction.ShouldThrow<Exception>();
        }

        [Test]
        public void GivenNullCandidateViewModel_ThenExceptionIsThrown()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = null
            };
            Action patchApplicationViewModelAction = () => new ApprenticeshipApplicationProviderBuilder().Build().PatchApplicationViewModel(Guid.NewGuid(), null, viewModel);
            patchApplicationViewModelAction.ShouldThrow<Exception>();
        }

        [Test]
        public void GivenNullAboutYouViewModel_ThenExceptionIsThrown()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel
                {
                    AboutYou = null
                }
            };
            Action patchApplicationViewModelAction = () => new ApprenticeshipApplicationProviderBuilder().Build().PatchApplicationViewModel(Guid.NewGuid(), null, viewModel);
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

            var patchedViewModel = new ApprenticeshipApplicationProviderBuilder().Build().PatchApplicationViewModel(Guid.NewGuid(), savedModel, submittedModel);
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

            var patchedViewModel = new ApprenticeshipApplicationProviderBuilder().Build().PatchApplicationViewModel(Guid.NewGuid(), savedModel, submittedModel);
            patchedViewModel.Candidate.AboutYou.AnythingWeCanDoToSupportYourInterview.Should().Be(anythingWeCanDoToSupportYourInterview);
        }
    }
}