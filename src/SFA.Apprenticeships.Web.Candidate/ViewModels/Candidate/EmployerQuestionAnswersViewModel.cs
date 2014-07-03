namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Candidate
{
    using System;
    using FluentValidation.Attributes;
    using Validators;

    [Serializable]
    [Validator(typeof(EmployerQuestionAnswersViewModelClientValidator))]
    public class EmployerQuestionAnswersViewModel
    {
        public string SupplementaryQuestion1  { get; set; }

        public string CandidateAnswer1 { get; set; }

        public string SupplementaryQuestion2 { get; set; }

        public string CandidateAnswer2 { get; set; }
    }
}