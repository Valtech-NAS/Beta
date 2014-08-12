namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Candidate;

    public class AboutYouViewModelValidator : AbstractValidator<AboutYouViewModel>
    {
        public AboutYouViewModelValidator()
        {
            RuleFor(x => x.WhatAreYourStrengths)
                .Length(0, 4000)
                .WithMessage(AboutYouMessages.WhatAreYourStrengthsMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(AboutYouMessages.WhatAreYourStrengthsMessages.RequiredErrorText)
                .Matches(AboutYouMessages.WhatAreYourStrengthsMessages.WhiteListRegularExpression)
                .WithMessage(AboutYouMessages.WhatAreYourStrengthsMessages.WhiteListErrorText);

            RuleFor(x => x.WhatDoYouFeelYouCouldImprove)
                .Length(0, 4000)
                .WithMessage(AboutYouMessages.WhatDoYouFeelYouCouldImproveMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(AboutYouMessages.WhatDoYouFeelYouCouldImproveMessages.RequiredErrorText)
                .Matches(AboutYouMessages.WhatDoYouFeelYouCouldImproveMessages.WhiteListRegularExpression)
                .WithMessage(AboutYouMessages.WhatDoYouFeelYouCouldImproveMessages.WhiteListErrorText);

            RuleFor(x => x.WhatAreYourHobbiesInterests)
                .Length(0, 4000)
                .WithMessage(AboutYouMessages.WhatAreYourHobbiesInterestsMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(AboutYouMessages.WhatAreYourHobbiesInterestsMessages.RequiredErrorText)
                .Matches(AboutYouMessages.WhatAreYourHobbiesInterestsMessages.WhiteListRegularExpression)
                .WithMessage(AboutYouMessages.WhatAreYourHobbiesInterestsMessages.WhiteListErrorText);

            RuleFor(x => x.AnythingWeCanDoToSupportYourInterview)
                .Length(0, 4000)
                .WithMessage(AboutYouMessages.AnythingWeCanDoToSupportYourInterviewMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(AboutYouMessages.AnythingWeCanDoToSupportYourInterviewMessages.RequiredErrorText)
                .Matches(AboutYouMessages.AnythingWeCanDoToSupportYourInterviewMessages.WhiteListRegularExpression)
                .WithMessage(AboutYouMessages.AnythingWeCanDoToSupportYourInterviewMessages.WhiteListErrorText);            
        }
    }

    public class AboutYouViewModelSaveValidator : AbstractValidator<AboutYouViewModel>
    {
        public AboutYouViewModelSaveValidator()
        {
            RuleFor(x => x.WhatAreYourStrengths)
                .Length(0, 4000)
                .WithMessage(AboutYouMessages.WhatAreYourStrengthsMessages.TooLongErrorText)
                .Matches(AboutYouMessages.WhatAreYourStrengthsMessages.WhiteListRegularExpression)
                .WithMessage(AboutYouMessages.WhatAreYourStrengthsMessages.WhiteListErrorText);

            RuleFor(x => x.WhatDoYouFeelYouCouldImprove)
                .Length(0, 4000)
                .WithMessage(AboutYouMessages.WhatDoYouFeelYouCouldImproveMessages.TooLongErrorText)
                .Matches(AboutYouMessages.WhatDoYouFeelYouCouldImproveMessages.WhiteListRegularExpression)
                .WithMessage(AboutYouMessages.WhatDoYouFeelYouCouldImproveMessages.WhiteListErrorText);

            RuleFor(x => x.WhatAreYourHobbiesInterests)
                .Length(0, 4000)
                .WithMessage(AboutYouMessages.WhatAreYourHobbiesInterestsMessages.TooLongErrorText)
                .Matches(AboutYouMessages.WhatAreYourHobbiesInterestsMessages.WhiteListRegularExpression)
                .WithMessage(AboutYouMessages.WhatAreYourHobbiesInterestsMessages.WhiteListErrorText);

            RuleFor(x => x.AnythingWeCanDoToSupportYourInterview)
                .Length(0, 4000)
                .WithMessage(AboutYouMessages.AnythingWeCanDoToSupportYourInterviewMessages.TooLongErrorText)
                .Matches(AboutYouMessages.AnythingWeCanDoToSupportYourInterviewMessages.WhiteListRegularExpression)
                .WithMessage(AboutYouMessages.AnythingWeCanDoToSupportYourInterviewMessages.WhiteListErrorText)
                .When(x => x.RequiresSupportForInterview);
        }
    }

    public class AboutYouViewModelServerValidator : AbstractValidator<AboutYouViewModel>
    {
        public AboutYouViewModelServerValidator()
        {
            RuleFor(x => x.WhatAreYourStrengths)
                .Length(0, 4000)
                .WithMessage(AboutYouMessages.WhatAreYourStrengthsMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(AboutYouMessages.WhatAreYourStrengthsMessages.RequiredErrorText)
                .Matches(AboutYouMessages.WhatAreYourStrengthsMessages.WhiteListRegularExpression)
                .WithMessage(AboutYouMessages.WhatAreYourStrengthsMessages.WhiteListErrorText);

            RuleFor(x => x.WhatDoYouFeelYouCouldImprove)
                .Length(0, 4000)
                .WithMessage(AboutYouMessages.WhatDoYouFeelYouCouldImproveMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(AboutYouMessages.WhatDoYouFeelYouCouldImproveMessages.RequiredErrorText)
                .Matches(AboutYouMessages.WhatDoYouFeelYouCouldImproveMessages.WhiteListRegularExpression)
                .WithMessage(AboutYouMessages.WhatDoYouFeelYouCouldImproveMessages.WhiteListErrorText);

            RuleFor(x => x.WhatAreYourHobbiesInterests)
                .Length(0, 4000)
                .WithMessage(AboutYouMessages.WhatAreYourHobbiesInterestsMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(AboutYouMessages.WhatAreYourHobbiesInterestsMessages.RequiredErrorText)
                .Matches(AboutYouMessages.WhatAreYourHobbiesInterestsMessages.WhiteListRegularExpression)
                .WithMessage(AboutYouMessages.WhatAreYourHobbiesInterestsMessages.WhiteListErrorText);

            RuleFor(x => x.AnythingWeCanDoToSupportYourInterview)
                .Length(0, 4000)
                .WithMessage(AboutYouMessages.AnythingWeCanDoToSupportYourInterviewMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(AboutYouMessages.AnythingWeCanDoToSupportYourInterviewMessages.RequiredErrorText)
                .Matches(AboutYouMessages.AnythingWeCanDoToSupportYourInterviewMessages.WhiteListRegularExpression)
                .WithMessage(AboutYouMessages.AnythingWeCanDoToSupportYourInterviewMessages.WhiteListErrorText)
                .When(x => x.RequiresSupportForInterview);
        }
    }
}