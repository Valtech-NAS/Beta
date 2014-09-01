namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Applications
{
    using System;
    using Candidate;
    using Common.Constants;
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

        public ApplicationViewModel(string message) : base(message) { }

        public ApplicationViewModel() : base() { }

        public VacancyDetailViewModel VacancyDetail { get; set; } //TODO Make this the summary info as don't need all details
        public CandidateViewModel Candidate { get; set; }
        public ApplicationStatuses Status { get; set; }
        public DateTime? DateUpdated { get; set; }
        public ApplicationAction ApplicationAction { get; set; }
        public int VacancyId { get; set; }

        //Used for validating Qualification and Work Experience
        public int CurrentYear = DateTime.Now.Year;
        public string WhiteListRegex = Whitelists.FreetextWhitelist.RegularExpression;
    }
}
