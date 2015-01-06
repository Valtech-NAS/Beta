namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    using Common.Providers;
    using Domain.Interfaces.Configuration;

    public class TraineeshipApplicationMediator : SearchMediatorBase, ITraineeshipApplicationMediator
    {
        public TraineeshipApplicationMediator(IConfigurationManager configManager, IUserDataProvider userDataProvider) : base(configManager, userDataProvider)
        {
        }
    }
}