namespace SFA.Apprenticeships.Web.Employer.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    public static class Extensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }
    }
}