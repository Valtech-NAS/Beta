namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    public partial class Codes
    {
        public class TraineeshipSearch
        {
            public class Index
            {
                public const string Ok = "TraineeshipSearch.Index.Ok";
            }

            public class Results
            {
                public const string Ok = "TraineeshipSearch.Results.Ok";
                public const string ValidationError = "TraineeshipSearch.Results.ValidationError";
                public const string HasError = "TraineeshipSearch.Results.HasError";
            }

            public class Details
            {
                public const string Ok = "TraineeshipSearch.Details.Ok";
                public const string VacancyNotFound = "TraineeshipSearch.Details.VacancyNotFound";
                public const string VacancyHasError = "TraineeshipSearch.Details.VacancyHasError";
            }
        }
    }
}