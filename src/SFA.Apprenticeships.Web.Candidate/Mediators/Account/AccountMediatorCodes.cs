namespace SFA.Apprenticeships.Web.Candidate.Mediators.Account
{
    public class AccountMediatorCodes
    {
        public class Archive
        {
            public const string SuccessfullyArchived = "AccountMediator.Archive.SuccessfullyArchived";
            public const string ErrorArchiving = "AccountMediator.Archive.ErrorArchiving";
        }

        public class Index
        {
            public const string Success = "AccountMediator.Index.Success";
        }

        public class Delete
        {
            public const string SuccessfullyDeleted = "AccountMediator.Delete.SuccessfullyDeleted";
            public const string SuccessfullyDeletedExpiredOrWithdrawn = "AccountMediator.Delete.SuccessfullyDeletedExpiredOrWithdrawn";
            public const string ErrorDeleting = "AccountMediator.Delete.ErrorDeleting";
            public const string AlreadyDeleted = "AccountMediator.Delete.AlreadyDeleted";
        }

        public class Settings
        {
            public const string Success = "AccountMediator.Settings.Success";
            public const string ValidationError = "AccountMediator.Settings.ValidationError";
            public const string SaveError = "AccountMediator.Settings.SaveError";
            public const string MobileVerificationRequired = "AccountMediator.Settings.MobileVerificationRequired";
        }

        public class DismissTraineeshipPrompts
        {
            public const string SuccessfullyDismissed = "AccountMediator.DismissTraineeshipPrompts.SuccessfullyDismissed";
            public const string ErrorDismissing = "AccountMediator.DismissTraineeshipPrompts.ErrorDismissing";
        }

        public class Track
        {
            public const string SuccessfullyTracked = "AccountMediator.Track.SuccessfullyTracked";
            public const string ErrorTracking = "AccountMediator.Track.ErrorTracking";
        }

        public class AcceptTermsAndConditions
        {
            public const string SuccessfullyAccepted = "AccountMediator.AcceptTermsAndConditions.SuccessfullyAccepted";
            public const string AlreadyAccepted = "AccountMediator.AcceptTermsAndConditions.AlreadyAccepted";
            public const string ErrorAccepting = "AccountMediator.AcceptTermsAndConditions.ErrorAccepting";
        }

        public class VacancyDetails
        {
            public const string Available = "AccountMediator.ApprenticeshipDetails.Available";
            public const string Unavailable = "AccountMediator.ApprenticeshipDetails.Unavailable";
            public const string Error = "AccountMediator.ApprenticeshipDetails.Error";
        }

        public class VerifyMobile
        {
            public const string Success = "AccountMediator.VerifyMobile.Success";
            public const string ValidationError = "AccountMediator.VerifyMobile.ValidationError";
            public const string VerificationNotRequired = "AccountMediator.VerifyMobile.Failed";
            public const string InvalidCode = "AccountMediator.VerifyMobile.InvalidCode";
            public const string Error = "AccountMediator.VerifyMobile.Error";
            public const string ReturnUrl = "AccountMediator.VerifyMobile.ReturnUrl";
        }

        public class Resend
        {
            public const string ResentSuccessfully = "AccountMediator.Resend.Successfully";
            public const string ResendNotRequired = "AccountMediator.Resend.Failed";
            public const string Error = "AccountMediator.Resend.Error";
        }
    }
}
