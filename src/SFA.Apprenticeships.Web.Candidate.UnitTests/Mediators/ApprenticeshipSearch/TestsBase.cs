namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipSearch
{
    using Candidate.Mediators;
    using Candidate.Providers;
    using Candidate.Validators;
    using Common.Providers;
    using Domain.Interfaces.Configuration;

    public abstract class TestsBase
    {
        protected static IApprenticeshipSearchMediator GetMediator(IConfigurationManager configurationManager, ISearchProvider searchProvider, IApprenticeshipVacancyDetailProvider apprenticeshipVacancyDetailProvider, IUserDataProvider userDataProvider)
        {
            var mediator = new ApprenticeshipSearchMediator(configurationManager, searchProvider, apprenticeshipVacancyDetailProvider, userDataProvider, new ApprenticeshipSearchViewModelClientValidator(), new ApprenticeshipSearchViewModelLocationValidator());
            return mediator;
        }
    }
}