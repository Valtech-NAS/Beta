﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.34003
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace SpecBind.CodedUI.IntegrationTests.Features
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UITesting.CodedUITestAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.DeploymentItemAttribute("SpecBind.CodedUI.dll")]
    public partial class TableListValidationFeaturesFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "TableListValidationTests.feature"
#line hidden
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Table List Validation Features", "Contains tests for table list validation features.", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute()]
        public virtual void TestInitialize()
        {
            if (((TechTalk.SpecFlow.FeatureContext.Current != null) 
                        && (TechTalk.SpecFlow.FeatureContext.Current.FeatureInfo.Title != "Table List Validation Features")))
            {
                SpecBind.CodedUI.IntegrationTests.Features.TableListValidationFeaturesFeature.FeatureSetup(null);
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute()]
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
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Validate Table List \"Start With\" Validator")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Table List Validation Features")]
        public virtual void ValidateTableListStartWithValidator()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Validate Table List \"Start With\" Validator", ((string[])(null)));
#line 4
this.ScenarioSetup(scenarioInfo);
#line 5
  testRunner.Given("I navigated to the Home page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 6
       testRunner.And("I chose Students", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 7
    testRunner.And("I was on the Students Search page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 8
   testRunner.When("I choose Search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table1.AddRow(new string[] {
                        "First Name",
                        "Equals",
                        "Carson"});
            table1.AddRow(new string[] {
                        "Last Name",
                        "Equals",
                        "Alexander"});
#line 9
   testRunner.Then("I see results grid list starts with", ((string)(null)), table1, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Validate Table List \"Ends With\" Validator")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Table List Validation Features")]
        public virtual void ValidateTableListEndsWithValidator()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Validate Table List \"Ends With\" Validator", ((string[])(null)));
#line 14
this.ScenarioSetup(scenarioInfo);
#line 15
  testRunner.Given("I navigated to the Home page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 16
       testRunner.And("I chose Students", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 17
    testRunner.And("I was on the Students Search page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 18
   testRunner.When("I choose Search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table2.AddRow(new string[] {
                        "First Name",
                        "Equals",
                        "Arturo"});
            table2.AddRow(new string[] {
                        "Last Name",
                        "Equals",
                        "Anand"});
#line 19
   testRunner.Then("I see results grid list ends with", ((string)(null)), table2, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Validate Table List \"Contains\" Validator")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Table List Validation Features")]
        public virtual void ValidateTableListContainsValidator()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Validate Table List \"Contains\" Validator", ((string[])(null)));
#line 24
this.ScenarioSetup(scenarioInfo);
#line 25
  testRunner.Given("I navigated to the Home page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 26
       testRunner.And("I chose Students", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 27
    testRunner.And("I was on the Students Search page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 28
   testRunner.When("I choose Search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table3.AddRow(new string[] {
                        "First Name",
                        "Equals",
                        "Meredith"});
            table3.AddRow(new string[] {
                        "Last Name",
                        "Equals",
                        "Alonso"});
#line 29
   testRunner.Then("I see results grid list contains", ((string)(null)), table3, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Validate Table List \"Equals\" Validator")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Table List Validation Features")]
        public virtual void ValidateTableListEqualsValidator()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Validate Table List \"Equals\" Validator", ((string[])(null)));
#line 34
this.ScenarioSetup(scenarioInfo);
#line 35
  testRunner.Given("I navigated to the Home page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 36
       testRunner.And("I chose Students", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 37
    testRunner.And("I was on the Students Search page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table4.AddRow(new string[] {
                        "Find by name",
                        "Alonso"});
#line 38
   testRunner.When("I enter data", ((string)(null)), table4, "When ");
#line 41
    testRunner.And("I choose Search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table5.AddRow(new string[] {
                        "First Name",
                        "Equals",
                        "Meredith"});
            table5.AddRow(new string[] {
                        "Last Name",
                        "Equals",
                        "Alonso"});
#line 42
   testRunner.Then("I see results grid list equals", ((string)(null)), table5, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Validate Table List \"Does Not Contain\" Validator")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Table List Validation Features")]
        public virtual void ValidateTableListDoesNotContainValidator()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Validate Table List \"Does Not Contain\" Validator", ((string[])(null)));
#line 47
this.ScenarioSetup(scenarioInfo);
#line 48
  testRunner.Given("I navigated to the Home page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 49
       testRunner.And("I chose Students", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 50
    testRunner.And("I was on the Students Search page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 51
   testRunner.When("I choose Search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table6.AddRow(new string[] {
                        "First Name",
                        "Equals",
                        "Bob"});
            table6.AddRow(new string[] {
                        "Last Name",
                        "Equals",
                        "Smitty"});
#line 52
   testRunner.Then("I see results grid list does not contain", ((string)(null)), table6, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
