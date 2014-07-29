namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using ViewModels.Login;
    using ViewModels.Register;

    public interface ICandidateServiceProvider
    {
        bool Register(RegisterViewModel model);
        int? LastViewedVacancyId { get; set; }
        bool Activate(ActivationViewModel model, string candidateId);
        bool IsUsernameAvailable(string username);
        UserStatuses GetUserStatus(string username);
        bool Authenticate(LoginViewModel model);
        void RequestForgottenPasswordReset(ForgottenPasswordViewModel model);
        bool VerifyPasswordReset(PasswordResetViewModel model);
        bool VerifyAccountUnlockCode(AccountUnlockViewModel model);
    }
}
