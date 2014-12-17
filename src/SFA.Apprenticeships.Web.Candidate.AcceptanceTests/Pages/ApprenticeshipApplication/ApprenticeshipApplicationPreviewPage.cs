namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.Application
{
    using global::SpecBind.Pages;
    using OpenQA.Selenium;

    [PageNavigation("/apprenticeship/preview/[0-9]+")]
    [PageAlias("ApprenticeshipApplicationPreviewPage")]
    public class ApprenticeshipApplicationPreviewPage : BaseValidationPage
    {
        public ApprenticeshipApplicationPreviewPage(ISearchContext context)
            : base(context)
        {
        }

        [ElementLocator(Id = "applicationSavedTopMessage")]
        public IWebElement ApplicationSavedTopMessage { get; set; }

        [ElementLocator(Id = "applicationSavedBottomMessage")]
        public IWebElement ApplicationSavedBottomMessage { get; set; }

        [ElementLocator(Id = "candidate-fullname")]
        public IWebElement Fullname { get; set; }

        [ElementLocator(Id = "candidate-dob")]
        public IWebElement DateOfBirth { get; set; }

        [ElementLocator(Id = "candidate-email")]
        public IWebElement EmailAddress { get; set; }

        [ElementLocator(Id = "candidate-phone")]
        public IWebElement Phonenumber { get; set; }

        [ElementLocator(Id = "candidate-address-line1")]
        public IWebElement AddressLine1 { get; set; }

        [ElementLocator(Id = "candidate-address-line2")]
        public IWebElement AddressLine2 { get; set; }

        [ElementLocator(Id = "candidate-address-line3")]
        public IWebElement AddressLine3 { get; set; }

        [ElementLocator(Id = "candidate-address-line4")]
        public IWebElement AddressLine4 { get; set; }

        [ElementLocator(Id = "candidate-address-postcode")]
        public IWebElement Postcode { get; set; }

        #region About You
        [ElementLocator(Id = "Candidate_AboutYou_WhatAreYourStrengths")]
        public IWebElement WhatAreYourStrengths { get; set; }

        [ElementLocator(Id = "Candidate_AboutYou_WhatDoYouFeelYouCouldImprove")]
        public IWebElement WhatCanYouImprove { get; set; }

        [ElementLocator(Id = "Candidate_AboutYou_WhatAreYourHobbiesInterests")]
        public IWebElement HobbiesAndInterests { get; set; }

        [ElementLocator(Id = "Candidate_AboutYou_AnythingWeCanDoToSupportYourInterview")]
        public IWebElement WhatCanWeDoToSupportYou { get; set; }

        #endregion

        #region Education

        [ElementLocator(Id = "Candidate_Education_NameOfMostRecentSchoolCollege")]
        public IWebElement EducationNameOfSchool { get; set; }

        [ElementLocator(Id = "Candidate_Education_FromYear")]
        public IWebElement EducationFromYear { get; set; }

        [ElementLocator(Id = "Candidate_Education_ToYear")]
        public IWebElement EducationToYear { get; set; }

        #endregion

        #region Qualifications

        [ElementLocator(Id = "no-qualifications")]
        public IWebElement NoQualificationsMessage { get; set; }

        #endregion

        #region Work Experience

        [ElementLocator(Id = "no-work-experience")]
        public IWebElement NoWorkExperienceMessage { get; set; }

        #endregion

        [ElementLocator(Id = "submit-application")]
        public IWebElement SubmitApplication { get; set; }
    }
}