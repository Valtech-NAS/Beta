namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    using System;
    using System.Web.Security;
    using Common.Providers;
    using Domain.Interfaces.Configuration;
    using Providers;
    using ViewModels.Applications;

    public class TraineeshipApplicationMediator : SearchMediatorBase, ITraineeshipApplicationMediator
    {
        private readonly ITraineeshipApplicationProvider _traineeshipApplicationProvider;

        public TraineeshipApplicationMediator(ITraineeshipApplicationProvider traineeshipApplicationProvider, IConfigurationManager configManager, IUserDataProvider userDataProvider) : base(configManager, userDataProvider)
        {
            _traineeshipApplicationProvider = traineeshipApplicationProvider;
        }

        public MediatorResponse<TraineeshipApplicationViewModel> Apply(Guid candidateId, int vacancyId)
        {
            var model = _traineeshipApplicationProvider.GetApplicationViewModel(candidateId, vacancyId);

            if (model.HasError())
            {
                return GetMediatorResponse<TraineeshipApplicationViewModel>(Codes.TraineeshipApplication.Apply.HasError);
            }

            model.SessionTimeout = FormsAuthentication.Timeout.TotalSeconds - 30;

            return GetMediatorResponse(Codes.TraineeshipApplication.Apply.Ok, model);
        }
    }
}