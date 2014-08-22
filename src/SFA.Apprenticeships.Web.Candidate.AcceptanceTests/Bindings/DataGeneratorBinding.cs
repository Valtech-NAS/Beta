namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Bindings
{
    using Domain.Interfaces.Repositories;
    using Generators;
    using global::SpecBind.Helpers;
    using StructureMap;
    using System.Linq;
    using TechTalk.SpecFlow;

    [Binding]
    public class DataGeneratorBinding
    {
        private const string EmailTokenId = "EmailToken";
        private const string ActivationTokenId = "ActivationToken";
        private const string InvalidPasswordTokenName = "InvalidPasswordToken";
        private const string Password = "?Password01!"; //TODO: remove duplication?
        private readonly ITokenManager _tokenManager;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserWriteRepository _userWriteRepository;
        private string _email;

        public DataGeneratorBinding(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
            _userReadRepository = ObjectFactory.GetInstance<IUserReadRepository>();
            _userWriteRepository = ObjectFactory.GetInstance<IUserWriteRepository>();
        }

        [Given("I have created a new email address")]
        [When("I have created a new email address")]
        public void GivenICreateANewUserEmailAddress()
        {
            _email = EmailGenerator.GenerateEmailAddress();
            _tokenManager.SetToken(EmailTokenId, _email);
            _tokenManager.SetToken(InvalidPasswordTokenName, new string(Password.Reverse().ToArray()));
        }

        [When("I get the token for my newly created account")]
        public void WhenIGetTokenForMyNewlyCreatedAccount()
        {
            var email = _tokenManager.GetTokenByKey(EmailTokenId);
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
