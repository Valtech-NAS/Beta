namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using System;
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels;

    public class DateOfBirthViewModelClientValidator : AbstractValidator<DateViewModel>
    {
        public DateOfBirthViewModelClientValidator()
        {
            this.AddCommonRules();
        }
    }

    public class DateOfBirthViewModelServerValidator : AbstractValidator<DateViewModel>
    {
        public DateOfBirthViewModelServerValidator()
        {
            this.AddCommonRules();
            this.AddServerRules();
        }
    }

    internal static class DateOfBirthValidaitonRules
    {
        internal static void AddCommonRules(this AbstractValidator<DateViewModel> validator)
        {
            validator.RuleFor(x => x.Day)
                .InclusiveBetween(1, 31)
                .WithMessage(DateViewModelMessages.DayMessages.RangeErrorText)
                .NotEmpty()
                .WithMessage(DateViewModelMessages.DayMessages.RequiredErrorText);

            validator.RuleFor(x => x.Month)
                .InclusiveBetween(1, 12)
                .WithMessage(DateViewModelMessages.MonthMessages.RangeErrorText)
                .NotEmpty()
                .WithMessage(DateViewModelMessages.MonthMessages.RequiredErrorText);

            validator.RuleFor(x => x.Year)
                .InclusiveBetween(DateTime.Now.Year - 100, DateTime.Now.Year)
                .WithMessage(DateViewModelMessages.YearMessages.RangeErrorText)
                .NotEmpty()
                .WithMessage(DateViewModelMessages.YearMessages.RequiredErrorText);
        }

        internal static void AddServerRules(this AbstractValidator<DateViewModel> validator)
        {
            validator.RuleFor(x => x.Day)
                .Must(BeValidDate)
                .WithMessage(DateViewModelMessages.MustBeValidDate);
        }

        private static bool BeValidDate(DateViewModel instance, int day)
        {
            try
            {
                var date = new DateTime(instance.Year, instance.Month, instance.Day);
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }

            return true;
        }
    }
}