namespace SFA.Apprenticeships.Web.Candidate.Extensions
{
    public static class CollectionExtensions
    {
        public static string ToQueryString(this string[] collection, string parameterName)
        {
            if (collection == null || collection.Length == 0) return string.Empty;

            var queryStringSeparator = string.Format("&{0}=", parameterName);

            var queryString = string.Join(queryStringSeparator, collection);

            return queryStringSeparator + queryString;
        }
    }
}