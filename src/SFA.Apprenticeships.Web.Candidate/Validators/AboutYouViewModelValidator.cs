namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using FluentValidation;
    using ViewModels.Candidate;

    public class AboutYouViewModelValidator : AbstractValidator<AboutYouViewModel>
    {
        public AboutYouViewModelValidator()
        {
            RuleFor(x => x.WhatAreYourStrengths)
                .Length(0, 4000)
                .WithMessage("'What are your strengths' must not exceed 4000 characters");

            RuleFor(x => x.WhatDoYouFeelYouCouldImprove)
                .Length(0, 4000)
                .WithMessage("'What do you feel you could improve' must not exceed 4000 characters");

            RuleFor(x => x.WhatAreYourHobbiesInterests)
                .Length(0, 4000)
                .WithMessage("'What are your hobbies/interests' must not exceed 4000 characters");

            RuleFor(x => x.AnythingWeCanDoToSupportYourInterview)
                .Length(0, 4000)
                .WithMessage("'Is there anything we can do to support your interview' must not exceed 4000 characters");

            RuleFor(x => x.WhatAreYourStrengths)
                .NotEmpty()
                .When(m => 
                    string.IsNullOrWhiteSpace(m.WhatDoYouFeelYouCouldImprove) && 
                    string.IsNullOrWhiteSpace(m.WhatAreYourHobbiesInterests) &&
                    string.IsNullOrWhiteSpace(m.AnythingWeCanDoToSupportYourInterview))
                .WithMessage("You must answer at least one of the 'About you' questions");
        }
    }
}