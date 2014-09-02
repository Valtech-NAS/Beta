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
namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Features.Application
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Apply for a vacancy")]
    [NUnit.Framework.CategoryAttribute("US352")]
    public partial class ApplyForAVacancyFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Apply.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Apply for a vacancy", "As a candidate\r\nI want to submit applications \r\nso that it can be reviewed by a V" +
                    "acancy Manager", ProgrammingLanguage.CSharp, new string[] {
                        "US352"});
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
#line 8
#line 9
 testRunner.Given("I navigated to the HomePage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 10
 testRunner.When("I am on the HomePage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("As a candidate I would like to apply for a vacancy")]
        [NUnit.Framework.CategoryAttribute("US486")]
        [NUnit.Framework.CategoryAttribute("US458")]
        [NUnit.Framework.CategoryAttribute("US354")]
        public virtual void AsACandidateIWouldLikeToApplyForAVacancy()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("As a candidate I would like to apply for a vacancy", new string[] {
                        "US486",
                        "US458",
                        "US354"});
#line 13
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line 14
 testRunner.Given("I have registered a new candidate", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table1.AddRow(new string[] {
                        "Location",
                        "N7 8LS"});
#line 15
 testRunner.When("I enter data", ((string)(null)), table1, "When ");
#line 18
 testRunner.And("I choose Search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 19
 testRunner.Then("I am on the VacancySearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 20
 testRunner.When("I choose FirstVacancyLink", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 21
 testRunner.Then("I am on the VacancyDetailsPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 22
 testRunner.When("I choose ApplyButton", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 23
 testRunner.Then("I am on the ApplicationPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 24
 testRunner.When("I choose SupportMeYes", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table2.AddRow(new string[] {
                        "EducationNameOfSchool",
                        "SchoolName"});
            table2.AddRow(new string[] {
                        "EducationFromYear",
                        "2010"});
            table2.AddRow(new string[] {
                        "EducationToYear",
                        "2012"});
            table2.AddRow(new string[] {
                        "WhatAreYourStrengths",
                        "My strengths"});
            table2.AddRow(new string[] {
                        "WhatCanYouImprove",
                        "What can I improve"});
            table2.AddRow(new string[] {
                        "HobbiesAndInterests",
                        "Hobbies and interests"});
            table2.AddRow(new string[] {
                        "WhatCanWeDoToSupportYou",
                        "What can we do to support you"});
#line 25
 testRunner.And("I enter data", ((string)(null)), table2, "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table3.AddRow(new string[] {
                        "Candidate_EmployerQuestionAnswers_CandidateAnswer1",
                        "Emp 1"});
            table3.AddRow(new string[] {
                        "Candidate_EmployerQuestionAnswers_CandidateAnswer2",
                        "Emp 2"});
#line 34
 testRunner.And("I enter employer question data if present", ((string)(null)), table3, "And ");
#line 38
 testRunner.And("I choose ApplyButton", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 39
 testRunner.Then("I am on the ApplicationPreviewPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 40
 testRunner.When("I choose SubmitApplication", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 41
 testRunner.Then("I am on the ApplicationCompletePage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 42
 testRunner.When("I choose MyApplicationsLink", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 43
 testRunner.Then("I am on the MyApplicationsPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table4.AddRow(new string[] {
                        "SubmittedApplicationsCount",
                        "Equals",
                        "1"});
#line 44
 testRunner.And("I see", ((string)(null)), table4, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("As a candidate I want to save my application as a draft and be able to resume")]
        [NUnit.Framework.CategoryAttribute("US461")]
        [NUnit.Framework.CategoryAttribute("US154")]
        [NUnit.Framework.CategoryAttribute("US458")]
        [NUnit.Framework.CategoryAttribute("US464")]
        public virtual void AsACandidateIWantToSaveMyApplicationAsADraftAndBeAbleToResume()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("As a candidate I want to save my application as a draft and be able to resume", new string[] {
                        "US461",
                        "US154",
                        "US458",
                        "US464"});
#line 49
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line 50
 testRunner.Given("I have registered a new candidate", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table5.AddRow(new string[] {
                        "Location",
                        "N7 8LS"});
#line 51
 testRunner.When("I enter data", ((string)(null)), table5, "When ");
#line 54
 testRunner.And("I choose Search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 55
 testRunner.Then("I am on the VacancySearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 56
 testRunner.When("I choose FirstVacancyLink", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 57
 testRunner.Then("I am on the VacancyDetailsPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 58
 testRunner.When("I choose ApplyButton", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 59
 testRunner.Then("I am on the ApplicationPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table6.AddRow(new string[] {
                        "EducationNameOfSchool",
                        "SchoolName"});
            table6.AddRow(new string[] {
                        "EducationFromYear",
                        "2010"});
            table6.AddRow(new string[] {
                        "EducationToYear",
                        "2012"});
            table6.AddRow(new string[] {
                        "WhatAreYourStrengths",
                        "My strengths"});
            table6.AddRow(new string[] {
                        "WhatCanYouImprove",
                        "What can I improve"});
            table6.AddRow(new string[] {
                        "HobbiesAndInterests",
                        "Hobbies and interests"});
#line 60
 testRunner.When("I enter data", ((string)(null)), table6, "When ");
#line 68
 testRunner.And("I choose SaveButton", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 69
 testRunner.Then("I wait to see ApplicationSavedMessage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table7.AddRow(new string[] {
                        "ApplicationSavedMessage",
                        "Ends With",
                        "my applications"});
#line 70
 testRunner.And("I see", ((string)(null)), table7, "And ");
#line 73
 testRunner.When("I choose MyApplicationsLink", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 74
 testRunner.Then("I am on the MyApplicationsPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table8.AddRow(new string[] {
                        "DraftApplicationsCount",
                        "Equals",
                        "1"});
#line 75
 testRunner.And("I see", ((string)(null)), table8, "And ");
#line 78
 testRunner.When("I choose ResumeLink", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 79
 testRunner.Then("I am on the ApplicationPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table9.AddRow(new string[] {
                        "ApplicationSavedMessage",
                        "Ends With",
                        "my applications"});
            table9.AddRow(new string[] {
                        "EducationNameOfSchool",
                        "Equals",
                        "SchoolName"});
            table9.AddRow(new string[] {
                        "EducationFromYear",
                        "Equals",
                        "2010"});
            table9.AddRow(new string[] {
                        "EducationToYear",
                        "Equals",
                        "2012"});
            table9.AddRow(new string[] {
                        "WhatAreYourStrengths",
                        "Equals",
                        "My strengths"});
            table9.AddRow(new string[] {
                        "WhatCanYouImprove",
                        "Equals",
                        "What can I improve"});
            table9.AddRow(new string[] {
                        "HobbiesAndInterests",
                        "Equals",
                        "Hobbies and interests"});
#line 80
 testRunner.And("I see", ((string)(null)), table9, "And ");
#line 89
 testRunner.When("I choose MyApplicationsLink", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 90
 testRunner.Then("I am on the MyApplicationsPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("As a candidate I want to enter my qualifications and work experience")]
        [NUnit.Framework.IgnoreAttribute()]
        [NUnit.Framework.CategoryAttribute("US461")]
        [NUnit.Framework.CategoryAttribute("US362")]
        [NUnit.Framework.CategoryAttribute("US365")]
        [NUnit.Framework.CategoryAttribute("US154")]
        [NUnit.Framework.CategoryAttribute("US463")]
        [NUnit.Framework.CategoryAttribute("US352")]
        [NUnit.Framework.CategoryAttribute("US354")]
        public virtual void AsACandidateIWantToEnterMyQualificationsAndWorkExperience()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("As a candidate I want to enter my qualifications and work experience", new string[] {
                        "US461",
                        "US362",
                        "US365",
                        "US154",
                        "US463",
                        "US352",
                        "US354",
                        "ignore"});
#line 94
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line 95
 testRunner.Given("I have registered a new candidate", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table10.AddRow(new string[] {
                        "Location",
                        "N7 8LS"});
#line 96
 testRunner.When("I enter data", ((string)(null)), table10, "When ");
#line 99
 testRunner.And("I choose Search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 100
 testRunner.Then("I am on the VacancySearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 101
 testRunner.When("I choose FirstVacancyLink", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 102
 testRunner.Then("I am on the VacancyDetailsPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 103
 testRunner.When("I choose ApplyButton", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 104
 testRunner.Then("I am on the ApplicationPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 105
 testRunner.When("I choose QualificationsYes", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 106
 testRunner.And("I choose SaveQualification", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 107
 testRunner.Then("I wait to see QualificationValidationSummary", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table11.AddRow(new string[] {
                        "QualificationValidationSummary",
                        "Contains",
                        "Please complete all fields displayed"});
#line 108
 testRunner.And("I see", ((string)(null)), table11, "And ");
#line hidden
            TechTalk.SpecFlow.Table table12 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table12.AddRow(new string[] {
                        "Text",
                        "Equals",
                        "GCSE"});
#line 111
 testRunner.When("I am on QualificationTypeDropdown list item matching criteria", ((string)(null)), table12, "When ");
#line 114
 testRunner.And("I choose WrappedElement", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 115
 testRunner.And("I am on the ApplicationPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table13 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table13.AddRow(new string[] {
                        "SubjectYear",
                        "2012"});
            table13.AddRow(new string[] {
                        "SubjectName",
                        "SubjectName"});
            table13.AddRow(new string[] {
                        "SubjectGrade",
                        "SubjectGrade"});
#line 116
 testRunner.When("I enter data", ((string)(null)), table13, "When ");
#line 121
 testRunner.And("I am on the ApplicationPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 122
 testRunner.And("I choose SaveQualification", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 123
 testRunner.Then("I wait to see QualificationsSummary", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table14 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table14.AddRow(new string[] {
                        "QualificationsSummaryCount",
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
                        "Subject",
                        "Equals",
                        "SubjectName"});
            table15.AddRow(new string[] {
                        "Year",
                        "Equals",
                        "2012"});
            table15.AddRow(new string[] {
                        "Grade",
                        "Equals",
                        "SubjectGrade"});
#line 127
 testRunner.And("I am on QualificationsSummaryItems list item matching criteria", ((string)(null)), table15, "And ");
#line 132
 testRunner.When("I choose RemoveQualificationLink", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 133
 testRunner.And("I am on the ApplicationPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table16 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table16.AddRow(new string[] {
                        "QualificationsSummaryCount",
                        "Equals",
                        "0"});
#line 134
 testRunner.Then("I see", ((string)(null)), table16, "Then ");
#line 137
 testRunner.When("I choose WorkExperienceYes", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 138
 testRunner.And("I choose SaveWorkExperience", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table17 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table17.AddRow(new string[] {
                        "ValidationErrorsCount",
                        "Equals",
                        "5"});
#line 139
 testRunner.Then("I see", ((string)(null)), table17, "Then ");
#line hidden
            TechTalk.SpecFlow.Table table18 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table18.AddRow(new string[] {
                        "WorkEmployer",
                        "WorkEmployer"});
            table18.AddRow(new string[] {
                        "WorkTitle",
                        "WorkTitle"});
            table18.AddRow(new string[] {
                        "WorkRole",
                        "WorkRole"});
            table18.AddRow(new string[] {
                        "WorkFromYear",
                        "2011"});
            table18.AddRow(new string[] {
                        "WorkToYear",
                        "2012"});
#line 142
 testRunner.When("I enter data", ((string)(null)), table18, "When ");
#line 149
 testRunner.And("I choose SaveWorkExperience", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 150
 testRunner.Then("I wait to see WorkExperienceSummary", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table19 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table19.AddRow(new string[] {
                        "WorkExperiencesCount",
                        "Equals",
                        "1"});
#line 151
 testRunner.Then("I see", ((string)(null)), table19, "Then ");
#line hidden
            TechTalk.SpecFlow.Table table20 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table20.AddRow(new string[] {
                        "Employer",
                        "Equals",
                        "WorkEmployer"});
            table20.AddRow(new string[] {
                        "JobTitle",
                        "Equals",
                        "WorkTitle"});
            table20.AddRow(new string[] {
                        "MainDuties",
                        "Equals",
                        "WorkRole"});
#line 154
 testRunner.And("I am on WorkExperienceSummaryItems list item matching criteria", ((string)(null)), table20, "And ");
#line 159
 testRunner.When("I choose RemoveLink", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 160
 testRunner.And("I am on the ApplicationPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table21 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table21.AddRow(new string[] {
                        "WorkExperiencesCount",
                        "Equals",
                        "0"});
#line 161
 testRunner.Then("I see", ((string)(null)), table21, "Then ");
#line hidden
            TechTalk.SpecFlow.Table table22 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table22.AddRow(new string[] {
                        "EducationNameOfSchool",
                        "SchoolName"});
            table22.AddRow(new string[] {
                        "EducationFromYear",
                        "2010"});
            table22.AddRow(new string[] {
                        "EducationToYear",
                        "2012"});
            table22.AddRow(new string[] {
                        "WhatAreYourStrengths",
                        "My strengths"});
            table22.AddRow(new string[] {
                        "WhatCanYouImprove",
                        "What can I improve"});
            table22.AddRow(new string[] {
                        "HobbiesAndInterests",
                        "Hobbies and interests"});
#line 165
 testRunner.When("I enter data", ((string)(null)), table22, "When ");
#line hidden
            TechTalk.SpecFlow.Table table23 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table23.AddRow(new string[] {
                        "Candidate_EmployerQuestionAnswers_CandidateAnswer1",
                        "Emp 1"});
            table23.AddRow(new string[] {
                        "Candidate_EmployerQuestionAnswers_CandidateAnswer2",
                        "Emp 2"});
#line 173
 testRunner.And("I enter employer question data if present", ((string)(null)), table23, "And ");
#line 177
 testRunner.And("I choose QualificationsYes", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table24 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table24.AddRow(new string[] {
                        "Text",
                        "Equals",
                        "GCSE"});
#line 178
 testRunner.And("I am on QualificationTypeDropdown list item matching criteria", ((string)(null)), table24, "And ");
#line 181
 testRunner.And("I choose WrappedElement", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 182
 testRunner.And("I am on the ApplicationPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table25 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table25.AddRow(new string[] {
                        "SubjectYear",
                        "2012"});
            table25.AddRow(new string[] {
                        "SubjectName",
                        "SubjectName"});
            table25.AddRow(new string[] {
                        "SubjectGrade",
                        "SubjectGrade"});
#line 183
 testRunner.And("I enter data", ((string)(null)), table25, "And ");
#line 188
 testRunner.And("I choose SaveQualification", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 189
 testRunner.When("I choose WorkExperienceYes", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table26 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table26.AddRow(new string[] {
                        "WorkEmployer",
                        "WorkEmployer"});
            table26.AddRow(new string[] {
                        "WorkTitle",
                        "WorkTitle"});
            table26.AddRow(new string[] {
                        "WorkRole",
                        "WorkRole"});
            table26.AddRow(new string[] {
                        "WorkFromYear",
                        "2011"});
            table26.AddRow(new string[] {
                        "WorkToYear",
                        "2012"});
#line 190
 testRunner.And("I enter data", ((string)(null)), table26, "And ");
#line 197
 testRunner.And("I choose SaveWorkExperience", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 198
 testRunner.And("I choose SaveButton", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 199
 testRunner.Then("I wait to see ApplicationSavedMessage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table27 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table27.AddRow(new string[] {
                        "ApplicationSavedMessage",
                        "Ends With",
                        "my applications"});
#line 200
 testRunner.And("I see", ((string)(null)), table27, "And ");
#line 203
 testRunner.When("I choose MyApplicationsLink", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 204
 testRunner.Then("I am on the MyApplicationsPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table28 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table28.AddRow(new string[] {
                        "DraftApplicationsCount",
                        "Equals",
                        "1"});
#line 205
 testRunner.And("I see", ((string)(null)), table28, "And ");
#line 208
 testRunner.When("I choose ResumeLink", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 209
 testRunner.Then("I am on the ApplicationPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table29 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table29.AddRow(new string[] {
                        "QualificationsSummaryCount",
                        "Equals",
                        "1"});
#line 210
 testRunner.And("I see", ((string)(null)), table29, "And ");
#line hidden
            TechTalk.SpecFlow.Table table30 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table30.AddRow(new string[] {
                        "Subject",
                        "Equals",
                        "SubjectName"});
            table30.AddRow(new string[] {
                        "Year",
                        "Equals",
                        "2012"});
            table30.AddRow(new string[] {
                        "Grade",
                        "Equals",
                        "SubjectGrade"});
#line 213
 testRunner.And("I am on QualificationsSummaryItems list item matching criteria", ((string)(null)), table30, "And ");
#line 218
 testRunner.And("I am on the ApplicationPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table31 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table31.AddRow(new string[] {
                        "WorkExperiencesCount",
                        "Equals",
                        "1"});
#line 219
 testRunner.And("I see", ((string)(null)), table31, "And ");
#line hidden
            TechTalk.SpecFlow.Table table32 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table32.AddRow(new string[] {
                        "Employer",
                        "Equals",
                        "WorkEmployer"});
            table32.AddRow(new string[] {
                        "JobTitle",
                        "Equals",
                        "WorkTitle"});
            table32.AddRow(new string[] {
                        "MainDuties",
                        "Equals",
                        "WorkRole"});
#line 222
 testRunner.And("I am on WorkExperienceSummaryItems list item matching criteria", ((string)(null)), table32, "And ");
#line 227
 testRunner.When("I am on the ApplicationPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 228
 testRunner.And("I choose ApplyButton", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 229
 testRunner.Then("I am on the ApplicationPreviewPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table33 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table33.AddRow(new string[] {
                        "Fullname",
                        "Equals",
                        "Firstname Lastname"});
            table33.AddRow(new string[] {
                        "Phonenumber",
                        "Equals",
                        "07970523193"});
            table33.AddRow(new string[] {
                        "EmailAddress",
                        "Equals",
                        "{EmailToken}"});
            table33.AddRow(new string[] {
                        "Postcode",
                        "Equals",
                        "N7 8LS"});
            table33.AddRow(new string[] {
                        "DateOfBirth",
                        "Equals",
                        "01 January 2000"});
            table33.AddRow(new string[] {
                        "EducationNameOfSchool",
                        "Equals",
                        "SchoolName"});
            table33.AddRow(new string[] {
                        "EducationFromYear",
                        "Equals",
                        "2010"});
            table33.AddRow(new string[] {
                        "EducationToYear",
                        "Equals",
                        "2012"});
            table33.AddRow(new string[] {
                        "WhatAreYourStrengths",
                        "Equals",
                        "My strengths"});
            table33.AddRow(new string[] {
                        "WhatCanYouImprove",
                        "Equals",
                        "What can I improve"});
            table33.AddRow(new string[] {
                        "HobbiesAndInterests",
                        "Equals",
                        "Hobbies and interests"});
#line 230
 testRunner.And("I see", ((string)(null)), table33, "And ");
#line 243
 testRunner.When("I choose SubmitApplication", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 244
 testRunner.Then("I am on the ApplicationCompletePage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 245
 testRunner.When("I choose MyApplicationsLink", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 246
 testRunner.Then("I am on the MyApplicationsPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table34 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table34.AddRow(new string[] {
                        "SubmittedApplicationsCount",
                        "Equals",
                        "1"});
#line 247
 testRunner.And("I see", ((string)(null)), table34, "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
