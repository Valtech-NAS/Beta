namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.Application
{
    using global::SpecBind.Pages;
    using OpenQA.Selenium;

    [PageNavigation("/application/apply/[0-9]+")]
    [PageAlias("ApplicationPage")]
    public class ApplicationPage : BaseValidationPage
    {
        public ApplicationPage(ISearchContext context) : base(context)
        {
        }

        [ElementLocator(Id = "myapplications-link")]
        public IWebElement MyApplicationsLink { get; set; }

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

        [ElementLocator(Id = "Candidate_AboutYou_WhatAreYourStrengths")]
        public IWebElement WhatAreYourStrengths { get; set; }

        [ElementLocator(Id = "Candidate_AboutYou_WhatDoYouFeelYouCouldImprove")]
        public IWebElement WhatCanYouImprove { get; set; }

        [ElementLocator(Id = "Candidate_AboutYou_WhatAreYourHobbiesInterests")]
        public IWebElement HobbiesAndInterests { get; set; }

        [ElementLocator(Id = "Candidate_AboutYou_AnythingWeCanDoToSupportYourInterview")]
        public IWebElement WhatCanWeDoToSupportYou { get; set; }

        [ElementLocator(Id = "apply-button")]
        public IWebElement ApplyButton { get; set; }
    }
}