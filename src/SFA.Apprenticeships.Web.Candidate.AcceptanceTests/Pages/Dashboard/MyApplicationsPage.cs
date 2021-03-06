﻿namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.Dashboard
{
    using System;
    using System.Linq;
    using global::SpecBind.Pages;
    using OpenQA.Selenium;
    using SpecBind.Selenium;

    [PageNavigation("/myapplications")]
    [PageAlias("MyApplicationsPage")]
    public class MyApplicationsPage : BaseValidationPage
    {
        public MyApplicationsPage(ISearchContext context)
            : base(context)
        {
        }

        [ElementLocator(Id = "draft-applications-count")]
        public IWebElement DraftApplicationsCount { get; set; }

        [ElementLocator(Id = "submitted-applications-count")]
        public IWebElement SubmittedApplicationsCount { get; set; }

        [ElementLocator(Id = "successful-applications-count")]
        public IWebElement SuccessfulApplicationsCount { get; set; }

        [ElementLocator(Id = "unsuccessful-applications-count")]
        public IWebElement UnsuccessfulApplicationsCount { get; set; }

        [ElementLocator(Id = "traineeship-applications-count")]
        public IWebElement TraineeshipApplicationsCount { get; set; }

        [ElementLocator(Class = "resume-link")]
        public IWebElement ResumeLink { get; set; }

        [ElementLocator(Class = "delete-draft")]
        public IWebElement DeleteDraftLink { get; set; }

        [ElementLocator(Class = "archive-successful")]
        public IWebElement ArchiveSuccessfulLink { get; set; }

        [ElementLocator(Class = "archive-unsuccessful")]
        public IWebElement ArchiveUnsuccessfulLink { get; set; }

        [ElementLocator(Id = "empty-application-history-text")]
        public IWebElement EmptyApplicationHistoryText { get; set; }

        [ElementLocator(Id = "traineeshipPrompt")]
        public IWebElement TraineeshipsPrompt { get; set; }

        [ElementLocator(Id = "application-status-description")]
        public IWebElement ApplicationStatusDescription { get; set; }

        [ElementLocator(Id = "find-traineeship-link")]
        public IWebElement FindTraineeshipLink { get; set; }

        [ElementLocator(Id = "find-apprenticeship-button")]
        public IWebElement FindApprenticeshipButton { get; set; }

        [ElementLocator(Id = "find-apprenticeship-link")]
        public IWebElement FindApprenticeshipLink { get; set; }

        public string TraineeshipsPromptDisplayed
        {
            get { return TraineeshipsPrompt.Displayed.ToString(); }
        }

        [ElementLocator(Id = "traineeship-overview-link")]
        public IWebElement TraineeshipOverviewLink { get; set; }

        [ElementLocator(Id = "dismiss-traineeship-prompts-link")]
        public IWebElement DismissTraineeshipPromptsLink { get; set; }

        [ElementLocator(Id = "traineeships-table")]
        public IWebElement TraineeshipTable { get; set; }

        [ElementLocator(Class = "btnExpandRows")]
        public IWebElement MoreTraineeshipsLink { get; set; }

        [ElementLocator(Id = "traineeships-table")]
        public IElementList<IWebElement, TraineeshipTableRow> TraineeshipTableRows { get; set; }

        public string TraineeshipTableRowCount
        {
            get { return Convert.ToString(TraineeshipTableRows.Count()); }
        }

        [ElementLocator(Class = "traineeship-table-row")]
        public class TraineeshipTableRow : WebElement
        {
            public TraineeshipTableRow(ISearchContext parent)
                : base(parent)
            {
            }
        }

        [ElementLocator(Id = "candidate-support-message")]
        public IWebElement CandidateSupportMessage { get; set; }
    }
}