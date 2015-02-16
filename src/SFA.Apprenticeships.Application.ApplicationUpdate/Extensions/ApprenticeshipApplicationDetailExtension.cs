namespace SFA.Apprenticeships.Application.ApplicationUpdate.Extensions
{
    using Domain.Entities.Applications;
    using Entities;

    internal static class ApprenticeshipApplicationDetailExtension
    {
        internal static bool UpdateApprenticeshipApplicationDetail(this ApprenticeshipApplicationDetail apprenticeshipApplication,
            ApplicationStatusSummary applicationStatusSummary)
        {
            var updated = false;

            if (apprenticeshipApplication.Status != applicationStatusSummary.ApplicationStatus)
            {
                apprenticeshipApplication.Status = applicationStatusSummary.ApplicationStatus;

                // Application status has changed, ensure it appears on the candidate's dashboard.
                apprenticeshipApplication.IsArchived = false;
                updated = true;
            }

            // TODO: AG: comment 'applicationStatusSummary.LegacyApplicationId != 0'.
            if (applicationStatusSummary.LegacyApplicationId != 0 && apprenticeshipApplication.LegacyApplicationId != applicationStatusSummary.LegacyApplicationId)
            {
                // Ensure the application is linked to the legacy application (if any).
                apprenticeshipApplication.LegacyApplicationId = applicationStatusSummary.LegacyApplicationId;
                updated = true;
            }

            if (apprenticeshipApplication.VacancyStatus != applicationStatusSummary.VacancyStatus)
            {
                apprenticeshipApplication.VacancyStatus = applicationStatusSummary.VacancyStatus;
                updated = true;
            }

            if (apprenticeshipApplication.Vacancy.ClosingDate != applicationStatusSummary.ClosingDate)
            {
                apprenticeshipApplication.Vacancy.ClosingDate = applicationStatusSummary.ClosingDate;
                updated = true;
            }

            if (apprenticeshipApplication.UnsuccessfulReason != applicationStatusSummary.UnsuccessfulReason)
            {
                apprenticeshipApplication.UnsuccessfulReason = applicationStatusSummary.UnsuccessfulReason;
                updated = true;
            }

            return updated;
        }
    }
}
