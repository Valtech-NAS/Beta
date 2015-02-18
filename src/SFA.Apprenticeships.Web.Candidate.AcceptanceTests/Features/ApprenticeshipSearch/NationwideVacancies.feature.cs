﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.34014
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Features.ApprenticeshipSearch
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Nationwide apprenticeships")]
    [NUnit.Framework.CategoryAttribute("US500")]
    public partial class NationwideApprenticeshipsFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "NationwideVacancies.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Nationwide apprenticeships", "As a candidate\r\nI want to be able to see apprenticeships that exist nationwide\r\ns" +
                    "o that I can see opportunities that may be of interest to me irrespective of my " +
                    "location", ProgrammingLanguage.CSharp, new string[] {
                        "US500"});
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
#line 7
#line 8
 testRunner.Given("I navigated to the ApprenticeshipSearchPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 9
 testRunner.And("I am logged out", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 10
 testRunner.And("I navigated to the ApprenticeshipSearchPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 11
 testRunner.Then("I am on the ApprenticeshipSearchPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("After clicking on nationwide apprenticeships I see them")]
        [NUnit.Framework.CategoryAttribute("SmokeTests")]
        public virtual void AfterClickingOnNationwideApprenticeshipsISeeThem()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("After clicking on nationwide apprenticeships I see them", new string[] {
                        "SmokeTests"});
#line 14
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 15
 testRunner.Given("I navigated to the ApprenticeshipSearchPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table1.AddRow(new string[] {
                        "Location",
                        "London"});
            table1.AddRow(new string[] {
                        "WithInDistance",
                        "40 miles"});
            table1.AddRow(new string[] {
                        "ApprenticeshipLevel",
                        "All levels"});
#line 16
 testRunner.When("I enter data", ((string)(null)), table1, "When ");
#line 21
 testRunner.And("I choose Search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 22
 testRunner.Then("I am on the ApprenticeshipSearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 23
 testRunner.When("I choose NationwideLocationTypeLink", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 24
 testRunner.Then("I am on the ApprenticeshipSearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table2.AddRow(new string[] {
                        "LocalLocationTypeLink",
                        "Exists",
                        ""});
            table2.AddRow(new string[] {
                        "NationwideLocationTypeLink",
                        "Does Not Exist",
                        ""});
#line 25
 testRunner.And("I see", ((string)(null)), table2, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Nationwide apprenticeships do not show distance")]
        [NUnit.Framework.CategoryAttribute("SmokeTests")]
        public virtual void NationwideApprenticeshipsDoNotShowDistance()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Nationwide apprenticeships do not show distance", new string[] {
                        "SmokeTests"});
#line 32
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 33
 testRunner.Given("I navigated to the ApprenticeshipSearchPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table3.AddRow(new string[] {
                        "Location",
                        "London"});
            table3.AddRow(new string[] {
                        "WithInDistance",
                        "40 miles"});
            table3.AddRow(new string[] {
                        "ApprenticeshipLevel",
                        "All levels"});
#line 34
 testRunner.When("I enter data", ((string)(null)), table3, "When ");
#line 39
 testRunner.And("I choose Search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 40
 testRunner.Then("I am on the ApprenticeshipSearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 41
 testRunner.When("I choose NationwideLocationTypeLink", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 42
 testRunner.Then("I am on the ApprenticeshipSearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table4.AddRow(new string[] {
                        "DistanceDisplayed",
                        "Equals",
                        "False"});
            table4.AddRow(new string[] {
                        "ClosingDateDisplayed",
                        "Equals",
                        "True"});
            table4.AddRow(new string[] {
                        "NationwideDisplayed",
                        "Equals",
                        "True"});
#line 43
 testRunner.Then("I see SearchResults list contains", ((string)(null)), table4, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Nationwide apprenticeships are in closing date order")]
        [NUnit.Framework.CategoryAttribute("SmokeTests")]
        public virtual void NationwideApprenticeshipsAreInClosingDateOrder()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Nationwide apprenticeships are in closing date order", new string[] {
                        "SmokeTests"});
#line 51
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 52
 testRunner.Given("I navigated to the ApprenticeshipSearchPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table5.AddRow(new string[] {
                        "Location",
                        "London"});
            table5.AddRow(new string[] {
                        "WithInDistance",
                        "40 miles"});
            table5.AddRow(new string[] {
                        "ApprenticeshipLevel",
                        "All levels"});
#line 53
 testRunner.When("I enter data", ((string)(null)), table5, "When ");
#line 58
 testRunner.And("I choose Search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 59
 testRunner.Then("I am on the ApprenticeshipSearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 60
 testRunner.When("I choose NationwideLocationTypeLink", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 61
 testRunner.Then("I am on the ApprenticeshipSearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table6.AddRow(new string[] {
                        "ResultsAreInClosingDateOrder",
                        "Equals",
                        "True"});
#line 62
 testRunner.And("I see", ((string)(null)), table6, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Nationwide apprenticeships found by keyword are in best match order")]
        [NUnit.Framework.CategoryAttribute("SmokeTests")]
        public virtual void NationwideApprenticeshipsFoundByKeywordAreInBestMatchOrder()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Nationwide apprenticeships found by keyword are in best match order", new string[] {
                        "SmokeTests"});
#line 68
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 69
 testRunner.Given("I navigated to the ApprenticeshipSearchPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table7.AddRow(new string[] {
                        "Keywords",
                        "it"});
            table7.AddRow(new string[] {
                        "Location",
                        "London"});
            table7.AddRow(new string[] {
                        "WithInDistance",
                        "40 miles"});
            table7.AddRow(new string[] {
                        "ApprenticeshipLevel",
                        "All levels"});
#line 70
 testRunner.When("I enter data", ((string)(null)), table7, "When ");
#line 76
 testRunner.And("I choose Search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 77
 testRunner.Then("I am on the ApprenticeshipSearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 78
 testRunner.When("I choose NationwideLocationTypeLink", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 79
 testRunner.Then("I am on the ApprenticeshipSearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table8.AddRow(new string[] {
                        "ResultsAreInClosingDateOrder",
                        "Equals",
                        "False"});
#line 80
 testRunner.And("I see", ((string)(null)), table8, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Nationwide apprenticeships found by keyword can be ordered")]
        [NUnit.Framework.CategoryAttribute("SmokeTests")]
        public virtual void NationwideApprenticeshipsFoundByKeywordCanBeOrdered()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Nationwide apprenticeships found by keyword can be ordered", new string[] {
                        "SmokeTests"});
#line 85
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 86
 testRunner.Given("I navigated to the ApprenticeshipSearchPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table9.AddRow(new string[] {
                        "Keywords",
                        "it"});
            table9.AddRow(new string[] {
                        "Location",
                        "London"});
            table9.AddRow(new string[] {
                        "WithInDistance",
                        "40 miles"});
            table9.AddRow(new string[] {
                        "ApprenticeshipLevel",
                        "All levels"});
#line 87
 testRunner.When("I enter data", ((string)(null)), table9, "When ");
#line 93
 testRunner.And("I choose Search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 94
 testRunner.And("I am on the ApprenticeshipSearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table10.AddRow(new string[] {
                        "SearchResultItemsCount",
                        "Greater Than",
                        "0"});
            table10.AddRow(new string[] {
                        "SortOrderingDropDown",
                        "Equals",
                        "Best Match"});
            table10.AddRow(new string[] {
                        "NationwideLocationTypeLink",
                        "Exists",
                        ""});
#line 95
 testRunner.Then("I see", ((string)(null)), table10, "Then ");
#line 100
 testRunner.When("I choose NationwideLocationTypeLink", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 101
 testRunner.Then("I am on the ApprenticeshipSearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table11.AddRow(new string[] {
                        "LocalLocationTypeLink",
                        "Exists",
                        ""});
            table11.AddRow(new string[] {
                        "NationwideLocationTypeLink",
                        "Does Not Exist",
                        ""});
            table11.AddRow(new string[] {
                        "SortOrderingDropDownItemsCount",
                        "Equals",
                        "3"});
            table11.AddRow(new string[] {
                        "SortOrderingDropDownItemsText",
                        "Equals",
                        "Best Match,Closing Date,Recently Added"});
            table11.AddRow(new string[] {
                        "SortOrderingDropDown",
                        "Equals",
                        "Best Match"});
            table11.AddRow(new string[] {
                        "ResultsAreInClosingDateOrder",
                        "Equals",
                        "False"});
#line 102
 testRunner.And("I see", ((string)(null)), table11, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("When I\'m seeing nationwide apprenticeships and I change the results per page I re" +
            "main there")]
        [NUnit.Framework.CategoryAttribute("SmokeTests")]
        public virtual void WhenIMSeeingNationwideApprenticeshipsAndIChangeTheResultsPerPageIRemainThere()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("When I\'m seeing nationwide apprenticeships and I change the results per page I re" +
                    "main there", new string[] {
                        "SmokeTests"});
#line 112
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 113
 testRunner.Given("I navigated to the ApprenticeshipSearchPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table12 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table12.AddRow(new string[] {
                        "Location",
                        "London"});
            table12.AddRow(new string[] {
                        "WithInDistance",
                        "40 miles"});
            table12.AddRow(new string[] {
                        "ApprenticeshipLevel",
                        "All levels"});
#line 114
 testRunner.When("I enter data", ((string)(null)), table12, "When ");
#line 119
 testRunner.And("I choose Search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 120
 testRunner.Then("I am on the ApprenticeshipSearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 121
 testRunner.When("I choose NationwideLocationTypeLink", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 122
 testRunner.Then("I am on the ApprenticeshipSearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table13 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table13.AddRow(new string[] {
                        "ResultsPerPageDropDown",
                        "25 per page"});
#line 123
 testRunner.When("I enter data", ((string)(null)), table13, "When ");
#line 126
 testRunner.Then("I am on the ApprenticeshipSearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table14 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table14.AddRow(new string[] {
                        "LocalLocationTypeLink",
                        "Exists",
                        ""});
            table14.AddRow(new string[] {
                        "NationwideLocationTypeLink",
                        "Does Not Exist",
                        ""});
#line 127
 testRunner.And("I see", ((string)(null)), table14, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("When I\'m seeing nationwide apprenticeships and I change the sort order I remain t" +
            "here")]
        [NUnit.Framework.CategoryAttribute("SmokeTests")]
        public virtual void WhenIMSeeingNationwideApprenticeshipsAndIChangeTheSortOrderIRemainThere()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("When I\'m seeing nationwide apprenticeships and I change the sort order I remain t" +
                    "here", new string[] {
                        "SmokeTests"});
#line 133
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 134
 testRunner.Given("I navigated to the ApprenticeshipSearchPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table15 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table15.AddRow(new string[] {
                        "Keywords",
                        "Construction"});
            table15.AddRow(new string[] {
                        "Location",
                        "London"});
            table15.AddRow(new string[] {
                        "WithInDistance",
                        "40 miles"});
            table15.AddRow(new string[] {
                        "ApprenticeshipLevel",
                        "All levels"});
#line 135
 testRunner.When("I enter data", ((string)(null)), table15, "When ");
#line 141
 testRunner.And("I choose Search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 142
 testRunner.Then("I am on the ApprenticeshipSearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 143
 testRunner.When("I choose NationwideLocationTypeLink", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 144
 testRunner.Then("I am on the ApprenticeshipSearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table16 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table16.AddRow(new string[] {
                        "SortOrderingDropDown",
                        "Closing Date"});
#line 145
 testRunner.When("I enter data", ((string)(null)), table16, "When ");
#line 148
 testRunner.Then("I am on the ApprenticeshipSearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table17 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table17.AddRow(new string[] {
                        "LocalLocationTypeLink",
                        "Exists",
                        ""});
            table17.AddRow(new string[] {
                        "NationwideLocationTypeLink",
                        "Does Not Exist",
                        ""});
#line 149
 testRunner.And("I see", ((string)(null)), table17, "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
