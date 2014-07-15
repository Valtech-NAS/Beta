namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Register
{
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using Validators;
    using FluentValidation.Attributes;

    [Validator(typeof(ActivationViewModelClientValidator))]
    public class ActivationViewModel
    {
        public ActivationViewModel()
        { }

        public string EmailAddress { get; set; }

        [Display(Name = ActivationCodeMessages.ActivationCode.LabelText, Description = ActivationCodeMessages.ActivationCode.HintText)]
        public string ActivationCode { get; set; }

        public bool IsActivated { get; set; }
    }
}