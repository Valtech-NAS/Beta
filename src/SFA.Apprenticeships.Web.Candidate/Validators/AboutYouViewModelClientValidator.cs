namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Candidate;

    public class AboutYouViewModelClientValidator : AbstractValidator<AboutYouViewModel>
    {
        public AboutYouViewModelClientValidator()
        {
            this.AddMandatoryRules();
            this.AddSaveRules();
        }
    }

    public class AboutYouViewModelSaveValidator : AbstractValidator<AboutYouViewModel>
    {
        public AboutYouViewModelSaveValidator()
        {
            this.AddSaveRules();
            this.AddConditionalRules();
        }
    }

    public class AboutYouViewModelServerValidator : AbstractValidator<AboutYouViewModel>
    {
        public AboutYouViewModelServerValidator()
        {
            this.AddMandatoryRules();
            this.AddSaveRules();
            this.AddConditionalRules();
        }
    }

    internal static class AboutYouValidationRules
    {
        internal static void AddMandatoryRules(this AbstractValidator<AboutYouViewModel> validator)
        {
            validator.RuleFor(x => x.WhatAreYourStrengths)
                .NotEmpty()
                .WithMessage(AboutYouViewModelMessages.WhatAreYourStrengthsMessages.RequiredErrorText);

            validator.RuleFor(x => x.WhatDoYouFeelYouCouldImprove)
                .NotEmpty()
                .WithMessage(AboutYouViewModelMessages.WhatDoYouFeelYouCouldImproveMessages.RequiredErrorText);

            validator.RuleFor(x => x.WhatAreYourHobbiesInterests)
                .NotEmpty()
                .WithMessage(AboutYouViewModelMessages.WhatAreYourHobbiesInterestsMessages.RequiredErrorText);
        }

        internal static void AddSaveRules(this AbstractValidator<AboutYouViewModel> validator)
        {
            validator.RuleFor(x => x.WhatAreYourStrengths)
                .Length(0, 4000)
                .WithMessage(AboutYouViewModelMessages.WhatAreYourStrengthsMessages.TooLongErrorText)
                .Matches(AboutYouViewModelMessages.WhatAreYourStrengthsMessages.WhiteListRegularExpression)
                .WithMessage(AboutYouViewModelMessages.WhatAreYourStrengthsMessages.WhiteListErrorText);

            validator.RuleFor(x => x.WhatDoYouFeelYouCouldImprove)
                .Length(0, 4000)
                .WithMessage(AboutYouViewModelMessages.WhatDoYouFeelYouCouldImproveMessages.TooLongErrorText)
                .Matches(AboutYouViewModelMessages.WhatDoYouFeelYouCouldImproveMessages.WhiteListRegularExpression)
                .WithMessage(AboutYouViewModelMessages.WhatDoYouFeelYouCouldImproveMessages.WhiteListErrorText);

            validator.RuleFor(x => x.WhatAreYourHobbiesInterests)
                .Length(0, 4000)
                .WithMessage(AboutYouViewModelMessages.WhatAreYourHobbiesInterestsMessages.TooLongErrorText)
                .Matches(AboutYouViewModelMessages.WhatAreYourHobbiesInterestsMessages.WhiteListRegularExpression)
                .WithMessage(AboutYouViewModelMessages.WhatAreYourHobbiesInterestsMessages.WhiteListErrorText);          
        }

        internal static void AddConditionalRules(this AbstractValidator<AboutYouViewModel> validator)
        {
            validator.RuleFor(x => x.AnythingWeCanDoToSupportYourInterview)
               .NotEmpty()
               .WithMessage(AboutYouViewModelMessages.AnythingWeCanDoToSupportYourInterviewMessages.RequiredErrorText)
               .Length(0, 4000)
               .WithMessage(AboutYouViewModelMessages.AnythingWeCanDoToSupportYourInterviewMessages.TooLongErrorText)
               .Matches(
                   AboutYouViewModelMessages.AnythingWeCanDoToSupportYourInterviewMessages.WhiteListRegularExpression)
               .WithMessage(AboutYouViewModelMessages.AnythingWeCanDoToSupportYourInterviewMessages.WhiteListErrorText)
               .When(x => x.RequiresSupportForInterview);
        }
    }
}