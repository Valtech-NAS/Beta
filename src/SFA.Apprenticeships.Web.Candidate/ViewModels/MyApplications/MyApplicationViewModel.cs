namespace SFA.Apprenticeships.Web.Candidate.ViewModels.MyApplications
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Domain.Entities.Applications;

    public class MyApplicationViewModel
    {
        public int VacancyId { get; set; }

        public string Title { get; set; }

        public ApplicationStatuses ApplicationStatus { get; set; }

        [UIHint("Date")]
        [DataType(DataType.DateTime)]
        public DateTime? DateApplied { get; set; }

        // NOTE: this relates to the candidate withdrawing or declining.
        public string WithdrawnOrDeclinedReason { get; set; }

        // NOTE: this relates to the vacancy manager rejecting a candidate's application
        public string UnsuccessfulReason { get; set; }
    }
}
