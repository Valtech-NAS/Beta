namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.TraineeshipSearch
{
    using Candidate.Mediators.Traineeships;
    using Candidate.Providers;
    using Candidate.Validators;
    using Common.Providers;
    using Domain.Interfaces.Configuration;

    public abstract class TestsBase
    {
        protected static ITraineeshipSearchMediator GetMediator(IConfigurationManager configurationManager, ISearchProvider searchProvider, ITraineeshipVacancyDetailProvider traineeshipVacancyDetailProvider, IUserDataProvider userDataProvider)
        {
            return new TraineeshipSearchMediator(configurationManager, searchProvider, traineeshipVacancyDetailProvider, userDataProvider, new TraineeshipSearchViewModelServerValidator(), new TraineeshipSearchViewModelLocationValidator());
        }
    }
}