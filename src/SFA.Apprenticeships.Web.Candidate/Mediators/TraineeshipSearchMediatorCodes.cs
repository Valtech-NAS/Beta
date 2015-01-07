namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    public partial class Codes
    {
        public class TraineeshipSearch
        {
            public class Index
            {
                public const string Ok = "search";
            }

            public class Results
            {
                public const string Ok = "details";
                public const string ValidationError = "validationError";
                public const string HasError = "message";
            }

            public class Details
            {
                public const string Ok = "details";
                public const string VacancyNotFound = "410";
                public const string VacancyHasError = "message"; // TODO: AG: MEDIATORS: why not match string variable name / namespace?
            }
        }
    }
}