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
                .WithMessage(AboutYouViewModelMessages.WhatAreYourStrengthsMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(AboutYouViewModelMessages.WhatAreYourStrengthsMessages.RequiredErrorText)
                .Matches(AboutYouViewModelMessages.WhatAreYourStrengthsMessages.WhiteListRegularExpression)
                .WithMessage(AboutYouViewModelMessages.WhatAreYourStrengthsMessages.WhiteListErrorText);

            RuleFor(x => x.WhatDoYouFeelYouCouldImprove)
                .Length(0, 4000)
                .WithMessage(AboutYouViewModelMessages.WhatDoYouFeelYouCouldImproveMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(AboutYouViewModelMessages.WhatDoYouFeelYouCouldImproveMessages.RequiredErrorText)
                .Matches(AboutYouViewModelMessages.WhatDoYouFeelYouCouldImproveMessages.WhiteListRegularExpression)
                .WithMessage(AboutYouViewModelMessages.WhatDoYouFeelYouCouldImproveMessages.WhiteListErrorText);

            RuleFor(x => x.WhatAreYourHobbiesInterests)
                .Length(0, 4000)
                .WithMessage(AboutYouViewModelMessages.WhatAreYourHobbiesInterestsMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(AboutYouViewModelMessages.WhatAreYourHobbiesInterestsMessages.RequiredErrorText)
                .Matches(AboutYouViewModelMessages.WhatAreYourHobbiesInterestsMessages.WhiteListRegularExpression)
                .WithMessage(AboutYouViewModelMessages.WhatAreYourHobbiesInterestsMessages.WhiteListErrorText);

            RuleFor(x => x.AnythingWeCanDoToSupportYourInterview)
                .Length(0, 4000)
                .WithMessage(AboutYouViewModelMessages.AnythingWeCanDoToSupportYourInterviewMessages.TooLongErrorText)               
                .Matches(AboutYouViewModelMessages.AnythingWeCanDoToSupportYourInterviewMessages.WhiteListRegularExpression)
                .WithMessage(AboutYouViewModelMessages.AnythingWeCanDoToSupportYourInterviewMessages.WhiteListErrorText)
                .NotEmpty()
                .WithMessage(AboutYouViewModelMessages.AnythingWeCanDoToSupportYourInterviewMessages.RequiredErrorText);            
        }
    }

    public class AboutYouViewModelSaveValidator : AbstractValidator<AboutYouViewModel>
    {
        public AboutYouViewModelSaveValidator()
        {
            RuleFor(x => x.WhatAreYourStrengths)
                .Length(0, 4000)
                .WithMessage(AboutYouViewModelMessages.WhatAreYourStrengthsMessages.TooLongErrorText)
                .Matches(AboutYouViewModelMessages.WhatAreYourStrengthsMessages.WhiteListRegularExpression)
                .WithMessage(AboutYouViewModelMessages.WhatAreYourStrengthsMessages.WhiteListErrorText);

            RuleFor(x => x.WhatDoYouFeelYouCouldImprove)
                .Length(0, 4000)
                .WithMessage(AboutYouViewModelMessages.WhatDoYouFeelYouCouldImproveMessages.TooLongErrorText)
                .Matches(AboutYouViewModelMessages.WhatDoYouFeelYouCouldImproveMessages.WhiteListRegularExpression)
                .WithMessage(AboutYouViewModelMessages.WhatDoYouFeelYouCouldImproveMessages.WhiteListErrorText);

            RuleFor(x => x.WhatAreYourHobbiesInterests)
                .Length(0, 4000)
                .WithMessage(AboutYouViewModelMessages.WhatAreYourHobbiesInterestsMessages.TooLongErrorText)
                .Matches(AboutYouViewModelMessages.WhatAreYourHobbiesInterestsMessages.WhiteListRegularExpression)
                .WithMessage(AboutYouViewModelMessages.WhatAreYourHobbiesInterestsMessages.WhiteListErrorText);

            RuleFor(x => x.AnythingWeCanDoToSupportYourInterview)
                .Length(0, 4000)
                .WithMessage(AboutYouViewModelMessages.AnythingWeCanDoToSupportYourInterviewMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(AboutYouViewModelMessages.AnythingWeCanDoToSupportYourInterviewMessages.RequiredErrorText)
                .Matches(
                    AboutYouViewModelMessages.AnythingWeCanDoToSupportYourInterviewMessages.WhiteListRegularExpression)
                .WithMessage(AboutYouViewModelMessages.AnythingWeCanDoToSupportYourInterviewMessages.WhiteListErrorText)
                .When(x => x.RequiresSupportForInterview);
        }
    }

    public class AboutYouViewModelServerValidator : AbstractValidator<AboutYouViewModel>
    {
        public AboutYouViewModelServerValidator()
        {
            RuleFor(x => x.WhatAreYourStrengths)
                .Length(0, 4000)
                .WithMessage(AboutYouViewModelMessages.WhatAreYourStrengthsMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(AboutYouViewModelMessages.WhatAreYourStrengthsMessages.RequiredErrorText)
                .Matches(AboutYouViewModelMessages.WhatAreYourStrengthsMessages.WhiteListRegularExpression)
                .WithMessage(AboutYouViewModelMessages.WhatAreYourStrengthsMessages.WhiteListErrorText);

            RuleFor(x => x.WhatDoYouFeelYouCouldImprove)
                .Length(0, 4000)
                .WithMessage(AboutYouViewModelMessages.WhatDoYouFeelYouCouldImproveMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(AboutYouViewModelMessages.WhatDoYouFeelYouCouldImproveMessages.RequiredErrorText)
                .Matches(AboutYouViewModelMessages.WhatDoYouFeelYouCouldImproveMessages.WhiteListRegularExpression)
                .WithMessage(AboutYouViewModelMessages.WhatDoYouFeelYouCouldImproveMessages.WhiteListErrorText);

            RuleFor(x => x.WhatAreYourHobbiesInterests)
                .Length(0, 4000)
                .WithMessage(AboutYouViewModelMessages.WhatAreYourHobbiesInterestsMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(AboutYouViewModelMessages.WhatAreYourHobbiesInterestsMessages.RequiredErrorText)
                .Matches(AboutYouViewModelMessages.WhatAreYourHobbiesInterestsMessages.WhiteListRegularExpression)
                .WithMessage(AboutYouViewModelMessages.WhatAreYourHobbiesInterestsMessages.WhiteListErrorText);

            RuleFor(x => x.AnythingWeCanDoToSupportYourInterview)
                .Length(0, 4000)
                .WithMessage(AboutYouViewModelMessages.AnythingWeCanDoToSupportYourInterviewMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(AboutYouViewModelMessages.AnythingWeCanDoToSupportYourInterviewMessages.RequiredErrorText)
                .Matches(AboutYouViewModelMessages.AnythingWeCanDoToSupportYourInterviewMessages.WhiteListRegularExpression)
                .WithMessage(AboutYouViewModelMessages.AnythingWeCanDoToSupportYourInterviewMessages.WhiteListErrorText)
                .When(x => x.RequiresSupportForInterview);
        }
    }
}