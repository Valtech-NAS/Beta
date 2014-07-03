namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Candidate
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Validators;

    [Serializable]
    [Validator(typeof(AboutYouViewModelValidator))]
    public class AboutYouViewModel
    {
        [Display(Name = AboutYouMessages.WhatAreYourStrengthsMessages.LabelText, Description = AboutYouMessages.WhatAreYourStrengthsMessages.HintText)]
        public string WhatAreYourStrengths { get; set; }

        [Display(Name = AboutYouMessages.WhatDoYouFeelYouCouldImproveMessages.LabelText, Description = AboutYouMessages.WhatDoYouFeelYouCouldImproveMessages.HintText)]
        public string WhatDoYouFeelYouCouldImprove { get; set; }

        [Display(Name = AboutYouMessages.WhatAreYourHobbiesInterestsMessages.LabelText, Description = AboutYouMessages.WhatAreYourHobbiesInterestsMessages.HintText)]
        public string WhatAreYourHobbiesInterests { get; set; }

        [Display(Name = AboutYouMessages.AnythingWeCanDoToSupportYourInterviewMessages.LabelText, Description = AboutYouMessages.AnythingWeCanDoToSupportYourInterviewMessages.HintText)]
        public string AnythingWeCanDoToSupportYourInterview { get; set; }
    }
}