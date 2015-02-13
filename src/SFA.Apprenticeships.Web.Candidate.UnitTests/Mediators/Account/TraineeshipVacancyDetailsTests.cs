namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Account
{
    using System;
    using Candidate.Mediators.Account;
    using Candidate.Providers;
    using Candidate.ViewModels.VacancySearch;
    using Common.Constants;
    using Constants.Pages;
    using Domain.Entities.Vacancies;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class TraineeshipVacancyDetailsTests
    {
        [Test]
        public void VacancyStatusLiveTest()
        {
            var vacancyDetailViewModel = new VacancyDetailViewModel
            {
                VacancyStatus = VacancyStatuses.Live
            };

            var traineeshipVacancyDetailProvider = new Mock<ITraineeshipVacancyDetailProvider>();
            traineeshipVacancyDetailProvider.Setup(x => x.GetVacancyDetailViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(vacancyDetailViewModel);
            var accountMediator = new AccountMediatorBuilder().With(traineeshipVacancyDetailProvider).Build();

            var response = accountMediator.TraineeshipVacancyDetails(Guid.NewGuid(), 42);

            response.Code.Should().Be(AccountMediatorCodes.VacancyDetails.Available);
            response.Message.Should().BeNull();
        }

        [Test]
        public void VacancyStatusExpiredTest()
        {
            var vacancyDetailViewModel = new VacancyDetailViewModel
            {
                VacancyStatus = VacancyStatuses.Expired
            };

            var traineeshipVacancyDetailProvider = new Mock<ITraineeshipVacancyDetailProvider>();
            traineeshipVacancyDetailProvider.Setup(x => x.GetVacancyDetailViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(vacancyDetailViewModel);
            var accountMediator = new AccountMediatorBuilder().With(traineeshipVacancyDetailProvider).Build();

            var response = accountMediator.TraineeshipVacancyDetails(Guid.NewGuid(), 42);

            response.Code.Should().Be(AccountMediatorCodes.VacancyDetails.Available);
            response.Message.Should().BeNull();
        }

        [Test]
        public void VacancyStatusUnavailableTest()
        {
            var vacancyDetailViewModel = new VacancyDetailViewModel
            {
                VacancyStatus = VacancyStatuses.Unavailable
            };

            var traineeshipVacancyDetailProvider = new Mock<ITraineeshipVacancyDetailProvider>();
            traineeshipVacancyDetailProvider.Setup(x => x.GetVacancyDetailViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(vacancyDetailViewModel);
            var accountMediator = new AccountMediatorBuilder().With(traineeshipVacancyDetailProvider).Build();

            var response = accountMediator.TraineeshipVacancyDetails(Guid.NewGuid(), 42);

            response.Code.Should().Be(AccountMediatorCodes.VacancyDetails.Unavailable);
            response.Message.Should().NotBeNull();
            response.Message.Text.Should().Be(MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable);
            response.Message.Level.Should().Be(UserMessageLevel.Warning);
        }

        [Test]
        public void VacancyNotFoundTest()
        {
            var traineeshipVacancyDetailProvider = new Mock<ITraineeshipVacancyDetailProvider>();
            traineeshipVacancyDetailProvider.Setup(x => x.GetVacancyDetailViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(default(VacancyDetailViewModel));
            var accountMediator = new AccountMediatorBuilder().With(traineeshipVacancyDetailProvider).Build();

            var response = accountMediator.TraineeshipVacancyDetails(Guid.NewGuid(), 42);

            response.Code.Should().Be(AccountMediatorCodes.VacancyDetails.Unavailable);
            response.Message.Should().NotBeNull();
            response.Message.Text.Should().Be(MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable);
            response.Message.Level.Should().Be(UserMessageLevel.Warning);
        }

        [Test]
        public void ErrorTest()
        {
            var vacancyDetailViewModel = new VacancyDetailViewModel
            {
                ViewModelMessage = "Has error"
            };

            var traineeshipVacancyDetailProvider = new Mock<ITraineeshipVacancyDetailProvider>();
            traineeshipVacancyDetailProvider.Setup(x => x.GetVacancyDetailViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(vacancyDetailViewModel);
            var accountMediator = new AccountMediatorBuilder().With(traineeshipVacancyDetailProvider).Build();

            var response = accountMediator.TraineeshipVacancyDetails(Guid.NewGuid(), 42);

            response.Code.Should().Be(AccountMediatorCodes.VacancyDetails.Error);
            response.Message.Should().NotBeNull();
            response.Message.Text.Should().Be(vacancyDetailViewModel.ViewModelMessage);
            response.Message.Level.Should().Be(UserMessageLevel.Error);
        }
    }
}