namespace SFA.Apprenticeships.Web.Candidate.ViewModels.MyApplications
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Domain.Entities.Vacancies;

    public class MyTraineeshipApplicationViewModel
    {
        public int VacancyId { get; set; }

        public VacancyStatuses VacancyStatus { get; set; }

        public string Title { get; set; }

        public string EmployerName { get; set; }

        public bool IsArchived { get; set; }

        [UIHint("Date")]
        [DataType(DataType.DateTime)]
        public DateTime? DateApplied { get; set; }
    }
}