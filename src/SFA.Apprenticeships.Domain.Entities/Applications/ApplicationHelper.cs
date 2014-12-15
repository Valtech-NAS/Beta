namespace SFA.Apprenticeships.Domain.Entities.Applications
{
    using System;
    using Exceptions;
    using System.Linq;

    public static class ApplicationHelper
    {
        public static void AssertState(this ApplicationDetail applicationDetail, string errorMessage, params ApplicationStatuses[] allowedUserStatuses)
        {
            if (!allowedUserStatuses.Contains(applicationDetail.Status))
            {
                var expectedStatuses = string.Join(", ", allowedUserStatuses);
                var message = string.Format("Application in invalid state for '{0}' (id: {1}, current: {2}, expected: '{3}')", 
                    errorMessage, 
                    applicationDetail.EntityId, 
                    applicationDetail.Status,
                    expectedStatuses);

                throw new CustomException(message, ErrorCodes.ApplicationInIncorrectStateError);
            }
        }

        public static void SetStateSubmitting(this ApplicationDetail applicationDetail)
        {
            applicationDetail.Status = ApplicationStatuses.Submitting;
            applicationDetail.DateApplied = DateTime.UtcNow;
        }

        public static void SetStateSubmitted(this ApplicationDetail applicationDetail)
        {
            applicationDetail.Status = ApplicationStatuses.Submitted;
        }
        
        public static void SetStateExpiredOrWithdrawn(this ApplicationDetail applicationDetail)
        {
            applicationDetail.Status = ApplicationStatuses.ExpiredOrWithdrawn;
        }

        public static void RevertStateToDraft(this ApplicationDetail applicationDetail)
        {
            applicationDetail.Status = ApplicationStatuses.Draft;
            applicationDetail.DateApplied = null;
        }
    }
}
