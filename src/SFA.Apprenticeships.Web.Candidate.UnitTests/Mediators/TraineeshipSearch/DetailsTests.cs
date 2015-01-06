namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.TraineeshipSearch
{
    using System;
    using System.Collections.Generic;
    using Candidate.Mediators;
    using Candidate.Mediators.Traineeships;
    using Candidate.Providers;
    using Candidate.Validators;
    using Candidate.ViewModels.VacancySearch;
    using Common.Constants;
    using Common.Providers;
    using Domain.Interfaces.Configuration;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class DetailsTests
    {
        private const int VacancyId = 1;
        private const string Distance = "42";
        private const string SearchReturnUrl = "http://www.example.com";

        private Dictionary<string, string> _userData;

        [SetUp]
        public void SetUp()
        {
            _userData = new Dictionary<string, string>();
        }

        [Test]
        public void VacancyNotFound()
        {
            var mediator = GetMediator(null);
            var response = mediator.Details(VacancyId, null, null);

            response.Code.Should().Be(Codes.TraineeshipSearch.Details.VacancyNotFound);
            response.ViewModel.Should().BeNull();
            response.Message.Should().BeNull();
            response.Parameters.Should().BeNull();
            response.ValidationResult.Should().BeNull();
        }

        [Test]
        public void VacancyHasError()
        {
            const string message = "The vacancy has an error";

            var vacancyDetailViewModel = new VacancyDetailViewModel { ViewModelMessage = message };
            var mediator = GetMediator(vacancyDetailViewModel);

            var response = mediator.Details(VacancyId, null, null);

            response.Code.Should().Be(Codes.TraineeshipSearch.Details.VacancyHasError);
            response.ViewModel.Should().NotBeNull();
            response.Message.Message.Should().Be(message);
            response.Message.Level.Should().Be(UserMessageLevel.Warning);
            response.Parameters.Should().BeNull();
            response.ValidationResult.Should().BeNull();
        }
        [Test]
        public void Ok()
        {
            var vacancyDetailViewModel = new VacancyDetailViewModel { Id = VacancyId };
            var mediator = GetMediator(vacancyDetailViewModel);
            var response = mediator.Details(VacancyId, null, SearchReturnUrl);

            response.Code.Should().Be(Codes.TraineeshipSearch.Details.Ok);
            response.ViewModel.Should().Be(vacancyDetailViewModel);
            response.Message.Should().BeNull();
            response.Parameters.Should().BeNull();
            response.ValidationResult.Should().BeNull();
            
            response.ViewModel.Distance.Should().Be(Distance);
            response.ViewModel.SearchReturnUrl.Should().Be(SearchReturnUrl);

            _userData.ContainsKey(UserDataItemNames.VacancyDistance).Should().BeTrue();
            _userData[UserDataItemNames.VacancyDistance].Should().Be(Distance);
            
            _userData.ContainsKey(UserDataItemNames.LastViewedVacancyId).Should().BeTrue();
            _userData[UserDataItemNames.LastViewedVacancyId].Should().Be(Convert.ToString(VacancyId));
        }

        private ITraineeshipSearchMediator GetMediator(VacancyDetailViewModel vacancyDetailViewModel)
        {
            var configurationManager = new Mock<IConfigurationManager>();
            var searchProvider = new Mock<ISearchProvider>();
            var traineeshipVacancyDetailProvider = new Mock<ITraineeshipVacancyDetailProvider>();

            traineeshipVacancyDetailProvider.Setup(
                p => p.GetVacancyDetailViewModel(It.IsAny<Guid?>(), It.IsAny<int>())).Returns(vacancyDetailViewModel);

            var userDataProvider = new Mock<IUserDataProvider>();

            userDataProvider.Setup(p => p.Pop(
                It.Is<string>(s => s == UserDataItemNames.VacancyDistance)))
                .Returns(Distance);

            userDataProvider.Setup(p => p.Pop(
                It.Is<string>(s => s == UserDataItemNames.LastViewedVacancyId)))
                .Returns(Convert.ToString(VacancyId));

            userDataProvider.Setup(p => p.Push(
                It.IsAny<string>(),
                It.IsAny<string>()))
                .Callback<string, string>((key, value) => _userData.Add(key, value));

            return GetMediator(configurationManager.Object, searchProvider.Object, traineeshipVacancyDetailProvider.Object, userDataProvider.Object);
        }

        private ITraineeshipSearchMediator GetMediator(IConfigurationManager configurationManager, ISearchProvider searchProvider, ITraineeshipVacancyDetailProvider traineeshipVacancyDetailProvider, IUserDataProvider userDataProvider)
        {
            return new TraineeshipSearchMediator(configurationManager, searchProvider, traineeshipVacancyDetailProvider, userDataProvider, new TraineeshipSearchViewModelClientValidator(), new TraineeshipSearchViewModelLocationValidator());
        }
    }
}