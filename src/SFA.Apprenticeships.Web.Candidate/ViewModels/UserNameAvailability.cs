namespace SFA.Apprenticeships.Web.Candidate.ViewModels
{
    public class UserNameAvailability
    {
        public string ErrorMessage { get; set; }

        public bool HasError { get; set; }

        public bool IsUserNameAvailable { get; set; }
    }
}