namespace SFA.Apprenticeships.Domain.Entities.Locations
{
    using System.Text.RegularExpressions;

    public static class LocationHelper
    {
        // adapted from http://stackoverflow.com/a/164994/1882637
        const string PostcodeRegex = @"(GIR 0AA)|((([A-Z-[QVX]][0-9][0-9]?)|(([A-Z-[QVX]][A-Z-[IJZ]][0-9][0-9]?)|(([A-Z-[QVX‌​]][0-9][A-HJKSTUW])|([A-Z-[QVX]][A-Z-[IJZ]][0-9][ABEHMNPRVWXY]))))\s?[0-9][A-Z-[C‌​IKMOV]]{2})";

        const string PartialPostcodeRegex = @"((([A-Z-[QVX]][0-9][0-9]?)|(([A-Z-[QVX]][A-Z-[IJZ]][0-9][0-9]?)|(([A-Z-[QVX]][0-9][A-HJKSTUW])|([A-Z-[QVX]][A-Z-[IJZ]][0-9][ABEHMNPRVWXY])))))";

        public static bool IsPartialPostcode(string postcode)
        {
            return CheckPostcode(postcode, PartialPostcodeRegex);
        }

        public static bool IsPostcode(string postcode)
        {
            return CheckPostcode(postcode, PostcodeRegex);
        }

        #region Helpers
        private static bool CheckPostcode(string postcode, string regex)
        {
            if (string.IsNullOrWhiteSpace(postcode)) return false;

            var formattedPostcode = postcode.Trim().ToUpperInvariant();

            return Regex.IsMatch(formattedPostcode, regex);
        }
        #endregion
    }
}
