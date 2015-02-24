namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.Traineeships
{
    using System;
    using Builders;
    using Constants.Pages;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class PatchApplicationViewModelTests
    {
        [Test]
        public void NullSavedViewModel()
        {
            var candidateId = Guid.NewGuid();
            var traineeshipApplicationProvider = new TraineeshipApplicationProviderBuilder().Build();

            var traineeshipViewModel = new TraineeshipApplicationViewModelBuilder().Build();

            var viewModel = traineeshipApplicationProvider.PatchApplicationViewModel(candidateId, null, traineeshipViewModel);

            viewModel.Should().NotBeNull();
            viewModel.ViewModelMessage.Should().Be(ApplicationPageMessages.SubmitApplicationFailed);
            viewModel.HasError().Should().BeTrue();
        }
    }
}