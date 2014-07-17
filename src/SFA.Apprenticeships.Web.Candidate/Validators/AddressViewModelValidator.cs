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
                .WithMessage(AddressMessages.AddressLine1.TooLongErrorText)
                .NotEmpty()
                .WithMessage(AddressMessages.AddressLine1.RequiredErrorText)
                .Matches(AddressMessages.AddressLine1.WhiteListRegularExpression)
                .WithMessage(AddressMessages.AddressLine1.WhiteListErrorText);

            RuleFor(x => x.AddressLine2)
                .Length(0, 50)
                .WithMessage(AddressMessages.AddressLine2.TooLongErrorText)
                .Matches(AddressMessages.AddressLine2.WhiteListRegularExpression)
                .WithMessage(AddressMessages.AddressLine2.WhiteListErrorText);

            RuleFor(x => x.AddressLine3)
                .Length(0, 50)
                .WithMessage(AddressMessages.AddressLine3.TooLongErrorText)
                .NotEmpty()
                .WithMessage(AddressMessages.AddressLine3.RequiredErrorText)
                .Matches(AddressMessages.AddressLine3.WhiteListRegularExpression)
                .WithMessage(AddressMessages.AddressLine3.WhiteListErrorText);

            RuleFor(x => x.AddressLine4)
                .Length(0, 50)
                .WithMessage(AddressMessages.AddressLine4.TooLongErrorText)
                .NotEmpty()
                .WithMessage(AddressMessages.AddressLine4.RequiredErrorText)
                .Matches(AddressMessages.AddressLine4.WhiteListRegularExpression)
                .WithMessage(AddressMessages.AddressLine4.WhiteListErrorText);

            RuleFor(x => x.Postcode)
                .NotEmpty()
                .WithMessage(AddressMessages.Postcode.RequiredErrorText)
                .Matches(AddressMessages.Postcode.WhiteListRegularExpression)
                .WithMessage(AddressMessages.Postcode.WhiteListErrorText);
        }
    }
}