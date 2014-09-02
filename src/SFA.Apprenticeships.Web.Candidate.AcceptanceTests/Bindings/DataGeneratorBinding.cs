namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Bindings
{
    using Domain.Interfaces.Repositories;
    using Generators;
    using global::SpecBind.Helpers;
    using StructureMap;
    using System.Linq;
    using TechTalk.SpecFlow;

    [Binding]
    public class DataGeneratorBinding : Steps
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

        [Given(@"I have registered a new candidate")]
        public void GivenIHaveRegisteredANewCandidate()
        {
            Given("I navigated to the RegisterCandidatePage page");
            When("I have created a new email address");
            And("I enter data", GetRegistrationData());
            And("I choose HasAcceptedTermsAndConditions");
            And("I choose FindAddresses");
            And("I wait for 30 seconds to see AddressSelectLabel");
            And("I am on AddressDropdown list item matching criteria", GetAddressMatchingCriteria());
            And("I choose WrappedElement");
            And("I am on the RegisterCandidatePage page");
            And("I choose CreateAccountButton");
            Then("I wait 300 second for the ActivationPage page");
            When("I get the token for my newly created account");
            And("I enter data", GetActivationCodeData());
            And("I choose ActivateButton");
            Then("I am on the VacancySearchPage page"); 
        }

        private Table GetRegistrationData()
        {
            string[] header = { "Field", "Value" };
            string[] row1 = { "Firstname", "Firstname" };
            string[] row2 = { "Lastname", "Lastname" };
            string[] row3 = { "Phonenumber", "07970523193" };
            string[] row4 = { "EmailAddress", "{EmailToken}" };
            string[] row5 = { "PostcodeSearch", "N7 8LS  " };
            string[] row6 = { "Day", "01" };
            string[] row7 = { "Month", "01" };
            string[] row8 = { "Year", "2000" };
            string[] row9 = { "Password", "?Password01!" };

            var t = new Table(header);
            t.AddRow(row1);
            t.AddRow(row2);
            t.AddRow(row3);
            t.AddRow(row4);
            t.AddRow(row5);
            t.AddRow(row6);
            t.AddRow(row7);
            t.AddRow(row8);
            t.AddRow(row9);

            return t;
        }

        private Table GetAddressMatchingCriteria()
        {
            string[] header = { "Field", "Rule", "Value" };
            string[] row1 = { "Text", "Equals", "Flat A, 6 Furlong Road" };
            var t = new Table(header);
            t.AddRow(row1);
            return t;
        }

        private Table GetActivationCodeData()
        {
            string[] header = { "Field", "Value" };
            string[] row1 = { "ActivationCode", "{ActivationToken}" };
            var t = new Table(header);
            t.AddRow(row1);
            return t;
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
