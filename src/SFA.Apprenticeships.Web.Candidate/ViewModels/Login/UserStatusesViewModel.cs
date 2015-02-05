namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Login
{
    using Domain.Entities.Users;

    public class UserStatusesViewModel : ViewModelBase
    {
        public UserStatusesViewModel()
        {
        }

        public UserStatusesViewModel(string message) : base(message)
        {
        }

        public UserStatuses? UserStatus { get; set; }
    }
}