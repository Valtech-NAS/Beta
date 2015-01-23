namespace SFA.Apprenticeships.Web.Candidate.ViewModels.MyApplications
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;

    public class MyApprenticeshipApplicationViewModel
    {
        public int VacancyId { get; set; }

        public string Title { get; set; }

        public string EmployerName { get; set; }

        public ApplicationStatuses ApplicationStatus { get; set; }

        public VacancyStatuses VacancyStatus { get; set; }

        public string ApplicationStatusDescription
        {
            get
            {
                switch (ApplicationStatus)
                {
                    case ApplicationStatuses.Draft:
                        return "Draft";

                    case ApplicationStatuses.Submitted:
                    case ApplicationStatuses.Submitting:
                        return "Submitted";

                    case ApplicationStatuses.InProgress:
                        return "In progress";

                    case ApplicationStatuses.ExpiredOrWithdrawn:
                        return "Expired or withdrawn";

                    case ApplicationStatuses.Successful:
                        return "Successful";

                    case ApplicationStatuses.Unsuccessful:
                        return "Unsuccessful";
                }

                return string.Empty;
            }
        }

        public bool IsArchived { get; set; }

        [UIHint("Date")]
        [DataType(DataType.DateTime)]
        public DateTime? DateApplied { get; set; }

        [UIHint("Date")]
        [DataType(DataType.DateTime)]
        public DateTime ClosingDate { get; set; }

        public DateTime DateUpdated { get; set; }

        public string UnsuccessfulReason { get; set; }
    }
}
