namespace SFA.Apprenticeships.Web.Candidate.Helpers
{
    using System;

    public class AutoSaveDateTimeHelper
    {
        public static string GetDisplayDateTime(DateTime dateTime)
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
            var dataTimeByZoneId = TimeZoneInfo.ConvertTime(dateTime, timeZoneInfo);

            return dataTimeByZoneId.ToString("h:mm:ss tt") + " on " + dataTimeByZoneId.ToString("d/M/yyyy");
        }
    }
}