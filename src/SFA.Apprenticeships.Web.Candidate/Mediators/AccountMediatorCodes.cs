namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    public partial class Codes
    {
        public class AccountMediator
        {
            public class Archive
            {
                public const string SuccessfullyArchived = "SuccessfullyArchived";
                public const string ErrorArchiving = "ErrorArchiving";
            }

            public class Index
            {
                public const string Success = "Success";
            }

            public class Delete
            {
                public const string SuccessfullyDeleted = "SuccessfullyDeleted";
                public const string SuccessfullyDeletedExpiredOrWithdrawn = "SuccessfullyDeletedExpiredOrWithdrawn";
                public const string ErrorDeleting = "ErrorDeleting";
                public const string AlreadyDeleted = "AlreadyDeleted";
            }

            public class Settings
            {
                public const string Success = "Success";
                public const string ValidationError = "ValError";
                public const string SaveError = "SaveError";
            }

            public class DismissTraineeshipPrompts
            {
                public const string SuccessfullyDismissed = "SuccessfullyDismissed";
                public const string ErrorDismissing = "ErrorDismissing";
            }

            public class Track
            {
                public const string SuccessfullyTracked = "SuccessfullyTracked";
                public const string ErrorTracking = "ErrorTracking";
            }

            public class AcceptTermsAndConditions
            {
                public const string SuccessfullyAccepted = "SuccessfullyAccepted";
                public const string AlreadyAccepted = "AlreadyAccepted";
                public const string ErrorAccepting = "ErrorAccepting";
            }

            public class ApprenticeshipDetails
            {
                public const string VacancyAvailable = "ApprenticeshipDetails.VacancyAvailable";
                public const string VacancyUnavailable = "ApprenticeshipDetails.VacancyUnavailable";
                public const string Error = "ApprenticeshipDetails.Error";
            }
        }
    }
}