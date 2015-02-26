namespace SFA.Apprenticeships.Web.Employer.Validators
{
    using Constants;
    using FluentValidation;
    using ViewModels;

    public class AddressViewModelValidators : AbstractValidator<AddressViewModel>
    {
        public AddressViewModelValidators()
        {
            RuleFor(x => x.AddressLine1)
                .Length(0, 50)
                .WithMessage(AddressViewModelMessages.AddressLine1Messages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(AddressViewModelMessages.AddressLine1Messages.RequiredErrorText)
                .Matches(AddressViewModelMessages.AddressLine1Messages.WhiteListRegularExpression)
                .WithMessage(AddressViewModelMessages.AddressLine1Messages.WhiteListErrorText);

            RuleFor(x => x.AddressLine2)
                .Length(0, 50)
                .WithMessage(AddressViewModelMessages.AddressLine2Messages.TooLongErrorText)
                .Matches(AddressViewModelMessages.AddressLine2Messages.WhiteListRegularExpression)
                .WithMessage(AddressViewModelMessages.AddressLine2Messages.WhiteListErrorText);

            RuleFor(x => x.AddressLine3)
                .Length(0, 50)
                .WithMessage(AddressViewModelMessages.AddressLine3Messages.TooLongErrorText)
                .Matches(AddressViewModelMessages.AddressLine3Messages.WhiteListRegularExpression)
                .WithMessage(AddressViewModelMessages.AddressLine3Messages.WhiteListErrorText);

            RuleFor(x => x.City)
                .Length(0, 50)
                .WithMessage(AddressViewModelMessages.CityMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(AddressViewModelMessages.CityMessages.RequiredErrorText)
                .Matches(AddressViewModelMessages.CityMessages.WhiteListRegularExpression)
                .WithMessage(AddressViewModelMessages.CityMessages.WhiteListErrorText);

            RuleFor(x => x.Postcode)
                .Length(0, 8)
                .WithMessage(AddressViewModelMessages.PostcodeMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(AddressViewModelMessages.PostcodeMessages.RequiredErrorText)
                .Matches(AddressViewModelMessages.PostcodeMessages.WhiteListRegularExpression)
                .WithMessage(AddressViewModelMessages.PostcodeMessages.WhiteListErrorText);
        }
    }
}
