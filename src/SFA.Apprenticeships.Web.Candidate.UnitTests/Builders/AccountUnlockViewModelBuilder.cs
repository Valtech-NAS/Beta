namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Builders
{
    using Candidate.ViewModels.Login;

    public class AccountUnlockViewModelBuilder
    {
        private readonly string _emailAddress;
        private readonly string _accountUnlockCode;

        public AccountUnlockViewModelBuilder(string emailAddress, string accountUnlockCode)
        {
            _emailAddress = emailAddress;
            _accountUnlockCode = accountUnlockCode;
        }

        public AccountUnlockViewModel Build()
        {
            var model = new AccountUnlockViewModel
            {
                EmailAddress = _emailAddress,
                AccountUnlockCode = _accountUnlockCode
            };

            return model;
        }
    }
}