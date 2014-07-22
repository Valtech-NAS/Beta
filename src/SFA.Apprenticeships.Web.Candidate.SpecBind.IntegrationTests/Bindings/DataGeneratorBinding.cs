namespace SFA.Apprenticeships.Web.Candidate.SpecBind.IntegrationTests.Bindings
{
    using System;
    using System.Configuration;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using global::SpecBind.Helpers;
    using Infrastructure.Common.IoC;
    using Infrastructure.Repositories.Users.IoC;
    using StructureMap;
    using TechTalk.SpecFlow;

    [Binding]
    public class DataGeneratorBinding
    {
        private const string EmailTokenId = "EmailToken";
        private const string ActivationTokenId = "ActivationToken";

        private readonly ITokenManager _tokenManager;
        private readonly IUserReadRepository _userReadRepository;

        public DataGeneratorBinding(ITokenManager tokenManager)
        {
            //Lookup email...
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<UserRepositoryRegistry>();
            });

            _tokenManager = tokenManager;
            _userReadRepository = ObjectFactory.GetInstance<IUserReadRepository>();
        }

        [Given("I have created a new email address")]
        public void GivenICreateANewUserEmailAddress()
        {
            var rnd = new Random();
            var emailSuffix = rnd.Next(10000, 99999);
            var email = string.Format("specflowtest{0}@test.test", emailSuffix);
            // Do some database stuff here
            _tokenManager.SetToken(EmailTokenId, email);
        }

        [Then("I get the token for my newly created account")]
        public void GivenIGetTokenForMyNewlyCreatedAccount(Table emailTable)
        {
            if (emailTable.RowCount != 1 || !emailTable.ContainsColumn("Email"))
            {
                throw new ConfigurationErrorsException("Email address table should only contain 1 row with Email column");
            }

            var email = emailTable.Rows[0]["Email"];

            var user = _userReadRepository.Get(email);

            if (user != null)
            {
                _tokenManager.SetToken(ActivationTokenId, user.ActivationCode);
            }
        }
    }
}