namespace SFA.Apprenticeships.Web.Candidate.Helpers
{
    using System.Web.Mvc;

    public static class PreviewExtensions
    {
        public static string GetMonthYearLabel(this HtmlHelper helper, int month, int year)
        {
            if (month == 0 || year == 0)
            {
                return string.Empty;
            }

            var monthLabel = GetMonthLabel(month);

            return string.Format("{0} {1}", monthLabel, year);
        }

        private static string GetMonthLabel(int month)
        {
            var name = string.Empty;

            switch (month)
            {
                case 1:
                    name = "Jan";
                    break;
                case 2:
                    name = "Feb";
                    break;
                case 3:
                    name = "Mar";
                    break;
                case 4:
                    name = "Apr";
                    break;
                case 5:
                    name = "May";
                    break;
                case 6:
                    name = "June";
                    break;
                case 7:
                    name = "July";
                    break;
                case 8:
                    name = "Aug";
                    break;
                case 9:
                    name = "Sept";
                    break;
                case 10:
                    name = "Oct";
                    break;
                case 11:
                    name = "Nov";
                    break;
                case 12:
                    name = "Dec";
                    break;
            }

            return name;
        }
    }
}