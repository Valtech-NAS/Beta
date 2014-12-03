namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Login
{
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Validators;

    [Validator(typeof(AccountUnlockViewModelClientValidator))]
    public class AccountUnlockViewModel : ViewModelBase
    {
        public AccountUnlockViewModel()
        {
        }

        public AccountUnlockViewModel(string message)
            : base(message)
        {
        }

        [Display(Name = AccountUnlockViewModelMessages.AccountUnlockCodeMessages.EmailLabelText)]
        public string EmailAddress { get; set; }

        [Display(Name = AccountUnlockViewModelMessages.AccountUnlockCodeMessages.LabelText)]
        public string AccountUnlockCode { get; set; }

        public AccountUnlockState Status { get; set; }
    }
}