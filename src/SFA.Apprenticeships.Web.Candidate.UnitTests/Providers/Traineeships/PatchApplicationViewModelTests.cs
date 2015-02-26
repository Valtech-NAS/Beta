namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.Traineeships
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Builders;
    using Candidate.ViewModels.Candidate;
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

        [Test]
        public void QualificationChanges()
        {
            var candidateId = Guid.NewGuid();
            var traineeshipApplicationProvider = new TraineeshipApplicationProviderBuilder().Build();

            var savedTraineeshipViewModel = new TraineeshipApplicationViewModelBuilder().Build();
            var qualifications = new List<QualificationsViewModel> {new QualificationsViewModelBuilder().WithSubject("Maths").WithGrade("C").WithYear("2015").Build()};
            var traineeshipViewModel = new TraineeshipApplicationViewModelBuilder().WithQualifications(qualifications).Build();

            var viewModel = traineeshipApplicationProvider.PatchApplicationViewModel(candidateId, savedTraineeshipViewModel, traineeshipViewModel);

            viewModel.Should().NotBeNull();
            viewModel.ViewModelMessage.Should().BeNull();
            viewModel.HasError().Should().BeFalse();
            viewModel.Candidate.HasQualifications.Should().BeTrue();
            viewModel.Candidate.Qualifications.Should().Equal(qualifications);
        }

        [Test]
        public void WorkExperienceChanges()
        {
            var candidateId = Guid.NewGuid();
            var traineeshipApplicationProvider = new TraineeshipApplicationProviderBuilder().Build();

            var savedTraineeshipViewModel = new TraineeshipApplicationViewModelBuilder().Build();
            var workExperience = new List<WorkExperienceViewModel> { new WorkExperienceViewModelBuilder().WithDescription("Work").WithEmployer("Employer").Build() };
            var traineeshipViewModel = new TraineeshipApplicationViewModelBuilder().WithWorkExperience(workExperience).Build();

            var viewModel = traineeshipApplicationProvider.PatchApplicationViewModel(candidateId, savedTraineeshipViewModel, traineeshipViewModel);

            viewModel.Should().NotBeNull();
            viewModel.ViewModelMessage.Should().BeNull();
            viewModel.HasError().Should().BeFalse();
            viewModel.Candidate.HasWorkExperience.Should().BeTrue();
            viewModel.Candidate.WorkExperience.Should().Equal(workExperience);
        }
    }
}