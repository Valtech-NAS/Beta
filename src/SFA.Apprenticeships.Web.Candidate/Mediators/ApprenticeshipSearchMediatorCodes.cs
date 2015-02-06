namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    public partial class Codes
    {
        public class ApprenticeshipSearch
        {
            public class Index
            {
                public const string Ok = "ApprenticeshipSearch.Index.Ok";
            }

            public class SearchValidation
            {
                public const string Ok = "ApprenticeshipSearch.SearchValidation.Ok";
                public const string ValidationError = "ApprenticeshipSearch.SearchValidation.ValidationError";
            }

            public class Results
            {
                public const string HasError = "ApprenticeshipSearch.Results.HasError";
                public const string Ok = "ApprenticeshipSearch.Results.Ok";
                public const string ValidationError = "ApprenticeshipSearch.Results.ValidationError";
                public const string ExactMatchFound = "ApprenticeshipSearch.Results.ExactMatchFound";
            }

            public class Details
            {
                public const string VacancyNotFound = "ApprenticeshipSearch.Details.VacancyNotFound";
                public const string VacancyHasError = "ApprenticeshipSearch.Details.VacancyHasError";
                public const string Ok = "ApprenticeshipSearch.Details.Ok";
            }
        }
    }
}