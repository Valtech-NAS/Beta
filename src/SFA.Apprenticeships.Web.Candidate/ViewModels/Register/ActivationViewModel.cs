namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Register
{
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using Validators;
    using FluentValidation.Attributes;

    [Validator(typeof(ActivationViewModelClientValidator))]
    public class ActivationViewModel : ViewModelBase
    {
        public string EmailAddress { get; set; }

        [Display(Name = ActivationCodeMessages.ActivationCode.LabelText, Description = ActivationCodeMessages.ActivationCode.HintText)]
        public string ActivationCode { get; set; }

        //public bool IsActivated { get; set; }
        public ActivateUserState State { get; set; }

        public ActivationViewModel() : base() { }

        public ActivationViewModel(string emailAddress, string activationCode, ActivateUserState state)
        {
            EmailAddress = emailAddress;
            ActivationCode = activationCode;
            State = state;
        }

        public ActivationViewModel(string emailAddress, string activationCode, ActivateUserState state, string viewModelMessage) 
            : base(viewModelMessage)
        {
            EmailAddress = emailAddress;
            ActivationCode = activationCode;
            State = state;
        }
    }

    public enum ActivateUserState
    {
        Activated,
        InvalidCode,
        Error
    }
}
