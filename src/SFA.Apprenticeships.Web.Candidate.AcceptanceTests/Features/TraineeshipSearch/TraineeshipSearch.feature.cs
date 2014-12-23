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
namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Features.TraineeshipSearch
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Traineeship Search")]
    [NUnit.Framework.CategoryAttribute("US583")]
    public partial class TraineeshipSearchFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "TraineeshipSearch.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Traineeship Search", "In order to find a traineeship vacancy quickly\r\nAs a candidate\r\nI want to find a " +
                    "traineeship vacancy by location", ProgrammingLanguage.CSharp, new string[] {
                        "US583"});
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
 testRunner.Given("I navigated to the TraineeshipSearchPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 9
 testRunner.And("I am logged out", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 10
 testRunner.And("I navigated to the TraineeshipSearchPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 11
 testRunner.Then("I am on the TraineeshipSearchPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Find traineeships and test ordering")]
        [NUnit.Framework.CategoryAttribute("SmokeTests")]
        public virtual void FindTraineeshipsAndTestOrdering()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Find traineeships and test ordering", new string[] {
                        "SmokeTests"});
#line 14
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 15
 testRunner.Given("I navigated to the TraineeshipSearchPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table1.AddRow(new string[] {
                        "Location",
                        "London"});
#line 16
 testRunner.When("I enter data", ((string)(null)), table1, "When ");
#line 19
 testRunner.And("I choose Search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 20
 testRunner.And("I am on the TraineeshipSearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table2.AddRow(new string[] {
                        "SearchResultItemsCount",
                        "Equals",
                        "5"});
            table2.AddRow(new string[] {
                        "ResultsAreInDistanceOrder",
                        "Equals",
                        "True"});
#line 21
 testRunner.Then("I see", ((string)(null)), table2, "Then ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table3.AddRow(new string[] {
                        "SortOrderingDropDown",
                        "Closing Date"});
#line 25
 testRunner.And("I enter data", ((string)(null)), table3, "And ");
#line 28
 testRunner.And("I am on the TraineeshipSearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table4.AddRow(new string[] {
                        "SearchResultItemsCount",
                        "Equals",
                        "5"});
            table4.AddRow(new string[] {
                        "ResultsAreInClosingDateOrder",
                        "Equals",
                        "True"});
#line 29
 testRunner.And("I see", ((string)(null)), table4, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Find traineeships and change distance")]
        [NUnit.Framework.CategoryAttribute("SmokeTests")]
        public virtual void FindTraineeshipsAndChangeDistance()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Find traineeships and change distance", new string[] {
                        "SmokeTests"});
#line 35
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 36
 testRunner.Given("I navigated to the TraineeshipSearchPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table5.AddRow(new string[] {
                        "Location",
                        "London"});
#line 37
 testRunner.When("I enter data", ((string)(null)), table5, "When ");
#line 40
 testRunner.And("I choose Search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 41
 testRunner.And("I am on the TraineeshipSearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table6.AddRow(new string[] {
                        "WithInDistance",
                        "Equals",
                        "40 miles"});
#line 42
 testRunner.Then("I see", ((string)(null)), table6, "Then ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table7.AddRow(new string[] {
                        "WithInDistance",
                        "2 miles"});
#line 45
 testRunner.When("I enter data", ((string)(null)), table7, "When ");
#line 48
 testRunner.And("I choose Search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 49
 testRunner.Then("I am on the TraineeshipSearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table8.AddRow(new string[] {
                        "WithInDistance",
                        "Equals",
                        "2 miles"});
#line 50
 testRunner.And("I see", ((string)(null)), table8, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Find traineeships and test paging")]
        [NUnit.Framework.CategoryAttribute("SmokeTests")]
        public virtual void FindTraineeshipsAndTestPaging()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Find traineeships and test paging", new string[] {
                        "SmokeTests"});
#line 55
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 56
 testRunner.Given("I navigated to the TraineeshipSearchPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table9.AddRow(new string[] {
                        "Location",
                        "London"});
#line 57
 testRunner.When("I enter data", ((string)(null)), table9, "When ");
