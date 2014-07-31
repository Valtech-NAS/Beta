namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Login
{
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Validators;

    [Validator(typeof(AccountUnlockViewModelClientValidator))]
    public class AccountUnlockViewModel
    {
        public string EmailAddress { get; set; }

        [Display(Name = AccountUnlockViewModelMessages.AccountUnlockCodeMessages.LabelText)]
        public string AccountUnlockCode { get; set; }
    }
}
