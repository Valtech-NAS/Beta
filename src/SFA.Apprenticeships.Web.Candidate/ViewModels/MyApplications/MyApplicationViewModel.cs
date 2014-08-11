namespace SFA.Apprenticeships.Web.Candidate.ViewModels.MyApplications
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Domain.Entities.Applications;

    public class MyApplicationViewModel
    {
        public int VacancyId { get; set; }

        public string Title { get; set; }

        public string EmployerName { get; set; }

        public ApplicationStatuses ApplicationStatus { get; set; }

        [UIHint("Date")]
        [DataType(DataType.DateTime)]
        public DateTime? DateApplied { get; set; }

        public string WithdrawnOrDeclinedReason { get; set; }

        public string UnsuccessfulReason { get; set; }
    }
}
