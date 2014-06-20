﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.18408
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace SFA.Apprenticeships.Web.Candidate.IntegrationTests.SpecFlow.Features.Vacancies
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("VacancySearchNoJS")]
    [NUnit.Framework.CategoryAttribute("browser:disableJavaScript")]
    [NUnit.Framework.CategoryAttribute("vacancysearch")]
    [NUnit.Framework.CategoryAttribute("US58")]
    [NUnit.Framework.CategoryAttribute("US230")]
    [NUnit.Framework.CategoryAttribute("US258")]
    public partial class VacancySearchNoJSFeature : FluentAutomation.FluentTest
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "VacancySearchNoJS.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "VacancySearchNoJS", "In order to confirm a candidate can search for vacancies\r\nas a candidate\r\nI want " +
                    "to be find relevant vacancies in my area", ProgrammingLanguage.CSharp, new string[] {
                        "browser:disableJavaScript",
                        "vacancysearch",
                        "US58",
                        "US230",
                        "US258"});
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
            ScenarioContext.Current[ScenarioContext.Current.ScenarioInfo.Title] = this;
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Search for apprenticeships in my area")]
        [NUnit.Framework.TestCaseAttribute("", null)]
        [NUnit.Framework.TestCaseAttribute("admin", null)]
        public virtual void SearchForApprenticeshipsInMyArea(string search_Keywords, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Search for apprenticeships in my area", exampleTags);
#line 7
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Location",
                        "Distance"});
            table1.AddRow(new string[] {
                        "Warwick",
                        "10 miles"});
#line 8
 testRunner.Given("I am a candidate with preferences", ((string)(null)), table1, "Given ");
#line 11
 testRunner.And(string.Format("I enhance my search with the following \'{0}\'", search_Keywords), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 12
 testRunner.When("I search for vacancies", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 13
 testRunner.Then("I expect to see search results", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Search for apprenticeships - clear my criteria")]
        [NUnit.Framework.TestCaseAttribute("admin and other keywords", null)]
        public virtual void SearchForApprenticeships_ClearMyCriteria(string search_Keywords, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Search for apprenticeships - clear my criteria", exampleTags);
#line 19
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Location",
                        "Distance"});
            table2.AddRow(new string[] {
                        "Warwick",
                        "10 miles"});
#line 20
 testRunner.Given("I am a candidate with preferences", ((string)(null)), table2, "Given ");
#line 23
 testRunner.When("I clear my search criteria", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 24
 testRunner.Then("I expect to see the search page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 25
 testRunner.And("all search fields are reset", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Search for apprenticeships - unspecified location")]
        public virtual void SearchForApprenticeships_UnspecifiedLocation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Search for apprenticeships - unspecified location", ((string[])(null)));
#line 30
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Location",
                        "Distance"});
            table3.AddRow(new string[] {
                        "Warwick",
                        "10 miles"});
#line 31
 testRunner.Given("I am a candidate with preferences", ((string)(null)), table3, "Given ");
#line 34
 testRunner.When("I search for vacancies", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "field_name",
                        "message"});
            table4.AddRow(new string[] {
                        "location",
                        "something to say about it"});
#line 35
 testRunner.Then("I expect to see a validation message", ((string)(null)), table4, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
