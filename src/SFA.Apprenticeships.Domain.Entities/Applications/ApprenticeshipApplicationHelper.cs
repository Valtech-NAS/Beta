namespace SFA.Apprenticeships.Domain.Entities.Applications
{
    using System;
    using Exceptions;
    using System.Linq;

    public static class ApprenticeshipApplicationHelper
    {
        public static void AssertState(this ApprenticeshipApplicationDetail apprenticeshipApplicationDetail, string errorMessage, params ApplicationStatuses[] allowedUserStatuses)
        {
            if (!allowedUserStatuses.Contains(apprenticeshipApplicationDetail.Status))
            {
                var expectedStatuses = string.Join(", ", allowedUserStatuses);
                var message = string.Format("Application in invalid state for '{0}' (id: {1}, current: {2}, expected: '{3}')", 
                    errorMessage,
                    apprenticeshipApplicationDetail.EntityId,
                    apprenticeshipApplicationDetail.Status,
                    expectedStatuses);

                throw new CustomException(message, ErrorCodes.ApplicationInIncorrectStateError);
            }
        }

        public static void SetStateSubmitting(this ApprenticeshipApplicationDetail apprenticeshipApplicationDetail)
        {
            apprenticeshipApplicationDetail.Status = ApplicationStatuses.Submitting;
            apprenticeshipApplicationDetail.DateApplied = DateTime.UtcNow;
        }

        public static void SetStateSubmitted(this ApprenticeshipApplicationDetail apprenticeshipApplicationDetail)
        {
            apprenticeshipApplicationDetail.Status = ApplicationStatuses.Submitted;
        }

        public static void SetStateExpiredOrWithdrawn(this ApprenticeshipApplicationDetail apprenticeshipApplicationDetail)
        {
            apprenticeshipApplicationDetail.Status = ApplicationStatuses.ExpiredOrWithdrawn;
        }

        public static void RevertStateToDraft(this ApprenticeshipApplicationDetail apprenticeshipApplicationDetail)
        {
            apprenticeshipApplicationDetail.Status = ApplicationStatuses.Draft;
            apprenticeshipApplicationDetail.DateApplied = null;
        }
    }
}