#line 60
 testRunner.And("I choose Search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 61
 testRunner.And("I am on the TraineeshipSearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 62
 testRunner.And("I wait to not see PreviousPage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 63
 testRunner.And("I choose NextPage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 64
 testRunner.And("I am on the TraineeshipSearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 65
 testRunner.And("I wait to see PreviousPage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table10.AddRow(new string[] {
                        "NextPage",
                        "Contains",
                        "3 of"});
            table10.AddRow(new string[] {
                        "PreviousPage",
                        "Contains",
                        "1 of"});
#line 66
 testRunner.Then("I see", ((string)(null)), table10, "Then ");
#line 70
 testRunner.When("I choose NextPage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 71
 testRunner.And("I am on the TraineeshipSearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table11.AddRow(new string[] {
                        "NextPage",
                        "Contains",
                        "4 of"});
            table11.AddRow(new string[] {
                        "PreviousPage",
                        "Contains",
                        "2 of"});
#line 72
 testRunner.Then("I see", ((string)(null)), table11, "Then ");
#line 76
 testRunner.When("I choose PreviousPage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 77
 testRunner.And("I am on the TraineeshipSearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table12 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table12.AddRow(new string[] {
                        "NextPage",
                        "Contains",
                        "3 of"});
            table12.AddRow(new string[] {
                        "PreviousPage",
                        "Contains",
                        "1 of"});
#line 78
 testRunner.Then("I see", ((string)(null)), table12, "Then ");
#line 82
 testRunner.When("I choose PreviousPage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 83
 testRunner.And("I am on the TraineeshipSearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 84
 testRunner.And("I wait to not see PreviousPage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table13 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table13.AddRow(new string[] {
                        "NextPage",
                        "Contains",
                        "2 of"});
#line 85
 testRunner.Then("I see", ((string)(null)), table13, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Search when no results are returned for location")]
        [NUnit.Framework.CategoryAttribute("SmokeTests")]
        public virtual void SearchWhenNoResultsAreReturnedForLocation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Search when no results are returned for location", new string[] {
                        "SmokeTests"});
#line 90
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 91
 testRunner.Given("I navigated to the TraineeshipSearchPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table14 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table14.AddRow(new string[] {
                        "Location",
                        "Dundee"});
#line 92
 testRunner.When("I enter data", ((string)(null)), table14, "When ");
#line 95
 testRunner.And("I choose Search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 96
 testRunner.And("I am on the TraineeshipSearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table15 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table15.AddRow(new string[] {
                        "SortOrderingDropDown",
                        "Does Not Exist",
                        ""});
            table15.AddRow(new string[] {
                        "NoResultsTitle",
                        "Exists",
                        ""});
#line 97
 testRunner.Then("I see", ((string)(null)), table15, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Search doesn\'t error when location doesn\'t exist")]
        [NUnit.Framework.CategoryAttribute("SmokeTests")]
        public virtual void SearchDoesnTErrorWhenLocationDoesnTExist()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Search doesn\'t error when location doesn\'t exist", new string[] {
                        "SmokeTests"});
#line 103
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 104
 testRunner.Given("I navigated to the TraineeshipSearchPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table16 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table16.AddRow(new string[] {
                        "Location",
                        "KJHNSAKDFJHA"});
#line 105
 testRunner.When("I enter data", ((string)(null)), table16, "When ");
#line 108
 testRunner.And("I choose Search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 109
 testRunner.And("I am on the TraineeshipSearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table17 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table17.AddRow(new string[] {
                        "SortOrderingDropDown",
                        "Does Not Exist",
                        ""});
            table17.AddRow(new string[] {
                        "NoResultsTitle",
                        "Exists",
                        ""});
#line 110
 testRunner.Then("I see", ((string)(null)), table17, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Search location autocomplete appears on both initial search page and search resul" +
            "ts page")]
        [NUnit.Framework.CategoryAttribute("SmokeTests")]
        public virtual void SearchLocationAutocompleteAppearsOnBothInitialSearchPageAndSearchResultsPage()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Search location autocomplete appears on both initial search page and search resul" +
                    "ts page", new string[] {
                        "SmokeTests"});
#line 116
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 117
 testRunner.Given("I navigated to the TraineeshipSearchPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table18 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table18.AddRow(new string[] {
                        "Location",
                        "Coventry"});
#line 118
 testRunner.When("I enter data", ((string)(null)), table18, "When ");
