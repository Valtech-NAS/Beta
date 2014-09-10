namespace SFA.Apprenticeships.Web.Candidate.Helpers
{
    using System;
    using System.Web.Mvc;

    public static class PreviewExtensions
    {
        public static string GetMonthYearLabel(this HtmlHelper helper, int month, string year)
        {
            if (string.IsNullOrEmpty(year))
            {
                return "Current";
            }

            int intYear;
            int.TryParse(year, out intYear);

            if (intYear == DateTime.MinValue.Year)
            {
                return "Current";
            }

            var date = new DateTime(intYear, month, 1);

            return date.ToString("MMM yyyy");
        }

        public static string GetDisplayGrade(this HtmlHelper helper, string grade, bool isPredicted)
        {
            return isPredicted ? string.Format("{0}(Predicted)", grade) : grade;
        }
    }
}