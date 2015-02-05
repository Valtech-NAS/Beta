namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.Dashboard
{
    using System.Collections.Generic;
    using Candidate.ViewModels.Applications;
    using Candidate.ViewModels.MyApplications;

    public class MyApplicationViewModelBuilder
    {
        private List<MyApprenticeshipApplicationViewModel> _apprenticeshipApplicationViewModels;
        private List<MyTraineeshipApplicationViewModel> _traineeshipApplicationViewModels;
        private TraineeshipFeatureViewModel _traineeshipFeatureViewModel;

        public MyApplicationViewModelBuilder()
        {
            _apprenticeshipApplicationViewModels = new List<MyApprenticeshipApplicationViewModel>();
            _traineeshipApplicationViewModels = new List<MyTraineeshipApplicationViewModel>();
            _traineeshipFeatureViewModel = new TraineeshipFeatureViewModel();
        }

        public MyApplicationViewModelBuilder With(
            List<MyApprenticeshipApplicationViewModel> apprenticeshipApplicationViewModels)
        {
            _apprenticeshipApplicationViewModels = apprenticeshipApplicationViewModels;
            return this;
        }

        public MyApplicationViewModelBuilder With(
            List<MyTraineeshipApplicationViewModel> traineeshipApplicationViewModels)
        {
            _traineeshipApplicationViewModels = traineeshipApplicationViewModels;
            return this;
        }

        public MyApplicationViewModelBuilder With(TraineeshipFeatureViewModel traineeshipFeatureViewModel)
        {
            _traineeshipFeatureViewModel = traineeshipFeatureViewModel;
            return this;
        }

        public MyApplicationsViewModel Build()
        {
            return new MyApplicationsViewModel(_apprenticeshipApplicationViewModels, _traineeshipApplicationViewModels,
                _traineeshipFeatureViewModel);
        }
    }
}