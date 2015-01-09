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
        }
    }
}