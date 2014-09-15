﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.18444
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Features.Login
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Login Candidate")]
    [NUnit.Framework.CategoryAttribute("US415")]
    [NUnit.Framework.CategoryAttribute("US458")]
    public partial class LoginCandidateFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "LoginCandidate.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Login Candidate", "As a candidate,\r\nI want to sign in so that I can access my profile,\r\napply for ap" +
                    "prenticeships etc.", ProgrammingLanguage.CSharp, new string[] {
                        "US415",
                        "US458"});
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 9
#line 10
 testRunner.Given("I navigated to the HomePage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 11
 testRunner.When("I am on the HomePage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("As a candidate all required fields are present")]
        [NUnit.Framework.CategoryAttribute("US415")]
        public virtual void AsACandidateAllRequiredFieldsArePresent()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("As a candidate all required fields are present", new string[] {
                        "US415"});
#line 14
this.ScenarioSetup(scenarioInfo);
#line 9
this.FeatureBackground();
#line 15
 testRunner.Given("I navigated to the LoginPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 16
 testRunner.When("I am on the LoginPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 17
 testRunner.Then("I wait to see EmailAddress", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 18
 testRunner.And("I wait to see Password", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table1.AddRow(new string[] {
                        "ValidationSummary",
                        "Does Not Exist",
                        ""});
#line 19
 testRunner.And("I see", ((string)(null)), table1, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("As a candidate I can login with a registered and activated email address and pass" +
            "word")]
        [NUnit.Framework.CategoryAttribute("US415")]
        public virtual void AsACandidateICanLoginWithARegisteredAndActivatedEmailAddressAndPassword()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("As a candidate I can login with a registered and activated email address and pass" +
                    "word", new string[] {
                        "US415"});
#line 24
this.ScenarioSetup(scenarioInfo);
#line 9
this.FeatureBackground();
#line 25
 testRunner.Given("I registered an account and activated it", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 26
 testRunner.And("I navigated to the LoginPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 27
 testRunner.When("I am on the LoginPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table2.AddRow(new string[] {
                        "EmailAddress",
                        "{EmailAddressToken}"});
            table2.AddRow(new string[] {
                        "Password",
                        "{PasswordToken}"});
#line 28
 testRunner.And("I enter data", ((string)(null)), table2, "And ");
#line 32
 testRunner.And("I choose SignInButton", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 33
 testRunner.Then("I am on the MyApplicationsPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("As a candidate I must provide an email address and password")]
        [NUnit.Framework.CategoryAttribute("US415")]
        public virtual void AsACandidateIMustProvideAnEmailAddressAndPassword()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("As a candidate I must provide an email address and password", new string[] {
                        "US415"});
#line 36
this.ScenarioSetup(scenarioInfo);
#line 9
this.FeatureBackground();
#line 37
 testRunner.Given("I navigated to the LoginPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 38
 testRunner.When("I am on the LoginPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 39
 testRunner.And("I choose SignInButton", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 40
 testRunner.And("I am on the LoginPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table3.AddRow(new string[] {
                        "ValidationSummaryCount",
                        "Equals",
                        "2"});
#line 41
 testRunner.Then("I see", ((string)(null)), table3, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("As a candidate I want to be redirected to the previous page when I login")]
        [NUnit.Framework.CategoryAttribute("US415")]
        public virtual void AsACandidateIWantToBeRedirectedToThePreviousPageWhenILogin()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("As a candidate I want to be redirected to the previous page when I login", new string[] {
                        "US415"});
#line 45
this.ScenarioSetup(scenarioInfo);
#line 9
this.FeatureBackground();
#line 46
 testRunner.Given("I registered an account and activated it", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 47
 testRunner.And("I navigated to the VacancySearchPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 48
 testRunner.When("I choose SignInLink", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 49
 testRunner.And("I am on the LoginPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table4.AddRow(new string[] {
                        "EmailAddress",
                        "{EmailAddressToken}"});
            table4.AddRow(new string[] {
                        "Password",
                        "{PasswordToken}"});
#line 50
 testRunner.And("I enter data", ((string)(null)), table4, "And ");
#line 54
 testRunner.And("I choose SignInButton", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 55
 testRunner.Then("I am on the VacancySearchPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("As a candidate I cannot login with an invalid password")]
        [NUnit.Framework.CategoryAttribute("US415")]
        public virtual void AsACandidateICannotLoginWithAnInvalidPassword()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("As a candidate I cannot login with an invalid password", new string[] {
                        "US415"});
#line 58
this.ScenarioSetup(scenarioInfo);
#line 9
this.FeatureBackground();
#line 59
 testRunner.Given("I registered an account and activated it", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 60
 testRunner.And("I navigated to the LoginPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 61
 testRunner.When("I am on the LoginPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table5.AddRow(new string[] {
                        "EmailAddress",
                        "{EmailAddressToken}"});
            table5.AddRow(new string[] {
                        "Password",
                        "{InvalidPasswordToken}"});
#line 62
 testRunner.And("I enter data", ((string)(null)), table5, "And ");
#line 66
 testRunner.And("I choose SignInButton", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 67
 testRunner.And("I wait to see ValidationSummary", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table6.AddRow(new string[] {
                        "ValidationSummaryCount",
                        "Equals",
                        "1"});
#line 68
 testRunner.Then("I see", ((string)(null)), table6, "Then ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table7.AddRow(new string[] {
                        "Text",
                        "Equals",
                        "Please enter a valid email address or password"});
            table7.AddRow(new string[] {
                        "Href",
                        "Equals",
                        "#"});
#line 71
 testRunner.And("I am on ValidationSummaryItems list item matching criteria", ((string)(null)), table7, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("As a candidate I can login with a registered but unactivated account and am redir" +
            "ected to the activation page")]
        [NUnit.Framework.CategoryAttribute("US458")]
        [NUnit.Framework.CategoryAttribute("US444")]
        [NUnit.Framework.CategoryAttribute("US456")]
        public virtual void AsACandidateICanLoginWithARegisteredButUnactivatedAccountAndAmRedirectedToTheActivationPage()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("As a candidate I can login with a registered but unactivated account and am redir" +
                    "ected to the activation page", new string[] {
                        "US458",
                        "US444",
                        "US456"});
#line 77
this.ScenarioSetup(scenarioInfo);
#line 9
this.FeatureBackground();
#line 78
 testRunner.Given("I registered an account but did not activate it", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 79
 testRunner.And("I navigated to the LoginPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 80
 testRunner.When("I am on the LoginPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table8.AddRow(new string[] {
                        "EmailAddress",
                        "{EmailAddressToken}"});
            table8.AddRow(new string[] {
                        "Password",
                        "{PasswordToken}"});
#line 81
 testRunner.And("I enter data", ((string)(null)), table8, "And ");
#line 85
 testRunner.And("I choose SignInButton", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 86
 testRunner.And("I am on the ActivationPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table9.AddRow(new string[] {
                        "ActivationCode",
                        "{ActivationCodeToken}"});
#line 87
 testRunner.And("I enter data", ((string)(null)), table9, "And ");
#line 90
 testRunner.And("I choose ActivateButton", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 91
 testRunner.Then("I am on the VacancySearchPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Reset password after locking an account does not have to unlock the account")]
        [NUnit.Framework.IgnoreAttribute()]
        [NUnit.Framework.CategoryAttribute("US458")]
        [NUnit.Framework.CategoryAttribute("US457")]
        public virtual void ResetPasswordAfterLockingAnAccountDoesNotHaveToUnlockTheAccount()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Reset password after locking an account does not have to unlock the account", new string[] {
                        "ignore",
                        "US458",
                        "US457"});
#line 96
this.ScenarioSetup(scenarioInfo);
#line 9
this.FeatureBackground();
#line 97
 testRunner.Given("I have registered a new candidate", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 98
 testRunner.When("I choose SignoutLink", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 99
 testRunner.Then("I am on the LoginPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table10.AddRow(new string[] {
                        "EmailAddress",
                        "{EmailToken}"});
            table10.AddRow(new string[] {
                        "Password",
                        "{InvalidPasswordToken}"});
#line 100
 testRunner.When("I enter data", ((string)(null)), table10, "When ");
#line 104
 testRunner.And("I choose SignInButton", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 105
 testRunner.And("I am on the LoginPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 106
 testRunner.And("I wait for 300 seconds to see ValidationSummary", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table11.AddRow(new string[] {
                        "ValidationSummaryCount",
                        "Equals",
                        "1"});
#line 107
 testRunner.Then("I see", ((string)(null)), table11, "Then ");
#line hidden
            TechTalk.SpecFlow.Table table12 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table12.AddRow(new string[] {
                        "Text",
                        "Equals",
                        "Please enter a valid email address or password"});
            table12.AddRow(new string[] {
                        "Href",
                        "Equals",
                        "#"});
#line 110
 testRunner.And("I am on ValidationSummaryItems list item matching criteria", ((string)(null)), table12, "And ");
#line 114
 testRunner.And("I am on the LoginPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table13 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table13.AddRow(new string[] {
                        "Password",
                        "{InvalidPasswordToken}"});
#line 115
 testRunner.When("I enter data", ((string)(null)), table13, "When ");
#line 119
 testRunner.And("I choose SignInButton", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 121
 testRunner.And("I choose SignInButton", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 122
 testRunner.And("I am on the LoginPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 123
 testRunner.And("I wait for 300 seconds to see ValidationSummary", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table14 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table14.AddRow(new string[] {
                        "ValidationSummaryCount",
                        "Equals",
                        "1"});
#line 124
 testRunner.Then("I see", ((string)(null)), table14, "Then ");
#line hidden
            TechTalk.SpecFlow.Table table15 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table15.AddRow(new string[] {
                        "Text",
                        "Equals",
                        "Please enter a valid email address or password"});
            table15.AddRow(new string[] {
                        "Href",
                        "Equals",
                        "#"});
#line 127
 testRunner.And("I am on ValidationSummaryItems list item matching criteria", ((string)(null)), table15, "And ");
#line 131
 testRunner.And("I am on the LoginPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table16 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table16.AddRow(new string[] {
                        "Password",
                        "{InvalidPasswordToken}"});
#line 132
 testRunner.When("I enter data", ((string)(null)), table16, "When ");
#line 136
 testRunner.When("I choose SignInButton", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 138
 testRunner.And("I choose SignInButton", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 139
 testRunner.Then("I wait 180 second for the UnlockPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 140
 testRunner.And("I get the account unlock code", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table17 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table17.AddRow(new string[] {
                        "EmailAddressText",
                        "Equals",
                        "{EmailToken}"});
            table17.AddRow(new string[] {
                        "AccountUnlockCode",
                        "Exists",
                        ""});
#line 141
 testRunner.And("I see", ((string)(null)), table17, "And ");
#line 145
 testRunner.And("the user login incorrect attempts should be three", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 146
 testRunner.When("I navigate to the ForgottenPasswordPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 147
 testRunner.Then("I am on the ForgottenPasswordPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table18 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table18.AddRow(new string[] {
                        "EmailAddress",
                        "{EmailToken}"});
#line 148
 testRunner.And("I enter data", ((string)(null)), table18, "And ");
#line 151
 testRunner.When("I choose SendCodeButton", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 152
 testRunner.Then("I am on the ResetPasswordPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 153
 testRunner.And("the user login incorrect attempts should be three", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 154
 testRunner.And("the account unlock code and date should be set", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 155
 testRunner.And("the password reset code and date should be set", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 156
 testRunner.When("I navigate to the LoginPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 157
 testRunner.Then("I am on the LoginPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table19 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table19.AddRow(new string[] {
                        "EmailAddress",
                        "{EmailToken}"});
            table19.AddRow(new string[] {
                        "Password",
                        "?Password01!"});
#line 158
 testRunner.When("I enter data", ((string)(null)), table19, "When ");
#line 162
 testRunner.And("I choose SignInButton", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 163
 testRunner.Then("I am on the UnlockPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 164
 testRunner.When("I navigate to the LoginPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 165
 testRunner.Then("I am on the LoginPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table20 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table20.AddRow(new string[] {
                        "EmailAddress",
                        "{EmailToken}"});
            table20.AddRow(new string[] {
                        "Password",
                        "{InvalidPasswordToken}"});
#line 166
 testRunner.When("I enter data", ((string)(null)), table20, "When ");
#line 170
 testRunner.And("I choose SignInButton", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 171
 testRunner.Then("I am on the UnlockPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 172
 testRunner.And("the account unlock code and date should be set", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 173
 testRunner.And("the password reset code and date should be set", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 174
 testRunner.When("I choose ResendAccountUnlockCodeLink", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 175
 testRunner.Then("I get the same account unlock code", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 176
 testRunner.When("I navigate to the LoginPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 177
 testRunner.Then("I am on the LoginPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table21 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table21.AddRow(new string[] {
                        "EmailAddress",
                        "{EmailToken}"});
            table21.AddRow(new string[] {
                        "Password",
                        "?Password01!"});
#line 178
 testRunner.When("I enter data", ((string)(null)), table21, "When ");
#line 182
 testRunner.And("I choose SignInButton", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 183
 testRunner.Then("I am on the UnlockPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
