namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    using Common.Providers;
    using Domain.Interfaces.Configuration;
    using Providers;

    public class TraineeshipApplicationMediator : SearchMediatorBase, ITraineeshipApplicationMediator
    {
        private readonly ITraineeshipApplicationProvider _traineeshipApplicationProvider;

        public TraineeshipApplicationMediator(ITraineeshipApplicationProvider traineeshipApplicationProvider, IConfigurationManager configManager, IUserDataProvider userDataProvider) : base(configManager, userDataProvider)
        {
            _traineeshipApplicationProvider = traineeshipApplicationProvider;
        }
    }
}