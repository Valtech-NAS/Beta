namespace SFA.Apprenticeships.Web.Candidate.Helpers
{
    using System;
    using System.Web.Mvc;

    public static class PreviewExtensions
    {
        public static string GetMonthYearLabel(this HtmlHelper helper, int month, int year)
        {
            if ( year == 0 || month == 0 )
            {
                return "Current";
            }

            var date = new DateTime(year, month, 1);

            return date.ToString("MMM yyyy");
        }

        
    }
}