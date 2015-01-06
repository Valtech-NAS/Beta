namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.TraineeshipApplication
{
    using Candidate.Mediators;
    using Candidate.Providers;
    using Common.Providers;
    using Domain.Interfaces.Configuration;

    public abstract class TestsBase
    {
        protected static ITraineeshipApplicationMediator GetMediator(ITraineeshipApplicationProvider traineeshipApplicationProvider, IConfigurationManager configurationManager, IUserDataProvider userDataProvider)
        {
            var mediator = new TraineeshipApplicationMediator(traineeshipApplicationProvider, configurationManager, userDataProvider);
            return mediator;
        }
    }
}