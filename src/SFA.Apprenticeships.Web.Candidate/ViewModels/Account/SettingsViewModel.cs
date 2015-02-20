namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Account
{
    using System.ComponentModel.DataAnnotations;
    using Applications;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Locations;
    using Validators;

    [Validator(typeof(SettingsViewModelClientValidator))]
    public class SettingsViewModel
    {
        [Display(Name = SettingsViewModelMessages.FirstnameMessages.LabelText)]
        public string Firstname { get; set; }

        [Display(Name = SettingsViewModelMessages.LastnameMessages.LabelText)]
        public string Lastname { get; set; }

        public DateViewModel DateOfBirth { get; set; }

        public AddressViewModel Address { get; set; }

        [Display(Name = SettingsViewModelMessages.PhoneNumberMessages.LabelText)]
        public string PhoneNumber { get; set; }

        public bool VerifiedMobile { get; set; }

        [Display(Name = SettingsViewModelMessages.AllowEmailMessages.LabelText)]
        public bool AllowEmailComms { get; set; }

        [Display(Name = SettingsViewModelMessages.AllowSmsMessages.LabelText)]
        public bool AllowSmsComms { get; set; }

        public TraineeshipFeatureViewModel TraineeshipFeature { get; set; }

        public bool SmsEnabled { get; set; }

        [Display(Name = SettingsViewModelMessages.AllowEmailMessages.LabelText)]
        public bool AllowEmailMarketing { get; set; }

        [Display(Name = SettingsViewModelMessages.AllowSmsMessages.LabelText)]
        public bool AllowSmsMarketing { get; set; }
    }
}
