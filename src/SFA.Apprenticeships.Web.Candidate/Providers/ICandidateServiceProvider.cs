namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using ViewModels.Login;
    using ViewModels.Register;
    public interface ICandidateServiceProvider
    {
        Candidate Register(RegisterViewModel model);
        bool Activate(ActivationViewModel model);
        bool IsUsernameAvailable(string username);
        Domain.Entities.Candidates.Candidate Authenticate(LoginViewModel model);
        UserStatuses GetUserStatus(string username);
        string[] GetRoles(string username);
    }
}
