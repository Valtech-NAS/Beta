namespace SFA.Apprenticeships.Web.Employer.Validators
{
    using Constants;
    using FluentValidation;
    using ViewModels;

    public class EmployerEnquiryViewModelServerValidators : AbstractValidator<EmployerEnquiryViewModel>
    {
        public EmployerEnquiryViewModelServerValidators()
        {
            this.AddCommonRules();
            this.AddServerRules();
        }
    }

    public class EmployerEnquiryViewModelClientValidator : AbstractValidator<EmployerEnquiryViewModel>
    {
        public EmployerEnquiryViewModelClientValidator()
        {
            this.AddCommonRules();
        }
    }

    internal static class EmployerEnquiryViewModelValidationRules
    {
        internal static void AddCommonRules(this AbstractValidator<EmployerEnquiryViewModel> validator)
        {
            validator.RuleFor(x => x.Firstname)
                 .Length(0, 35)
                 .WithMessage(EmployerEnquiryViewModelMessages.FirstnameMessages.TooLongErrorText)
                 .NotEmpty()
                 .WithMessage(EmployerEnquiryViewModelMessages.FirstnameMessages.RequiredErrorText)
                 .Matches(EmployerEnquiryViewModelMessages.FirstnameMessages.WhiteListRegularExpression)
                 .WithMessage(EmployerEnquiryViewModelMessages.FirstnameMessages.WhiteListErrorText);

            validator.RuleFor(x => x.Lastname)
                 .Length(0, 35)
                 .WithMessage(EmployerEnquiryViewModelMessages.LastnameMessages.TooLongErrorText)
                 .NotEmpty()
                 .WithMessage(EmployerEnquiryViewModelMessages.LastnameMessages.RequiredErrorText)
                 .Matches(EmployerEnquiryViewModelMessages.LastnameMessages.WhiteListRegularExpression)
                 .WithMessage(EmployerEnquiryViewModelMessages.LastnameMessages.WhiteListErrorText);

            validator.RuleFor(x => x.Companyname)
                .Length(0, 35)
                .WithMessage(EmployerEnquiryViewModelMessages.CompanynameMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(EmployerEnquiryViewModelMessages.CompanynameMessages.RequiredErrorText)
                .Matches(EmployerEnquiryViewModelMessages.CompanynameMessages.WhiteListRegularExpression)
                .WithMessage(EmployerEnquiryViewModelMessages.CompanynameMessages.WhiteListErrorText);

            validator.RuleFor(x => x.Email)
                .Length(0, 100)
                .WithMessage(EmployerEnquiryViewModelMessages.EmailAddressMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(EmployerEnquiryViewModelMessages.EmailAddressMessages.RequiredErrorText)
                .Matches(EmployerEnquiryViewModelMessages.EmailAddressMessages.WhiteListRegularExpression)
                .WithMessage(EmployerEnquiryViewModelMessages.EmailAddressMessages.WhiteListErrorText);

            validator.RuleFor(x => x.WorkPhoneNumber)
               .Length(8, 16)
               .WithMessage(EmployerEnquiryViewModelMessages.WorkPhoneNumberMessages.LengthErrorText)
               .NotEmpty()
               .WithMessage(EmployerEnquiryViewModelMessages.WorkPhoneNumberMessages.RequiredErrorText)
               .Matches(EmployerEnquiryViewModelMessages.WorkPhoneNumberMessages.WhiteListRegularExpression)
               .WithMessage(EmployerEnquiryViewModelMessages.WorkPhoneNumberMessages.WhiteListErrorText);

            validator.RuleFor(x => x.MobileNumber)
                .Length(8, 16)
                .WithMessage(EmployerEnquiryViewModelMessages.MobileNumberMessages.LengthErrorText)
                .Matches(EmployerEnquiryViewModelMessages.MobileNumberMessages.WhiteListRegularExpression)
                .WithMessage(EmployerEnquiryViewModelMessages.MobileNumberMessages.WhiteListErrorText);

            validator.RuleFor(x => x.Position)
                .Length(0, 35)
                .WithMessage(EmployerEnquiryViewModelMessages.PositionMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(EmployerEnquiryViewModelMessages.PositionMessages.RequiredErrorText)
                .Matches(EmployerEnquiryViewModelMessages.PositionMessages.WhiteListRegularExpression)
                .WithMessage(EmployerEnquiryViewModelMessages.PositionMessages.WhiteListErrorText);

            validator.RuleFor(x => x.EnquiryDescription)
                .Length(0, 4000)
                .WithMessage(EmployerEnquiryViewModelMessages.EnquiryDescriptionMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(EmployerEnquiryViewModelMessages.EnquiryDescriptionMessages.RequiredErrorText)
                .Matches(EmployerEnquiryViewModelMessages.EnquiryDescriptionMessages.WhiteListRegularExpression)
                 .WithMessage(EmployerEnquiryViewModelMessages.EnquiryDescriptionMessages.WhiteListErrorText);

            validator.RuleFor(x => x.WorkSector)
                .NotEmpty()
                .WithMessage(EmployerEnquiryViewModelMessages.WorkSectorMessages.RequiredErrorText);

            validator.RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage(EmployerEnquiryViewModelMessages.NameTitleMessages.RequiredErrorText);

            validator.RuleFor(x => x.PreviousExperienceType)
                .NotEmpty()
                .WithMessage(EmployerEnquiryViewModelMessages.PreviousExperienceTypeMessages.RequiredErrorText);

            validator.RuleFor(x => x.EmployeesCount)
                .NotEmpty()
                .WithMessage(EmployerEnquiryViewModelMessages.EmployeCountMessages.RequiredErrorText);

            validator.RuleFor(x => x.EnquirySource)
                .NotEmpty()
                .WithMessage(EmployerEnquiryViewModelMessages.EnquirySourceMessages.RequiredErrorText);
        }

        internal static void AddServerRules(this AbstractValidator<EmployerEnquiryViewModel> validator)
        {

        }
    }
}
