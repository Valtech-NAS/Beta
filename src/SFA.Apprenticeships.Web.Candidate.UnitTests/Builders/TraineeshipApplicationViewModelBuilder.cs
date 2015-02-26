namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Builders
{
    using System.Collections.Generic;
    using Candidate.ViewModels.Applications;
    using Candidate.ViewModels.Candidate;
    using Candidate.ViewModels.VacancySearch;

    public class TraineeshipApplicationViewModelBuilder
    {
        private string _message;
        private IEnumerable<QualificationsViewModel> _qualifications;
        private IEnumerable<WorkExperienceViewModel> _workExperience;

        public TraineeshipApplicationViewModelBuilder WithMessage(string message)
        {
            _message = message;
            return this;
        }

        public TraineeshipApplicationViewModelBuilder WithQualifications(IEnumerable<QualificationsViewModel> qualifications)
        {
            _qualifications = qualifications;
            return this;
        }

        public TraineeshipApplicationViewModelBuilder WithWorkExperience(IEnumerable<WorkExperienceViewModel> workExperience)
        {
            _workExperience = workExperience;
            return this;
        }

        public TraineeshipApplicationViewModel Build()
        {
            var viewModel = new TraineeshipApplicationViewModel(_message)
            {
                Candidate = new TraineeshipCandidateViewModel
                {
                    HasQualifications = _qualifications != null,
                    Qualifications = _qualifications,
                    HasWorkExperience = _workExperience != null,
                    WorkExperience = _workExperience
                },
                VacancyDetail = new VacancyDetailViewModel(),
            };
            return viewModel;
        }
    }
}