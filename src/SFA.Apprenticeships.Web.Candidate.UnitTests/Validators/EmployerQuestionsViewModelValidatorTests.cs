namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Validators
{
    using Candidate.ViewModels.Candidate;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Candidate.Validators;

    [TestFixture]
    public class EmployerQuestionsViewModelValidatorTests
    {
        [Test]
        public void ShouldNotHaveErrorsOnSaveWhenBlank()
        {
            var viewModel = new EmployerQuestionAnswersViewModel();
            var viewModelSaveValidator = new EmployerQuestionAnswersViewModelSaveValidator();

            viewModelSaveValidator.ShouldNotHaveValidationErrorFor(x => x.CandidateAnswer1, viewModel);
            viewModelSaveValidator.ShouldNotHaveValidationErrorFor(x => x.CandidateAnswer2, viewModel);
        }

        [Test]
        public void ShouldNotHaveErrorsOnServerWhenBlank()
        {
            var viewModel = new EmployerQuestionAnswersViewModel();
            var viewModelServerValidator = new EmployerQuestionAnswersViewModelServerValidator();

            viewModelServerValidator.ShouldNotHaveValidationErrorFor(x => x.CandidateAnswer1, viewModel);
            viewModelServerValidator.ShouldNotHaveValidationErrorFor(x => x.CandidateAnswer2, viewModel);
        }

        [Test]
        public void ShouldHaveErrorOnServerWhenQuestion1IsBlank()
        {
            var viewModel = new EmployerQuestionAnswersViewModel {SupplementaryQuestion1 = "Dummy"};
            var viewModelServerValidator = new EmployerQuestionAnswersViewModelServerValidator();

            viewModelServerValidator.ShouldHaveValidationErrorFor(x => x.CandidateAnswer1, viewModel);
            viewModelServerValidator.ShouldNotHaveValidationErrorFor(x => x.CandidateAnswer2, viewModel);
        }

        [Test]
        public void ShouldHaveErrorOnServerWhenQuestion2IsBlank()
        {
            var viewModel = new EmployerQuestionAnswersViewModel { SupplementaryQuestion2 = "Dummy" };
            var viewModelServerValidator = new EmployerQuestionAnswersViewModelServerValidator();

            viewModelServerValidator.ShouldHaveValidationErrorFor(x => x.CandidateAnswer2, viewModel);
            viewModelServerValidator.ShouldNotHaveValidationErrorFor(x => x.CandidateAnswer1, viewModel);
        }

        [Test]
        public void ShouldNotHaveErrorOnSaveWhenQuestion1IsBlank()
        {
            var viewModel = new EmployerQuestionAnswersViewModel { SupplementaryQuestion1 = "Dummy" };
            var viewModelSaveValidator = new EmployerQuestionAnswersViewModelSaveValidator();

            viewModelSaveValidator.ShouldNotHaveValidationErrorFor(x => x.CandidateAnswer1, viewModel);
            viewModelSaveValidator.ShouldNotHaveValidationErrorFor(x => x.CandidateAnswer2, viewModel);
        }

        [Test]
        public void ShouldNotHaveErrorOnSaveWhenQuestion2IsBlank()
        {
            var viewModel = new EmployerQuestionAnswersViewModel { SupplementaryQuestion2 = "Dummy" };
            var viewModelSaveValidator = new EmployerQuestionAnswersViewModelSaveValidator();

            viewModelSaveValidator.ShouldNotHaveValidationErrorFor(x => x.CandidateAnswer1, viewModel);
            viewModelSaveValidator.ShouldNotHaveValidationErrorFor(x => x.CandidateAnswer2, viewModel);
        }
    }
}
