namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using System;
    using System.Data;
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Candidate;

    public class QualificationViewModelValidator : AbstractValidator<QualificationsViewModel>
    {
        public QualificationViewModelValidator()
        {
            RuleFor(x => x.QualificationType)
                .Length(0, 100)
                .WithMessage(QualificationViewModelMessages.QualificationTypeMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(QualificationViewModelMessages.QualificationTypeMessages.RequiredErrorText)
                .Matches(QualificationViewModelMessages.QualificationTypeMessages.WhiteListRegularExpression)
                .WithMessage(QualificationViewModelMessages.QualificationTypeMessages.WhiteListErrorText);

            RuleFor(x => x.Subject)
                .Length(0, 50)
                .WithMessage(QualificationViewModelMessages.SubjectMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(QualificationViewModelMessages.SubjectMessages.RequiredErrorText)
                .Matches(QualificationViewModelMessages.SubjectMessages.WhiteListRegularExpression)
                .WithMessage(QualificationViewModelMessages.SubjectMessages.WhiteListErrorText);

            RuleFor(x => x.Grade)
                .Length(0, 15)
                .WithMessage(QualificationViewModelMessages.GradeMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(QualificationViewModelMessages.GradeMessages.RequiredErrorText)
                .Matches(QualificationViewModelMessages.GradeMessages.WhiteListRegularExpression)
                .WithMessage(QualificationViewModelMessages.GradeMessages.WhiteListErrorText);

            RuleFor(x => x.Year)
                .NotEmpty()
                .WithMessage(QualificationViewModelMessages.YearMessages.RequiredErrorText)
                .Matches(QualificationViewModelMessages.YearMessages.WhiteListRegularExpression)               
                .WithMessage(QualificationViewModelMessages.YearMessages.WhiteListErrorText);

            RuleFor(x => x.Year)
                .Must(BeNowOrInThePast)   
                .WithMessage(QualificationViewModelMessages.YearMessages.BeforeOrEqualErrorText)
                .When(x => !x.IsPredicted);
        }

        private static bool BeNowOrInThePast(QualificationsViewModel instance, string year)
        {
            if (string.IsNullOrEmpty(year))
            {
                //Will be picked up by required validator
                return true;
            }
            var from = int.Parse(year);
            return from <= DateTime.Now.Year;
        }
    }
}