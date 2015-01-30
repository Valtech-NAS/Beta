namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    public partial class Codes
    {
        public class ApprenticeshipSearch
        {
            public class Index
            {
                public const string Ok = "search";
            }

            public class SearchValidation
            {
                public const string Ok = "results";
                public const string ValidationError = "validationError";
            }

            public class Results
            {
                public const string HasError = "message";
                public const string Ok = "results";
                public const string ValidationError = "validationError";
                public const string ExactMatchFound = "exactMatchFound";
            }

            public class Details
            {
                public const string VacancyNotFound = "410";
                public const string VacancyHasError = "message";
                public const string Ok = "details";
            }
        }
    }
}