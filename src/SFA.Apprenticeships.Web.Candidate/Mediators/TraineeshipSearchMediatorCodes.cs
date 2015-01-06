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

            public class Details
            {
                public const string VacancyNotFound = "410";
                public const string VacancyHasError = "message"; // TODO: AG: MEDIATORS: why not match string variable name?
                public const string Ok = "details";
            }
        }
    }
}