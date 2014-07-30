namespace SFA.Apprenticeships.Domain.Entities.Applications
{
    using Exceptions;
    using System.Linq;

    public static class ApplicationHelper
    {
        public static void AssertState(this ApplicationDetail applicationDetail, string errorMessage, params ApplicationStatuses[] allowedUserStatuses)
        {
            if (!allowedUserStatuses.Contains(applicationDetail.Status))
            {
                throw new CustomException(errorMessage, ErrorCodes.ApplicationInIncorrectStateError);
            }
        }
    }
}