namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Locations;

    public class AddressViewModelValidator : AbstractValidator<AddressViewModel>
    {
        public AddressViewModelValidator()
        {
            RuleFor(x => x.AddressLine1)
                .Length(0, 50)
                .WithMessage(AddressViewModelMessages.AddressLine1.TooLongErrorText)
                .NotEmpty()
                .WithMessage(AddressViewModelMessages.AddressLine1.RequiredErrorText)
                .Matches(AddressViewModelMessages.AddressLine1.WhiteListRegularExpression)
                .WithMessage(AddressViewModelMessages.AddressLine1.WhiteListErrorText);

            RuleFor(x => x.AddressLine2)
                .Length(0, 50)
                .WithMessage(AddressViewModelMessages.AddressLine2.TooLongErrorText)
                .Matches(AddressViewModelMessages.AddressLine2.WhiteListRegularExpression)
                .WithMessage(AddressViewModelMessages.AddressLine2.WhiteListErrorText);

            RuleFor(x => x.AddressLine3)
                .Length(0, 50)
                .WithMessage(AddressViewModelMessages.AddressLine3.TooLongErrorText)
                .Matches(AddressViewModelMessages.AddressLine3.WhiteListRegularExpression)
                .WithMessage(AddressViewModelMessages.AddressLine3.WhiteListErrorText);

            RuleFor(x => x.AddressLine4)
                .Length(0, 50)
                .WithMessage(AddressViewModelMessages.AddressLine4.TooLongErrorText)
                .Matches(AddressViewModelMessages.AddressLine4.WhiteListRegularExpression)
                .WithMessage(AddressViewModelMessages.AddressLine4.WhiteListErrorText);

            RuleFor(x => x.Postcode)
                .Length(0, 8)
                .WithMessage(AddressViewModelMessages.Postcode.TooLongErrorText)
                .NotEmpty()
                .WithMessage(AddressViewModelMessages.Postcode.RequiredErrorText)
                .Matches(AddressViewModelMessages.Postcode.WhiteListRegularExpression)
                .WithMessage(AddressViewModelMessages.Postcode.WhiteListErrorText);
        }
    }
}