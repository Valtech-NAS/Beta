namespace SFA.Apprenticeships.Web.Common.Constants
{
    using System.Globalization;

    public static class YearRegexRangeGenerator
    {
        public static string GetRegex(string year)
        {
            var d = int.Parse(year[2].ToString(CultureInfo.InvariantCulture));
            var u = int.Parse(year[3].ToString(CultureInfo.InvariantCulture));
            var nextD = d + 1;

            var regex = string.Format(@"19{0}[{1}-9]|19[{2}-9]\d|200\d", d, u, nextD);

            for (var i = 1; i <= d; i++)
            {
                if (i == d)
                {
                    regex += string.Format(@"|20{0}[1-{1}]", i, u);
                }
                else
                {
                    regex += string.Format(@"|20{0}[0-9]", i);
                }
            }

            return regex;
        }
    }
}