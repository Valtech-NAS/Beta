namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Candidate
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using FluentValidation.Attributes;
    using Validators;

    [Serializable]
    [Validator(typeof(AboutYouViewModelValidator))]
    public class AboutYouViewModel
    {
        [UIHint("FreetextLimited")]
        [Display(Name = "What are your strengths?")]
        public string WhatAreYourStrengths { get; set; }

        [UIHint("FreetextLimited")]
        [Display(Name = "Where do you feel you could improve?")]
        public string WhatDoYouFeelYouCouldImprove { get; set; }

        [UIHint("FreetextLimited")]
        [Display(Name = "What are your hobbies/interests?")]
        public string WhatAreYourHobbiesInterests { get; set; }

        [UIHint("FreetextLimited")]
        [Display(Name = "Is there anything we can do to support your interview?")]
        public string AnythingWeCanDoToSupportYourInterview { get; set; }
    }
}