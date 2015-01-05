namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipSearch
{
    using System;
    using Candidate.Mediators;
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
        private const int Id = 1;

        [Test]
        public void VacancyNotFound()
        {
            var mediator = GetMediator(null);

            var response = mediator.Details(Id, null, null);

            response.Code.Should().Be(Codes.ApprenticeshipSearch.Details.VacancyNotFound);
            response.ViewModel.Should().BeNull();
            response.Message.Should().BeNull();
            response.Parameters.Should().BeNull();
        }

        [Test]
        public void VacancyHasError()
        {
            const string message = "The vacancy has an error";
            
            var vacancyDetailViewModel = new VacancyDetailViewModel {ViewModelMessage = message};
            var mediator = GetMediator(vacancyDetailViewModel);

            var response = mediator.Details(Id, null, null);

            response.Code.Should().Be(Codes.ApprenticeshipSearch.Details.VacancyHasError);
            response.ViewModel.Should().NotBeNull();
            response.Message.Message.Should().Be(message);
            response.Message.Level.Should().Be(UserMessageLevel.Warning);
            response.Parameters.Should().BeNull();
        }

        [Test]
        public void Ok()
        {
            var vacancyDetailViewModel = new VacancyDetailViewModel();
            var mediator = GetMediator(vacancyDetailViewModel);

            var response = mediator.Details(Id, null, null);

            response.Code.Should().Be(Codes.ApprenticeshipSearch.Details.Ok);
            response.ViewModel.Should().NotBeNull();
            response.Message.Should().BeNull();
            response.Parameters.Should().BeNull();
        }

        private static IApprenticeshipSearchMediator GetMediator(VacancyDetailViewModel vacancyDetailViewModel)
        {
            var configurationManager = new Mock<IConfigurationManager>();
            var searchProvider = new Mock<ISearchProvider>();
            var apprenticeshipVacancyDetailProvider = new Mock<IApprenticeshipVacancyDetailProvider>();
            apprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(It.IsAny<Guid?>(), It.IsAny<int>())).Returns(vacancyDetailViewModel);
            var userDataProvider = new Mock<IUserDataProvider>();
            var mediator = GetMediator(configurationManager.Object, searchProvider.Object, apprenticeshipVacancyDetailProvider.Object, userDataProvider.Object);
            return mediator;
        }

        private static IApprenticeshipSearchMediator GetMediator(IConfigurationManager configurationManager, ISearchProvider searchProvider, IApprenticeshipVacancyDetailProvider apprenticeshipVacancyDetailProvider, IUserDataProvider userDataProvider)
        {
            var mediator = new ApprenticeshipSearchMediator(configurationManager, searchProvider, apprenticeshipVacancyDetailProvider, userDataProvider, new ApprenticeshipSearchViewModelClientValidator(), new ApprenticeshipSearchViewModelLocationValidator());
            return mediator;
        }
    }
}