namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Candidate
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using FluentValidation.Attributes;
    using Validators;

    [Serializable]
    [Validator(typeof(EmployerQuestionAnswersViewModelClientValidator))]
    public class EmployerQuestionAnswersViewModel
    {
        public string SupplementaryQuestion1  { get; set; }

        [UIHint("FreetextLimited")]
        public string CandidateAnswer1 { get; set; }

        public string SupplementaryQuestion2 { get; set; }


        [UIHint("FreetextLimited")]
        public string CandidateAnswer2 { get; set; }
    }
}