namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    using ViewModels.Login;

    public interface ILoginMediator
    {
        MediatorResponse Index(LoginViewModel viewModel);

        MediatorResponse<AccountUnlockViewModel> Unlock(AccountUnlockViewModel accountUnlockView);

        MediatorResponse<AccountUnlockViewModel> Resend(AccountUnlockViewModel accountUnlockViewModel);
    }
}