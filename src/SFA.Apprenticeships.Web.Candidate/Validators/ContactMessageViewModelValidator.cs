namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Home;

    public class ContactMessageViewModelValidator : AbstractValidator<ContactMessageViewModel>
    {
        public ContactMessageViewModelValidator()
        {
            this.AddCommonRules();
        }
    }

    internal static class ContactMessageValidationRules
    {
        internal static void AddCommonRules(this AbstractValidator<ContactMessageViewModel> validator)
        {
            validator.RuleFor(x => x.Name)
                .Length(0, 71)
                .WithMessage(ContactMessageViewModelMessages.FullNameMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(ContactMessageViewModelMessages.FullNameMessages.RequiredErrorText)
                .Matches(ContactMessageViewModelMessages.FullNameMessages.WhiteListRegularExpression)
                .WithMessage(ContactMessageViewModelMessages.FullNameMessages.WhiteListErrorText);


            validator.RuleFor(x => x.Email)
                .Length(0, 100)
                .WithMessage(ContactMessageViewModelMessages.EmailAddressMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(ContactMessageViewModelMessages.EmailAddressMessages.RequiredErrorText)
                .Matches(ContactMessageViewModelMessages.EmailAddressMessages.WhiteListRegularExpression)
                .WithMessage(ContactMessageViewModelMessages.EmailAddressMessages.WhiteListErrorText);

            validator.RuleFor(x => x.Enquiry)
                .Length(0, 100)
                .WithMessage(ContactMessageViewModelMessages.EnquiryMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(ContactMessageViewModelMessages.EnquiryMessages.RequiredErrorText)
                .Matches(ContactMessageViewModelMessages.EnquiryMessages.WhiteListRegularExpression)
                .WithMessage(ContactMessageViewModelMessages.EnquiryMessages.WhiteListErrorText); ;

            validator.RuleFor(x => x.Details)
                .Length(0, 4000)
                .WithMessage(ContactMessageViewModelMessages.DetailsMessages.TooLongErrorText)
                .Matches(ContactMessageViewModelMessages.DetailsMessages.WhiteListRegularExpression)
                .WithMessage(ContactMessageViewModelMessages.DetailsMessages.WhiteListErrorText);
        }

    }
}