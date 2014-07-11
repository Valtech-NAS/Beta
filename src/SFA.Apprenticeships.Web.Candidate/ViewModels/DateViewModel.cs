namespace SFA.Apprenticeships.Web.Candidate.ViewModels
{
    using FluentValidation.Attributes;
    using Validators;

    [Validator(typeof(DateOfBirthViewModelClientValidator))]
    public class DateViewModel
    {
        public int? Day { get; set; }

        public int? Month { get; set; }

        public int? Year { get; set; }
    }
}