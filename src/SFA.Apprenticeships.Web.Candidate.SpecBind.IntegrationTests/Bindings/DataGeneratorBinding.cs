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
        private string _email;
        private readonly ITokenManager _tokenManager;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserWriteRepository _userWriteRepository;

        public DataGeneratorBinding(ITokenManager tokenManager)
        {
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<UserRepositoryRegistry>();
            });

            _tokenManager = tokenManager;
            _userReadRepository = ObjectFactory.GetInstance<IUserReadRepository>();
            _userWriteRepository = ObjectFactory.GetInstance<IUserWriteRepository>();
        }

        [Given("I have created a new email address")]
        public void GivenICreateANewUserEmailAddress()
        {
            var rnd = new Random();
            var emailSuffix = rnd.Next();
            var email = string.Format("specflowtest{0}@test.test", emailSuffix).ToLower();
            _email = email;
            _tokenManager.SetToken(EmailTokenId, email);
        }

        [When("I get the token for my newly created account")]
        public void WhenIGetTokenForMyNewlyCreatedAccount()
        {
            string email =_tokenManager.GetTokenByKey(EmailTokenId);
            
            var user = _userReadRepository.Get(email);

            if (user != null)
            {
                _tokenManager.SetToken(ActivationTokenId, user.ActivationCode);
            }
        }

        [Then("I set my token here")]
        public void ThenISetMyTokenHere()
        {
            var user = _userReadRepository.Get(_email);

            if (user != null)
            {
                _tokenManager.SetToken(ActivationTokenId, user.ActivationCode);
            }
        }
    }
}