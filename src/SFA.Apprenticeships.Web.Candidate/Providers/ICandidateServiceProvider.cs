namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using Domain.Entities.Applications;
    using Domain.Entities.Users;
    using Domain.Entities.Candidates;
    using ViewModels.Login;
    using ViewModels.Register;

    public interface ICandidateServiceProvider
    {
        bool Register(RegisterViewModel model);
        int? LastViewedVacancyId { get; set; }
        bool Activate(ActivationViewModel model, string candidateId);
        bool IsUsernameAvailable(string username);
        UserStatuses GetUserStatus(string username);
        ApplicationStatuses? GetApplicationStatus(Guid candidateId, int vacancyId);
        Candidate Authenticate(LoginViewModel model);
        void RequestForgottenPasswordResetCode(ForgottenPasswordViewModel model);
        void RequestAccountUnlockCode(AccountUnlockViewModel model);
        bool VerifyPasswordReset(PasswordResetViewModel model);
        bool VerifyAccountUnlockCode(AccountUnlockViewModel model);
        bool ResendActivationCode(string username);
        Candidate GetCandidate(string username);
        Candidate GetCandidate(Guid candidateId);
    }
}
