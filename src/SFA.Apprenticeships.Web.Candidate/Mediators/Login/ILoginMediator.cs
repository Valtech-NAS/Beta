namespace SFA.Apprenticeships.Web.Candidate.Mediators.Login
{
    using ViewModels.Login;

    public interface ILoginMediator
    {
        MediatorResponse<LoginResultViewModel> Index(LoginViewModel viewModel);

        MediatorResponse<AccountUnlockViewModel> Unlock(AccountUnlockViewModel accountUnlockView);

        MediatorResponse<AccountUnlockViewModel> Resend(AccountUnlockViewModel accountUnlockViewModel);
    }
}