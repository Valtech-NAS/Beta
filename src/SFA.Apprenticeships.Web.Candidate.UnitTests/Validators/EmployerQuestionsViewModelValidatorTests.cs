namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Validators
{
    using Candidate.ViewModels.Candidate;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Candidate.Validators;

    [TestFixture]
    public class EmployerQuestionsViewModelValidatorTests
    {
        private EmployerQuestionAnswersViewModelServerValidator _viewModelServerValidator;
        private EmployerQuestionAnswersViewModelSaveValidator _viewModelSaveValidator;

        [SetUp]
        public void Setup()
        {
            _viewModelServerValidator = new EmployerQuestionAnswersViewModelServerValidator();
            _viewModelSaveValidator = new EmployerQuestionAnswersViewModelSaveValidator();
        }

        [Test]
        public void ShouldNotHaveErrorsOnSaveWhenBlank()
        {
            var viewModel = new EmployerQuestionAnswersViewModel();
            _viewModelSaveValidator.ShouldNotHaveValidationErrorFor(x => x.CandidateAnswer1, viewModel);
            _viewModelSaveValidator.ShouldNotHaveValidationErrorFor(x => x.CandidateAnswer2, viewModel);
        }

        [Test]
        public void ShouldNotHaveErrorsOnServerWhenBlank()
        {
            var viewModel = new EmployerQuestionAnswersViewModel();
            _viewModelServerValidator.ShouldNotHaveValidationErrorFor(x => x.CandidateAnswer1, viewModel);
            _viewModelServerValidator.ShouldNotHaveValidationErrorFor(x => x.CandidateAnswer2, viewModel);
        }

        [Test]
        public void ShouldHaveErrorOnServerWhenQuestion1IsBlank()
        {
            var viewModel = new EmployerQuestionAnswersViewModel {SupplementaryQuestion1 = "Dummy"};
            _viewModelServerValidator.ShouldHaveValidationErrorFor(x => x.CandidateAnswer1, viewModel);
            _viewModelServerValidator.ShouldNotHaveValidationErrorFor(x => x.CandidateAnswer2, viewModel);
        }

        [Test]
        public void ShouldHaveErrorOnServerWhenQuestion2IsBlank()
        {
            var viewModel = new EmployerQuestionAnswersViewModel { SupplementaryQuestion2 = "Dummy" };
            _viewModelServerValidator.ShouldHaveValidationErrorFor(x => x.CandidateAnswer2, viewModel);
            _viewModelServerValidator.ShouldNotHaveValidationErrorFor(x => x.CandidateAnswer1, viewModel);
        }

        [Test]
        public void ShouldNotHaveErrorOnSaveWhenQuestion1IsBlank()
        {
            var viewModel = new EmployerQuestionAnswersViewModel { SupplementaryQuestion1 = "Dummy" };
            _viewModelSaveValidator.ShouldNotHaveValidationErrorFor(x => x.CandidateAnswer1, viewModel);
            _viewModelSaveValidator.ShouldNotHaveValidationErrorFor(x => x.CandidateAnswer2, viewModel);
        }

        [Test]
        public void ShouldNotHaveErrorOnSaveWhenQuestion2IsBlank()
        {
            var viewModel = new EmployerQuestionAnswersViewModel { SupplementaryQuestion2 = "Dummy" };
            _viewModelSaveValidator.ShouldNotHaveValidationErrorFor(x => x.CandidateAnswer1, viewModel);
            _viewModelSaveValidator.ShouldNotHaveValidationErrorFor(x => x.CandidateAnswer2, viewModel);
        }

    }
}
