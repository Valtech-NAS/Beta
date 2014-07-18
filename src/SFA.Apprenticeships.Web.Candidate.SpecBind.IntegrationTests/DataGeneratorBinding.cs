namespace SFA.Apprenticeships.Web.Candidate.SpecBind.IntegrationTests
{
    using System;
    using global::SpecBind.Helpers;
    using TechTalk.SpecFlow;

    [Binding]
    public class DataGeneratorBinding
    {
        private const string EmailTokenId = "EmailToken";

        private readonly ITokenManager tokenManager;

        public DataGeneratorBinding(ITokenManager tokenManager)
        {
            this.tokenManager = tokenManager;
        }

        [Given("I have created a new email address")]
        public void GivenICreateANewUserEmailAddress()
        {
            var rnd = new Random();
            var emailSuffix = rnd.Next(10000, 99999);
            var email = string.Format("specflowtest{0}@test.test", emailSuffix);
            // Do some database stuff here
            this.tokenManager.SetToken(EmailTokenId, email);
        }
    }
}