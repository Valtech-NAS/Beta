namespace SFA.Apprenticeships.Web.Employer.Extensions
{
    using System;
using System.Collections.Generic;
using System.Linq;

    public static class HelperExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }

        public static void ThrowIfNull<T>(this Object targetObject, string parameterName, string message)
                            where T : class
        {
            if (targetObject == null)
            {
                throw new ArgumentNullException(parameterName, message);
            }
        }

        public static void ThrowIfNull<T>(this T? targetObject, string parameterName, string message)
                                 where T : struct
        {
            if (targetObject == null)
            {
                throw new ArgumentNullException(parameterName, message);
            }
        }
    }
}