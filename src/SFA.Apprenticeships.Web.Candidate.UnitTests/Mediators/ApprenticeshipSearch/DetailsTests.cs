namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipSearch
{
    using System;
    using Candidate.Mediators;
    using Candidate.Providers;
    using Candidate.Validators;
    using Candidate.ViewModels.VacancySearch;
    using Common.Constants;
    using Common.Providers;
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
            var searchProvider = new Mock<ISearchProvider>();
            var apprenticeshipVacancyDetailProvider = new Mock<IApprenticeshipVacancyDetailProvider>();
            apprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(It.IsAny<Guid?>(), It.IsAny<int>())).Returns((VacancyDetailViewModel)null);
            var userDataProvider = new Mock<IUserDataProvider>();
            var mediator = GetMediator(searchProvider.Object, apprenticeshipVacancyDetailProvider.Object, userDataProvider.Object);

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
            
            var searchProvider = new Mock<ISearchProvider>();
            var vacancyDetailViewModel = new VacancyDetailViewModel {ViewModelMessage = message};
            var apprenticeshipVacancyDetailProvider = new Mock<IApprenticeshipVacancyDetailProvider>();
            apprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(It.IsAny<Guid?>(), It.IsAny<int>())).Returns(vacancyDetailViewModel);
            var userDataProvider = new Mock<IUserDataProvider>();
            var mediator = GetMediator(searchProvider.Object, apprenticeshipVacancyDetailProvider.Object, userDataProvider.Object);

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
            var searchProvider = new Mock<ISearchProvider>();
            var vacancyDetailViewModel = new VacancyDetailViewModel();
            var apprenticeshipVacancyDetailProvider = new Mock<IApprenticeshipVacancyDetailProvider>();
            apprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(It.IsAny<Guid?>(), It.IsAny<int>())).Returns(vacancyDetailViewModel);
            var userDataProvider = new Mock<IUserDataProvider>();
            var mediator = GetMediator(searchProvider.Object, apprenticeshipVacancyDetailProvider.Object, userDataProvider.Object);

            var response = mediator.Details(Id, null, null);

            response.Code.Should().Be(Codes.ApprenticeshipSearch.Details.Ok);
            response.ViewModel.Should().NotBeNull();
            response.Message.Should().BeNull();
            response.Parameters.Should().BeNull();
        }

        private static IApprenticeshipSearchMediator GetMediator(ISearchProvider searchProvider, IApprenticeshipVacancyDetailProvider apprenticeshipVacancyDetailProvider, IUserDataProvider userDataProvider)
        {
            var mediator = new ApprenticeshipSearchMediator(searchProvider, apprenticeshipVacancyDetailProvider, userDataProvider, new ApprenticeshipSearchViewModelClientValidator(), new ApprenticeshipSearchViewModelLocationValidator());
            return mediator;
        }
    }
}