namespace SFA.Apprenticeships.Domain.Entities.Location
{
    using System;
    using System.Text.RegularExpressions;

    public class LocationHelper
    {
        // adapted from http://regexlib.com/REDetails.aspx?regexp_id=2653
        const string PostcodeRegex = @"(?<O>(?<d>[BEGLMNS]|A[BL]|B[ABDHLNRST]|C[ABFHMORTVW]|D[ADEGHLNTY]|E[HNX]|F[KY]|G[LUY]|H[ADGPRSUX]|I[GMPV]|JE|K[ATWY]|L[ADELNSU]|M[EKL]|N[EGNPRW]|O[LX]|P[AEHLOR]|R[GHM]|S[AEGKL-PRSTWY]|T[ADFNQRSW]|UB|W[ADFNRSV]|YO|ZE)(?<a>\d\d?)|(?<d>E)(?<a>\dW)|(?<d>EC)(?<a>\d[AMNPRVY0])|(?<d>N)(?<a>\dP)|(?<d>NW)(?<a>\dW)|(?<d>SE)(?<a>\dP)|(?<d>SW)(?<a>\d[AEHPVWXY])|(?<d>W)(?<a>1[0-4A-DFGHJKSTUW])|(?<d>W)(?<a>[2-9])|(?<d>WC)(?<a>[12][ABEHNRVX]))(\ {0,1})(?<I>(?<s>\d)(?<u>[ABD-HJLNP-UW-Z]{2}))";

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
