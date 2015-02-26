namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Builders
{
    using Candidate.ViewModels.Candidate;

    public class WorkExperienceViewModelBuilder
    {
        private string _description;
        private string _employer;
        private string _jobTitle;
        private int _fromMonth;
        private string _fromYear;
        private int _toMonth;
        private string _toYear;

        public WorkExperienceViewModelBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public WorkExperienceViewModelBuilder WithEmployer(string employer)
        {
            _employer = employer;
            return this;
        }

        public WorkExperienceViewModelBuilder WithJobTitle(string jobTitle)
        {
            _jobTitle = jobTitle;
            return this;
        }

        public WorkExperienceViewModelBuilder WithFromMonth(int fromMonth)
        {
            _fromMonth = fromMonth;
            return this;
        }

        public WorkExperienceViewModelBuilder WithFromYear(string fromYear)
        {
            _fromYear = fromYear;
            return this;
        }

        public WorkExperienceViewModelBuilder WithToMonth(int toMonth)
        {
            _toMonth = toMonth;
            return this;
        }

        public WorkExperienceViewModelBuilder WithToYear(string toYear)
        {
            _toYear = toYear;
            return this;
        }

        public WorkExperienceViewModel Build()
        {
            var viewModel = new WorkExperienceViewModel
            {
                Description = _description,
                Employer = _employer,
                JobTitle = _jobTitle,
                FromMonth = _fromMonth,
                FromYear = _fromYear,
                ToMonth = _toMonth,
                ToYear = _toYear
            };
            return viewModel;
        }
    }
}