namespace SFA.Apprenticeships.Application.ApplicationUpdate.Extensions
{
    using Domain.Entities.Applications;
    using Entities;

    internal static class TraineeshipApplicationDetailExtension
    {
        internal static bool UpdateTraineeshipApplicationDetail(this TraineeshipApplicationDetail traineeshipApplication, ApplicationStatusSummary applicationStatusSummary)
        {
            var updated = false;

            if (applicationStatusSummary.IsLegacySystemUpdate())
            {
                // Only update application status etc. if update originated from Legacy system.
                if (traineeshipApplication.Status != applicationStatusSummary.ApplicationStatus)
                {
                    traineeshipApplication.Status = applicationStatusSummary.ApplicationStatus;

                    // Application status has changed, ensure it appears on the candidate's dashboard.
                    traineeshipApplication.IsArchived = false;
                    updated = true;
                }

                if (traineeshipApplication.LegacyApplicationId != applicationStatusSummary.LegacyApplicationId)
                {
                    // Ensure the application is linked to the legacy application.
                    traineeshipApplication.LegacyApplicationId = applicationStatusSummary.LegacyApplicationId;
                    updated = true;
                }
            }

            if (traineeshipApplication.VacancyStatus != applicationStatusSummary.VacancyStatus)
            {
                traineeshipApplication.VacancyStatus = applicationStatusSummary.VacancyStatus;
                updated = true;
            }

            if (traineeshipApplication.Vacancy.ClosingDate != applicationStatusSummary.ClosingDate)
            {
                traineeshipApplication.Vacancy.ClosingDate = applicationStatusSummary.ClosingDate;
                updated = true;
            }

            return updated;
        }
    }
}
