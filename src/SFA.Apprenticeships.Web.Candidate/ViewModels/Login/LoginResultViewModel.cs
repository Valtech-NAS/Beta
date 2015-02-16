namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Login
{
    using Domain.Entities.Users;

    public class LoginResultViewModel : ViewModelBase
    {
        public LoginResultViewModel()
        {
        }

        public LoginResultViewModel(string message)
            : base(message)
        {
        }

        public string EmailAddress { get; set; }

        public string FullName { get; set; }

        public UserStatuses? UserStatus { get; set; }

        public bool IsAuthenticated { get; set; }

        public string AcceptedTermsAndConditionsVersion { get; set; }

        public bool MobileVerificationRequired { get; set; }
    }
}