namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using ViewModels.Login;
    using ViewModels.Register;

    public interface ICandidateServiceProvider
    {
        int? LastViewedVacancyId { get; set; }
        Candidate Register(RegisterViewModel model);
        bool Activate(ActivationViewModel model);
        bool IsUsernameAvailable(string username);
        Candidate Authenticate(LoginViewModel model);
        UserStatuses GetUserStatus(string username);
        string[] GetRoles(string username);

        void RequestForgottenPasswordReset(ForgottenPasswordViewModel model);

        bool VerifyPasswordReset(PasswordResetViewModel model);
    }
}