#line 121
 testRunner.Then("I wait for 5 seconds to see LocationAutoComplete", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table19 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table19.AddRow(new string[] {
                        "Text",
                        "Equals",
                        "Coventry (West Midlands)"});
#line 122
 testRunner.When("I am on LocationAutoCompletItems list item matching criteria", ((string)(null)), table19, "When ");
#line 125
 testRunner.And("I choose WrappedElement", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 126
 testRunner.And("I am on the TraineeshipSearchPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 127
 testRunner.And("I choose Search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 128
 testRunner.And("I am on the TraineeshipSearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table20 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table20.AddRow(new string[] {
                        "ClearLocation",
                        "Equals",
                        "True"});
#line 129
 testRunner.Then("I see", ((string)(null)), table20, "Then ");
#line hidden
            TechTalk.SpecFlow.Table table21 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table21.AddRow(new string[] {
                        "Location",
                        "London"});
#line 132
 testRunner.When("I enter data", ((string)(null)), table21, "When ");
#line 135
 testRunner.Then("I wait for 5 seconds to see LocationAutoComplete", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Different results per page")]
        [NUnit.Framework.CategoryAttribute("SmokeTests")]
        public virtual void DifferentResultsPerPage()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Different results per page", new string[] {
                        "SmokeTests"});
#line 138
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 139
 testRunner.Given("I navigated to the TraineeshipSearchPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table22 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table22.AddRow(new string[] {
                        "Location",
                        "London"});
#line 140
 testRunner.When("I enter data", ((string)(null)), table22, "When ");
#line 143
 testRunner.And("I choose Search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 144
 testRunner.Then("I am on the TraineeshipSearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table23 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table23.AddRow(new string[] {
                        "SearchResultItemsCount",
                        "Equals",
                        "5"});
#line 145
 testRunner.And("I see", ((string)(null)), table23, "And ");
#line hidden
            TechTalk.SpecFlow.Table table24 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table24.AddRow(new string[] {
                        "ResultsPerPageDropDown",
                        "10 per page"});
#line 148
 testRunner.When("I enter data", ((string)(null)), table24, "When ");
#line 151
 testRunner.Then("I am on the TraineeshipSearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table25 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table25.AddRow(new string[] {
                        "SearchResultItemsCount",
                        "Equals",
                        "10"});
#line 152
 testRunner.And("I see", ((string)(null)), table25, "And ");
#line hidden
            TechTalk.SpecFlow.Table table26 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table26.AddRow(new string[] {
                        "ResultsPerPageDropDown",
                        "25 per page"});
#line 155
 testRunner.When("I enter data", ((string)(null)), table26, "When ");
#line 158
 testRunner.Then("I am on the TraineeshipSearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table27 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table27.AddRow(new string[] {
                        "SearchResultItemsCount",
                        "Equals",
                        "25"});
#line 159
 testRunner.And("I see", ((string)(null)), table27, "And ");
#line hidden
            TechTalk.SpecFlow.Table table28 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table28.AddRow(new string[] {
                        "ResultsPerPageDropDown",
                        "50 per page"});
#line 162
 testRunner.When("I enter data", ((string)(null)), table28, "When ");
#line 165
 testRunner.Then("I am on the TraineeshipSearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table29 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table29.AddRow(new string[] {
                        "SearchResultItemsCount",
                        "Equals",
                        "50"});
#line 166
 testRunner.And("I see", ((string)(null)), table29, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Find traineeship by partial postcode returns results")]
        [NUnit.Framework.CategoryAttribute("SmokeTests")]
        public virtual void FindTraineeshipByPartialPostcodeReturnsResults()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Find traineeship by partial postcode returns results", new string[] {
                        "SmokeTests"});
#line 171
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 172
 testRunner.Given("I navigated to the TraineeshipSearchPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table30 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table30.AddRow(new string[] {
                        "Location",
                        "B1"});
#line 173
 testRunner.When("I enter data", ((string)(null)), table30, "When ");
#line 176
 testRunner.And("I choose Search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 177
 testRunner.And("I am on the TraineeshipSearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table31 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table31.AddRow(new string[] {
                        "SearchResultItemsCount",
                        "Equals",
                        "5"});
#line 178
 testRunner.Then("I see", ((string)(null)), table31, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
