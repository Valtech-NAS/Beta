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
namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Features.TraineeshipApplication
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Apply for a traineeship vacancy")]
    [NUnit.Framework.CategoryAttribute("US583")]
    public partial class ApplyForATraineeshipVacancyFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "ApplyForTraineeship.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Apply for a traineeship vacancy", "As a candidate\r\nI want to submit traineeship applications \r\nso that it can be rev" +
                    "iewed by a Vacancy Manager", ProgrammingLanguage.CSharp, new string[] {
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
        [NUnit.Framework.DescriptionAttribute("As a candidate I am taken to login or create an account when viewing traineeship " +
            "details")]
        [NUnit.Framework.CategoryAttribute("US592")]
        public virtual void AsACandidateIAmTakenToLoginOrCreateAnAccountWhenViewingTraineeshipDetails()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("As a candidate I am taken to login or create an account when viewing traineeship " +
                    "details", new string[] {
                        "US592"});
#line 14
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 15
 testRunner.Given("I select the \"first\" traineeship vacancy in location \"N7 8LS\" that can apply by t" +
                    "his website", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 16
 testRunner.When("I am on the TraineeshipDetailsPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 17
 testRunner.And("I choose ApplyButton", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 18
 testRunner.Then("I am on the LoginPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("As a candidate I want to enter my qualifications and work experience in a trainee" +
            "ship application")]
        public virtual void AsACandidateIWantToEnterMyQualificationsAndWorkExperienceInATraineeshipApplication()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("As a candidate I want to enter my qualifications and work experience in a trainee" +
                    "ship application", ((string[])(null)));
#line 20
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 21
 testRunner.Given("I have registered a new candidate", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 22
 testRunner.When("I select the \"first\" traineeship vacancy in location \"N7 8LS\" that can apply by t" +
                    "his website", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 23
 testRunner.Then("I am on the TraineeshipDetailsPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 24
 testRunner.When("I choose ApplyButton", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 25
 testRunner.Then("I am on the TraineeshipApplicationPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 26
 testRunner.When("I choose QualificationsYes", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 27
 testRunner.And("I choose SaveQualification", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table1.AddRow(new string[] {
                        "QualificationsValidationErrorsCount",
                        "Equals",
                        "4"});
#line 28
 testRunner.Then("I see", ((string)(null)), table1, "Then ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table2.AddRow(new string[] {
                        "Text",
                        "Equals",
                        "GCSE"});
#line 31
 testRunner.When("I am on QualificationTypeDropdown list item matching criteria", ((string)(null)), table2, "When ");
#line 34
 testRunner.And("I choose WrappedElement", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 35
 testRunner.And("I am on the TraineeshipApplicationPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table3.AddRow(new string[] {
                        "SubjectYear",
                        "2012"});
            table3.AddRow(new string[] {
                        "SubjectName",
                        "SubjectName"});
            table3.AddRow(new string[] {
                        "SubjectGrade",
                        "SubjectGrade"});
#line 36
 testRunner.When("I enter data", ((string)(null)), table3, "When ");
#line 41
 testRunner.And("I am on the TraineeshipApplicationPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 42
 testRunner.And("I choose SaveQualification", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 44
 testRunner.And("I choose SaveQualification", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 45
 testRunner.And("I wait for 30 seconds to see QualificationsSummary", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table4.AddRow(new string[] {
                        "QualificationsSummaryCount",
                        "Equals",
                        "1"});
#line 46
 testRunner.Then("I see", ((string)(null)), table4, "Then ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table5.AddRow(new string[] {
                        "Subject",
                        "Equals",
                        "SubjectName"});
            table5.AddRow(new string[] {
                        "Year",
                        "Equals",
                        "2012"});
            table5.AddRow(new string[] {
                        "Grade",
                        "Equals",
                        "SubjectGrade"});
#line 49
 testRunner.And("I am on QualificationsSummaryItems list item matching criteria", ((string)(null)), table5, "And ");
#line 54
 testRunner.When("I choose RemoveQualificationLink", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 55
 testRunner.And("I am on the TraineeshipApplicationPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table6.AddRow(new string[] {
                        "QualificationsSummary",
                        "Does Not Exist",
                        ""});
#line 56
 testRunner.Then("I see", ((string)(null)), table6, "Then ");
#line 59
 testRunner.When("I choose WorkExperienceYes", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 60
 testRunner.And("I choose SaveWorkExperience", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table7.AddRow(new string[] {
                        "WorkExperienceValidationErrorsCount",
                        "Equals",
                        "5"});
#line 61
 testRunner.Then("I see", ((string)(null)), table7, "Then ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table8.AddRow(new string[] {
                        "WorkEmployer",
                        "WorkEmployer"});
            table8.AddRow(new string[] {
                        "WorkTitle",
                        "WorkTitle"});
            table8.AddRow(new string[] {
                        "WorkRole",
                        "WorkRole"});
            table8.AddRow(new string[] {
                        "WorkFromYear",
                        "2011"});
            table8.AddRow(new string[] {
                        "WorkToYear",
                        "2012"});
#line 64
 testRunner.When("I enter data", ((string)(null)), table8, "When ");
#line 71
 testRunner.And("I choose SaveWorkExperience", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 73
 testRunner.And("I choose SaveWorkExperience", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 74
 testRunner.Then("I wait for 30 seconds to see WorkExperienceSummary", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table9.AddRow(new string[] {
                        "WorkExperiencesCount",
                        "Equals",
                        "1"});
#line 75
 testRunner.Then("I see", ((string)(null)), table9, "Then ");
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table10.AddRow(new string[] {
                        "Employer",
                        "Equals",
                        "WorkEmployer"});
            table10.AddRow(new string[] {
                        "JobTitle",
                        "Equals",
                        "WorkTitle"});
            table10.AddRow(new string[] {
                        "MainDuties",
                        "Equals",
                        "WorkRole"});
#line 78
 testRunner.And("I am on WorkExperienceSummaryItems list item matching criteria", ((string)(null)), table10, "And ");
#line 83
 testRunner.When("I choose RemoveLink", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 84
 testRunner.And("I am on the TraineeshipApplicationPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table11.AddRow(new string[] {
                        "WorkExperienceSummary",
                        "Does Not Exist",
                        ""});
#line 85
 testRunner.Then("I see", ((string)(null)), table11, "Then ");
#line hidden
            TechTalk.SpecFlow.Table table12 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table12.AddRow(new string[] {
                        "Candidate_EmployerQuestionAnswers_CandidateAnswer1",
                        "Emp 1"});
            table12.AddRow(new string[] {
                        "Candidate_EmployerQuestionAnswers_CandidateAnswer2",
                        "Emp 2"});
#line 89
 testRunner.When("I enter employer question data if present", ((string)(null)), table12, "When ");
#line 93
 testRunner.And("I choose QualificationsYes", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table13 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table13.AddRow(new string[] {
                        "Text",
                        "Equals",
                        "GCSE"});
#line 94
 testRunner.And("I am on QualificationTypeDropdown list item matching criteria", ((string)(null)), table13, "And ");
#line 97
 testRunner.And("I choose WrappedElement", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 98
 testRunner.And("I am on the TraineeshipApplicationPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table14 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table14.AddRow(new string[] {
                        "SubjectYear",
                        "2012"});
            table14.AddRow(new string[] {
                        "SubjectName",
                        "SubjectName"});
            table14.AddRow(new string[] {
                        "SubjectGrade",
                        "SubjectGrade"});
#line 99
 testRunner.And("I enter data", ((string)(null)), table14, "And ");
#line 104
 testRunner.And("I choose SaveQualification", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 105
 testRunner.When("I choose WorkExperienceYes", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table15 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table15.AddRow(new string[] {
                        "WorkEmployer",
                        "WorkEmployer"});
            table15.AddRow(new string[] {
                        "WorkTitle",
                        "WorkTitle"});
            table15.AddRow(new string[] {
                        "WorkRole",
                        "WorkRole"});
            table15.AddRow(new string[] {
                        "WorkFromYear",
                        "2011"});
            table15.AddRow(new string[] {
                        "WorkToYear",
                        "2012"});
#line 106
 testRunner.And("I enter data", ((string)(null)), table15, "And ");
#line 113
 testRunner.And("I choose SaveWorkExperience", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 114
 testRunner.When("I am on the TraineeshipApplicationPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 115
 testRunner.And("I choose ApplyButton", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 116
 testRunner.Then("I am on the TraineeshipWhatsNextPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
