namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.TraineeshipSearch
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Candidate.Mediators.Search;
    using Candidate.Providers;
    using Candidate.ViewModels.VacancySearch;
    using Common.Constants;
    using Common.Providers;
    using Constants;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Configuration;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class DetailsTests : TestsBase
    {
        private const string VacancyId = "1";
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

            response.AssertCode(TraineeshipSearchMediatorCodes.Details.VacancyNotFound, false);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(" 491802")]
        [TestCase("VAC000547307")]
        [TestCase("[[imgUrl]]")]
        public void GivenInvalidVacancyIdString_ThenVacancyNotFound(string vacancyId)
        {
            var mediator = GetMediator(null);
            var response = mediator.Details(vacancyId, null, null);

            response.AssertCode(TraineeshipSearchMediatorCodes.Details.VacancyNotFound, false);
        }

        [Test]
        public void VacancyUnavailable()
        {
            var vacancyDetailViewModel = new VacancyDetailViewModel
            {
                VacancyStatus = VacancyStatuses.Unavailable,
            };

            var mediator = GetMediator(vacancyDetailViewModel);
            var response = mediator.Details(VacancyId, null, null);

            response.AssertCode(TraineeshipSearchMediatorCodes.Details.VacancyNotFound, false);
        }

        [Test]
        public void VacancyHasError()
        {
            const string message = "The vacancy has an error";

            var vacancyDetailViewModel = new VacancyDetailViewModel
            {
                VacancyStatus = VacancyStatuses.Live,
                ViewModelMessage = message
            };

            var mediator = GetMediator(vacancyDetailViewModel);

            var response = mediator.Details(VacancyId, null, null);

            response.AssertMessage(TraineeshipSearchMediatorCodes.Details.VacancyHasError, message, UserMessageLevel.Warning, true);
        }

        [Test]
        public void Ok()
        {
            var vacancyDetailViewModel = new VacancyDetailViewModel
            {
                Id = int.Parse(VacancyId),
                VacancyStatus = VacancyStatuses.Live
            };

            var mediator = GetMediator(vacancyDetailViewModel);
            var response = mediator.Details(VacancyId, null, SearchReturnUrl);

            response.AssertCode(TraineeshipSearchMediatorCodes.Details.Ok, true);
            
            response.ViewModel.Distance.Should().Be(Distance);
            response.ViewModel.SearchReturnUrl.Should().Be(SearchReturnUrl);

            _userData.ContainsKey(CandidateDataItemNames.VacancyDistance).Should().BeTrue();
            _userData[CandidateDataItemNames.VacancyDistance].Should().Be(Distance);

            _userData.ContainsKey(CandidateDataItemNames.LastViewedVacancyId).Should().BeTrue();
            _userData[CandidateDataItemNames.LastViewedVacancyId].Should().Be(VacancyId.ToString(CultureInfo.InvariantCulture));
        }

        private Mock<IUserDataProvider> GetUserDataProvider()
        {
            var userDataProvider = new Mock<IUserDataProvider>();

            userDataProvider.Setup(p => p.Pop(
                It.Is<string>(s => s == CandidateDataItemNames.VacancyDistance)))
                .Returns(Distance);

            userDataProvider.Setup(p => p.Pop(
                It.Is<string>(s => s == CandidateDataItemNames.LastViewedVacancyId)))
                .Returns(VacancyId.ToString(CultureInfo.InvariantCulture));

            userDataProvider.Setup(p => p.Push(
                It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((key, value) => _userData.Add(key, value));

            return userDataProvider;
        }

        private ITraineeshipSearchMediator GetMediator(VacancyDetailViewModel vacancyDetailViewModel)
        {
            var configurationManager = new Mock<IConfigurationManager>();
            var searchProvider = new Mock<ISearchProvider>();
            var traineeshipVacancyDetailProvider = new Mock<ITraineeshipVacancyDetailProvider>();

            traineeshipVacancyDetailProvider.Setup(
                p => p.GetVacancyDetailViewModel(It.IsAny<Guid?>(), It.IsAny<int>())).Returns(vacancyDetailViewModel);

            var userDataProvider = GetUserDataProvider();

            return GetMediator(configurationManager.Object, searchProvider.Object, traineeshipVacancyDetailProvider.Object, userDataProvider.Object);
        }
    }
}