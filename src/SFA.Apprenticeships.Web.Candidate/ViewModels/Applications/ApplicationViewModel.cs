namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Applications
{
    using System;
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
        public string ConfirmationMessage = ApplicationPageMessages.LeavingPageMessage;
        public int CurrentYear = DateTime.Now.Year;
        public double SessionTimeout;
        public string WhiteListRegex = Whitelists.FreetextWhitelist.RegularExpression;

        public ApplicationViewModel(string message) : base(message)
        {
        }

        public ApplicationViewModel()
        {
        }

        public VacancyDetailViewModel VacancyDetail { get; set; }
        //TODO Make this the summary info as don't need all details
        public CandidateViewModel Candidate { get; set; }
        public ApplicationStatuses Status { get; set; }
        public DateTime? DateUpdated { get; set; }
        public ApplicationAction ApplicationAction { get; set; }
        public int VacancyId { get; set; }
    }
}