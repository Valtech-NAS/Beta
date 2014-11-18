namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.Application
{
    using System.Globalization;
    using global::SpecBind.Pages;
    using OpenQA.Selenium;
    using SpecBind.Selenium;
    using System.Linq;
    using SummaryItems;

    [PageNavigation("/application/apply/[0-9]+")]
    [PageAlias("ApplicationPage")]
    public class ApplicationPage : BaseValidationPage
    {
        private IElementList<IWebElement, QualificationTypeDropdownItem> _qualificationTypeDropdown;

        public ApplicationPage(ISearchContext context) : base(context)
        {
        }

        #region Save and apply
        [ElementLocator(Id = "applicationSavedTopMessage")]
        public IWebElement ApplicationSavedMessage { get; set; }

        [ElementLocator(Id = "apply-button")]
        public IWebElement ApplyButton { get; set; }

        [ElementLocator(Id = "save-button")]
        public IWebElement SaveButton { get; set; }
        #endregion

        #region General information

        [ElementLocator(Id = "vacancy-title")]
        public IWebElement VacancyTitle { get; set; }

        [ElementLocator(Id = "vacancy-sub-title")]
        public IWebElement VacancySubTitle { get; set; }

        [ElementLocator(Id = "vacancy-summary")]
        public IWebElement VacancySummary { get; set; }

        [ElementLocator(Id = "Fullname")]
        public IWebElement Fullname { get; set; }

        [ElementLocator(Id = "candidate-fullname")]
        public IWebElement FullnameReadOnly { get; set; }

        [ElementLocator(Id = "candidate-email")]
        public IWebElement EmailReadOnly { get; set; }

        [ElementLocator(Id = "candidate-dob")]
        public IWebElement DobReadOnly { get; set; }

        [ElementLocator(Id = "candidate-phone")]
        public IWebElement PhoneReadOnly { get; set; }

        [ElementLocator(Id = "candidate-address-line1")]
        public IWebElement AddressLine1ReadOnly { get; set; }

        [ElementLocator(Id = "candidate-address-line2")]
        public IWebElement AddressLine2ReadOnly { get; set; }

        [ElementLocator(Id = "candidate-address-line3")]
        public IWebElement AddressLine3ReadOnly { get; set; }

        [ElementLocator(Id = "candidate-address-line4")]
        public IWebElement AddressLine4ReadOnly { get; set; }

        [ElementLocator(Id = "candidate-address-postcode")]
        public IWebElement AddressPostcodeReadOnly { get; set; }

        [ElementLocator(Id = "EmailAddress")]
        public IWebElement EmailAddress { get; set; }

        [ElementLocator(Id = "DateOfBirth")]
        public IWebElement DataOfBirth { get; set; }

        [ElementLocator(Id = "PhoneNumber")]
        public IWebElement PhoneNumber { get; set; }

        [ElementLocator(Id = "Address")]
        public IWebElement Address { get; set; }

        #region Education

        [ElementLocator(Id = "Candidate_Education_NameOfMostRecentSchoolCollege")]
        public IWebElement EducationNameOfSchool { get; set; }

        [ElementLocator(Id = "Candidate_Education_FromYear")]
        public IWebElement  EducationFromYear { get; set; }

        [ElementLocator(Id = "Candidate_Education_ToYear")]
        public IWebElement EducationToYear { get; set; }

        [ElementLocator(Text = "Please enter name of most recent school or college")]
        public IWebElement EducationNameOfSchoolError { get; set; }

        [ElementLocator(Text = "Please enter year started")]
        public IWebElement EducationFromYearError { get; set; }

        [ElementLocator(Text = "Please enter year finished")]
        public IWebElement EducationToYearError { get; set; }
        #endregion
        #endregion

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
                return QualificationsSummaryItems.Count().ToString(CultureInfo.InvariantCulture); 
            }
        }

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
                return WorkExperienceSummaryItems.Count().ToString(CultureInfo.InvariantCulture);
            }
        }

        [ElementLocator(Id = "qualifications-panel")]
        public WorkExperiencePanel QualificationsPanel { get; set; }

        public string QualificationsValidationErrorsCount
        {
            get
            {
                return QualificationsPanel.ValidationErrorsCount;
            }
        }

        [ElementLocator(Id = "workexperience-panel")]
        public WorkExperiencePanel WorkExperiencePanel { get; set; }

        public string WorkExperienceValidationErrorsCount
        {
            get
            {
                return WorkExperiencePanel.ValidationErrorsCount;
            }
        }

        #endregion

        #region About You

        [ElementLocator(Id = "Candidate_AboutYou_WhatAreYourStrengths")]
        public IWebElement WhatAreYourStrengths { get; set; }

        [ElementLocator(Id = "Candidate_AboutYou_WhatDoYouFeelYouCouldImprove")]
        public IWebElement WhatCanYouImprove { get; set; }

        [ElementLocator(Id = "Candidate_AboutYou_WhatAreYourHobbiesInterests")]
        public IWebElement HobbiesAndInterests { get; set; }
        #endregion

        #region WhatCanWeDoToSupportYou

        [ElementLocator(Id = "support-yes")]
        public IWebElement SupportMeYes { get; set; }

        [ElementLocator(Id = "support-no")]
        public IWebElement SupportMeNo { get; set; }

        [ElementLocator(Id = "Candidate_AboutYou_AnythingWeCanDoToSupportYourInterview")]
        public IWebElement WhatCanWeDoToSupportYou { get; set; }

        #endregion
    }

    [ElementLocator(TagName = "option")]
    public class QualificationTypeDropdownItem : WebElement
    {
        public QualificationTypeDropdownItem(ISearchContext parent)
            : base(parent)
        {
        }
    }

    
    public class WorkExperiencePanel : WebElement
    {
        public WorkExperiencePanel(ISearchContext parent) : base(parent)
        {
        }
    
        public string ValidationErrorsCount
        {
            get
            {
                var count = FindElements(By.ClassName("field-validation-error"))
                    .Count(fve => fve.GetCssValue("display") != "none")
                    .ToString(CultureInfo.InvariantCulture);
                return count;
                //GetCssValue("color").ToLower() == "rgba(223, 48, 52, 1)".ToLower()).Count().ToString();
            }
        }
    }

    public class QualificationsPanel : WebElement
    {
        public QualificationsPanel(ISearchContext parent)
            : base(parent)
        {
        }

        public string ValidationErrorsCount
        {
            get
            {
                var count = FindElements(By.ClassName("field-validation-error"))
                    .Count(fve => fve.GetCssValue("display") != "none")
                    .ToString(CultureInfo.InvariantCulture);
                return count;
                //GetCssValue("color").ToLower() == "rgba(223, 48, 52, 1)".ToLower()).Count().ToString();
            }
        }
    }
}