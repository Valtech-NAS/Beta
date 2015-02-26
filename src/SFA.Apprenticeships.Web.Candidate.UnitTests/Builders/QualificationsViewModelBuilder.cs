namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Builders
{
    using Candidate.ViewModels.Candidate;

    public class QualificationsViewModelBuilder
    {
        private string _grade;
        private bool _isPredicted;
        private string _qualificationType;
        private string _subject;
        private string _year;

        public QualificationsViewModelBuilder WithGrade(string grade)
        {
            _grade = grade;
            return this;
        }

        public QualificationsViewModelBuilder WithIsPredicted(bool isPredicted)
        {
            _isPredicted = isPredicted;
            return this;
        }

        public QualificationsViewModelBuilder WithQualificationType(string qualificationType)
        {
            _qualificationType = qualificationType;
            return this;
        }

        public QualificationsViewModelBuilder WithSubject(string subject)
        {
            _subject = subject;
            return this;
        }

        public QualificationsViewModelBuilder WithYear(string year)
        {
            _year = year;
            return this;
        }

        public QualificationsViewModel Build()
        {
            var viewModel = new QualificationsViewModel
            {
                Grade = _grade,
                IsPredicted = _isPredicted,
                QualificationType = _qualificationType,
                Subject = _subject,
                Year = _year
            };
            return viewModel;
        }
    }
}