namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Builders
{
    using Candidate.ViewModels.Applications;
    using Candidate.ViewModels.Candidate;
    using Candidate.ViewModels.VacancySearch;

    public class TraineeshipApplicationViewModelBuilder
    {
        private string _message;

        public TraineeshipApplicationViewModelBuilder WithMessage(string message)
        {
            _message = message;
            return this;
        }

        public TraineeshipApplicationViewModel Build()
        {
            var viewModel = new TraineeshipApplicationViewModel(_message)
            {
                Candidate = new TraineeshipCandidateViewModel(),
                VacancyDetail = new VacancyDetailViewModel(),
            };
            return viewModel;
        }
    }
}