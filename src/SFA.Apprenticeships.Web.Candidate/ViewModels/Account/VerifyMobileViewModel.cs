namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Account
{
    using System.ComponentModel.DataAnnotations;
    using Applications;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Validators;

    [Validator(typeof(VerifyMobileViewModelClientValidator))]
    public class VerifyMobileViewModel : ViewModelBase
    {
        public VerifyMobileViewModel()
        {
        }

        public VerifyMobileViewModel(string message)
            : base(message)
        {
        }

        [Display(Name = VerifyMobileViewModelMessages.MobileNumberCodeMessages.LabelText)]
        public string PhoneNumber { get; set; }

        [Display(Name = VerifyMobileViewModelMessages.VerifyMobileCodeMessages.LabelText)]
        public string VerifyMobileCode { get; set; }

        public VerifyMobileState Status { get; set; }

        public TraineeshipFeatureViewModel TraineeshipFeature { get; set; }

        public string ReturnUrl { get; set; }
    }
}