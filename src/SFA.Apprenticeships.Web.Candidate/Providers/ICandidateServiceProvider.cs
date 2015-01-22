namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using ViewModels.Login;
    using ViewModels.Register;
    using Constants.ViewModels;

    public interface ICandidateServiceProvider
    {
        bool Register(RegisterViewModel model);
        ActivationViewModel Activate(ActivationViewModel model, Guid candidateId);
        LoginResultViewModel Login(LoginViewModel model);
        UserNameAvailability IsUsernameAvailable(string username);
        UserStatusesViewModel GetUserStatus(string username);
        ApplicationStatuses? GetApplicationStatus(Guid candidateId, int vacancyId);
        bool RequestForgottenPasswordResetCode(ForgottenPasswordViewModel model);
        AccountUnlockViewModel RequestAccountUnlockCode(AccountUnlockViewModel model);
        PasswordResetViewModel VerifyPasswordReset(PasswordResetViewModel passwordResetViewModel);
        AccountUnlockViewModel VerifyAccountUnlockCode(AccountUnlockViewModel model);
        bool ResendActivationCode(string username);
        Candidate GetCandidate(string username);
        Candidate GetCandidate(Guid candidateId);
        bool AcceptTermsAndConditions(Guid candidateId, string currentVersion);
    }
}
