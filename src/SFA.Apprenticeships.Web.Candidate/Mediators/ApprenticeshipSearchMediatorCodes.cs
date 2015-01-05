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

            public class Details
            {
                public const string VacancyNotFound = "410";
                public const string VacancyHasError = "message";
                public const string Ok = "details";
            }
        }
    }
}