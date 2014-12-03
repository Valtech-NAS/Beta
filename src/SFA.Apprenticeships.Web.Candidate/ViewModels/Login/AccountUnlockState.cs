namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Login
{
    public enum AccountUnlockState
    {
        Ok,
        UserInIncorrectState,
        AccountUnlockCodeExpired,
        AccountEmailAddressOrUnlockCodeInvalid,
        Error
    }
}