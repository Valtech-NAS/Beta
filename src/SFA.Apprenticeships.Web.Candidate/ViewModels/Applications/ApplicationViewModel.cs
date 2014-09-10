namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Applications
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Candidate;
    using Common.Constants;
    using Constants.Pages;
    using Domain.Entities.Applications;
    using VacancySearch;

    [Serializable]
    public enum ApplicationAction
    {
        Preview,
        Save
    }

    [Serializable]
    public class ApplicationViewModel : ViewModelBase
    {
        //Constants used on application form
        public int DefaultQualificationRows = 5;
        public int DefaultWorkExperienceRows = 3;
        public int CurrentYear = DateTime.Now.Year;
        public double SessionTimeout;
        public string ConfirmationMessage = ApplicationPageMessages.LeavingPageMessage;
        public string WhiteListRegex = Whitelists.FreetextWhitelist.RegularExpression;

        public ApplicationViewModel(string message) : base(message)
        {

        }

        public ApplicationViewModel()
        {
        }

        public IEnumerable<SelectListItem> Months
        {
            get
            {
                var list = new List<SelectListItem>
                {
                    new SelectListItem {Selected = false, Text = "Jan", Value = "1"},
                    new SelectListItem {Selected = false, Text = "Feb", Value = "2"},
                    new SelectListItem {Selected = false, Text = "Mar", Value = "3"},
                    new SelectListItem {Selected = false, Text = "Apr", Value = "4"},
                    new SelectListItem {Selected = false, Text = "May", Value = "5"},
                    new SelectListItem {Selected = false, Text = "June", Value = "6"},
                    new SelectListItem {Selected = false, Text = "July", Value = "7"},
                    new SelectListItem {Selected = false, Text = "Aug", Value = "8"},
                    new SelectListItem {Selected = false, Text = "Sept", Value = "9"},
                    new SelectListItem {Selected = false, Text = "Oct", Value = "10"},
                    new SelectListItem {Selected = false, Text = "Nov", Value = "11"},
                    new SelectListItem {Selected = false, Text = "Dec", Value = "12"}
                };

                return new SelectList(list, "Value", "Text");
            }
        }

        public VacancyDetailViewModel VacancyDetail { get; set; }
        //TODO Make this the summary info as don't need all details
        public CandidateViewModel Candidate { get; set; }
        public ApplicationStatuses Status { get; set; }
        public DateTime? DateUpdated { get; set; }
        public ApplicationAction ApplicationAction { get; set; }
        public int VacancyId { get; set; }
        public bool IsJavascript  { get; set; }
    }
}