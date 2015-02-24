namespace SFA.Apprenticeships.Web.Common.Framework
{
    using System;

    public static class DateDisplayExtensions
    {
        public static string ToFriendlyClosingWeek(this DateTime closingDate)
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
            var dateTimeByZoneId = TimeZoneInfo.ConvertTime(closingDate.ToUniversalTime(), timeZoneInfo);

            var daysLeft = (int)(dateTimeByZoneId - DateTime.Now.Date).TotalDays;

            if (daysLeft > 7 || daysLeft < 0)
            {
                return dateTimeByZoneId.ToString("dd MMM yyyy");
            }

            switch (daysLeft)
            {
                case 0:
                    return "today";
                case 1:
                    return "tomorrow";
                default:
                    return "in " + daysLeft + " days";
            }
        }

        public static string ToFriendlyClosingToday(this DateTime closingDate)
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
            var dateTimeByZoneId = TimeZoneInfo.ConvertTime(closingDate.ToUniversalTime(), timeZoneInfo);

            var daysLeft = (int) (dateTimeByZoneId - DateTime.Now.Date).TotalDays;

            switch (daysLeft)
            {
                case 0:
                    return "today";
                default:
                    return dateTimeByZoneId.ToString("dd MMM yyyy");
            }
        }
    }
}
