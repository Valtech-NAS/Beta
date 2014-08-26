namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.Application
{
    using global::SpecBind.Pages;
    using OpenQA.Selenium;
    using SpecBind.Selenium;
    using System.Linq;
    using System.Collections.Generic;

    [PageNavigation("/application/apply/[0-9]+")]
    [PageAlias("ApplicationPage")]
    public class ApplicationPage : BaseValidationPage
    {
        private IElementList<IWebElement, QualificationTypeDropdownItem> _qualificationTypeDropdown;

        public ApplicationPage(ISearchContext context) : base(context)
        {
        }

        [ElementLocator(Id = "applicationSavedTopMessage")]
        public IWebElement ApplicationSavedMessage { get; set; }

        [ElementLocator(Id = "vacancy-title")]
        public IWebElement VacancyTitle { get; set; }

        [ElementLocator(Id = "vacancy-sub-title")]
        public IWebElement VacancySubTitle { get; set; }

        [ElementLocator(Id = "vacancy-summary")]
        public IWebElement VacancySummary { get; set; }

        [ElementLocator(Id = "Fullname")]
        public IWebElement Fullname { get; set; }

        [ElementLocator(Id = "EmailAddress")]
        public IWebElement EmailAddress { get; set; }

        [ElementLocator(Id = "DateOfBirth")]
        public IWebElement DataOfBirth { get; set; }

        [ElementLocator(Id = "PhoneNumber")]
        public IWebElement PhoneNumber { get; set; }

        [ElementLocator(Id = "Address")]
        public IWebElement Address { get; set; }

        [ElementLocator(Id = "Candidate_Education_NameOfMostRecentSchoolCollege")]
        public IWebElement EducationNameOfSchool { get; set; }

        [ElementLocator(Id = "Candidate_Education_FromYear")]
        public IWebElement  EducationFromYear { get; set; }

        [ElementLocator(Id = "Candidate_Education_ToYear")]
        public IWebElement EducationToYear { get; set; }


        #region Qualifications

        [ElementLocator(Id = "qualifications-yes")]
        public IWebElement QualificationsYes { get; set; }

        [ElementLocator(Id = "qualifications-no")]
        public IWebElement QualificationsNo { get; set; }

        [ElementLocator(Id = "qualification-validation-summary")]
        public IWebElement QualificationValidationSummary { get; set; }

        [ElementLocator(Id = "saveQualification")]
        public IWebElement SaveQualification { get; set; }

        [ElementLocator(Id="qual-type")]
        public IElementList<IWebElement, QualificationTypeDropdownItem> QualificationTypeDropdown
        {
            get { return _qualificationTypeDropdown; }
            set { _qualificationTypeDropdown = value; }
        }

        [ElementLocator(Id = "subject-year")]
        public IWebElement SubjectYear { get; set; }

        [ElementLocator(Id = "subject-name")]
        public IWebElement SubjectName { get; set; }

        [ElementLocator(Id = "subject-grade")]
        public IWebElement SubjectGrade { get; set; }

        [ElementLocator(Id = "qualifications-summary")]
        public IWebElement QualificationsSummary { get; set; }

        [ElementLocator(Id = "qualifications-summary")]
        public IElementList<IWebElement, QualificationSummaryItem> QualificationsSummaryItems { get; set; }

        public string QualificationsSummaryCount
        {
            get { 
                return QualificationsSummaryItems.Count().ToString(); 
            }
        }

        [ElementLocator(Id = "edit-qualification-link")]
        public IWebElement EditQualificationLink { get; set; }

        [ElementLocator(Class = "icon-remove")]
        public IWebElement RemoveQualificationLink { get; set; }

        [ElementLocator(Class="field-validation-error")]
        public IWebElement FieldValidationError { get; set; }

        #endregion

        #region Work Experience

        [ElementLocator(Id = "workexp-yes")]
        public IWebElement WorkExperienceYes { get; set; }

        [ElementLocator(Id = "workexp-no")]
        public IWebElement WorkExperienceNo { get; set; }

        [ElementLocator(Id = "addWorkBtn")]
        public IWebElement SaveWorkExperience { get; set; }

        public string WorkExperienceValidationErrorsCount
        {
            get
            {
                return Context.FindElements(By.ClassName("field-validation-error")).Where(e => e.GetCssValue("color").ToLower() == "rgba(223, 48, 52, 1)".ToLower()).Count().ToString();
            }
        }

        [ElementLocator(Id = "work-employer")]
        public IWebElement WorkEmployer { get; set; }

        [ElementLocator(Id = "work-title")]
        public IWebElement WorkTitle { get; set; }

        [ElementLocator(Id = "work-role")]
        public IWebElement WorkRole { get; set; }

        [ElementLocator(Id = "work-from-year")]
        public IWebElement WorkFromYear { get; set; }

        [ElementLocator(Id = "work-to-year")]
        public IWebElement WorkToYear { get; set; }

        [ElementLocator(Id = "work-experience-summary")]
        public IWebElement WorkExperienceSummary { get; set; }

        [ElementLocator(Id = "work-experience-summary")]
        public IElementList<IWebElement, WorkExperienceSummaryItem> WorkExperienceSummaryItems { get; set; }

        public string WorkExperiencesCount
        {
            get
            {
                return WorkExperienceSummaryItems.Count().ToString();
            }
        }

        #endregion

        [ElementLocator(Id = "Candidate_AboutYou_WhatAreYourStrengths")]
        public IWebElement WhatAreYourStrengths { get; set; }

        [ElementLocator(Id = "Candidate_AboutYou_WhatDoYouFeelYouCouldImprove")]
        public IWebElement WhatCanYouImprove { get; set; }

        [ElementLocator(Id = "Candidate_AboutYou_WhatAreYourHobbiesInterests")]
        public IWebElement HobbiesAndInterests { get; set; }

        #region WhatCanWeDoToSupportYou

        [ElementLocator(Id = "support-yes")]
        public IWebElement SupportMeYes { get; set; }

        [ElementLocator(Id = "support-no")]
        public IWebElement SupportMeNo { get; set; }

        [ElementLocator(Id = "Candidate_AboutYou_AnythingWeCanDoToSupportYourInterview")]
        public IWebElement WhatCanWeDoToSupportYou { get; set; }

        #endregion

        [ElementLocator(Id = "apply-button")]
        public IWebElement ApplyButton { get; set; }

        [ElementLocator(Id = "save-button")]
        public IWebElement SaveButton { get; set; }
    }

    [ElementLocator(TagName = "option")]
    public class QualificationTypeDropdownItem : WebElement
    {
        public QualificationTypeDropdownItem(ISearchContext parent)
            : base(parent)
        {
        }
    }

    [ElementLocator(TagName = "tr")]
    public class QualificationSummaryItem : WebElement
    {
        public QualificationSummaryItem(ISearchContext parent)
            : base(parent)
        {
        }

        public string Subject
        {
            get { return GetCandidateQualificationInputValue("subject"); }
        }

        public string Year
        {
            get
            {
                return GetCandidateQualificationInputValue("year");
            }
        }

        public string Grade
        {
            get { return GetCandidateQualificationInputValue("grade"); }
        }

        private string GetCandidateQualificationInputValue(string key)
        {
            return FindElements(By.TagName("input")).Where(e => e.GetAttribute("id").StartsWith("candidate_qualifications_"))
                .Where(e => e.GetAttribute("id").Contains(key)).First()
                .GetAttribute("value"); ;
        }
    }

    [ElementLocator(Id = "work-history-item")]
    public class WorkExperienceSummaryItem : WebElement
    {
        public WorkExperienceSummaryItem(ISearchContext parent)
            : base(parent)
        {
        }

        public string WorkEmployer
        {
            get
            {
                var workEmployer = FindElement(By.TagName("table")).FindElement(By.TagName("tbody"))
                    .Text.Split(new char[]{'-'})[0].Trim();
                		//Text	"WorkEmployer -\r\nWorkTitle\r\nWorkRole"	string

                return workEmployer;
            }
        }

        public string WorkTitle
        {
            get
            {
                var workTitleRole = FindElement(By.TagName("table")).FindElement(By.TagName("tbody"))
                    .Text.Split(new char[] { '-' })[1];

                var workTitle = workTitleRole.Split(new string[]{"\r\n"}, System.StringSplitOptions.RemoveEmptyEntries)[0];

                return workTitle;
            }
        }

        public string WorkRole
        {
            get
            {
                var workTitleRole = FindElement(By.TagName("table")).FindElement(By.TagName("tbody"))
                    .Text.Split(new char[] { '-' })[1];

                var workRole = workTitleRole.Split(new string[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries)[0];

                return workRole;
            }
        }



        //public string Year
        //{
        //    get
        //    {
        //        return GetCandidateQualificationInputValue("year");
        //    }
        //}

        //public string Grade
        //{
        //    get { return GetCandidateQualificationInputValue("grade"); }
        //}

        //private string GetCandidateQualificationInputValue(string key)
        //{
        //    return FindElements(By.TagName("input")).Where(e => e.GetAttribute("id").StartsWith("candidate_qualifications_"))
        //        .Where(e => e.GetAttribute("id").Contains(key)).First()
        //        .GetAttribute("value"); ;
        //}
    }
}