namespace SFA.Apprenticeships.Web.Candidate.Constants
{
    using System.IO;

    public static class Whitelists
    {
        public static class NameWhitelist
        {
            public const string RegularExpression = @"^[a-zA-Z()',+\-\s]+$";
            public const string ErrorText = "contains some invalid characters";
        }

        public static class EmailAddressWhitelist
        {
            //From https://www.owasp.org/index.php/OWASP_Validation_Regex_Repository
            public const string RegularExpression = @"^[a-zA-Z0-9+&*-]+(?:\.[a-zA-Z0-9_+&*-]+)*@(?:[a-zA-Z0-9-]+\.)+[a-zA-Z]{2,7}$";
            public const string ErrorText = @"must be a valid email address";
        }

        public static class FreetextWhitelist
        {
            public const string RegularExpression = @"^[a-zA-Z0-9?$@#()""'!,+\-=_:;.&€£*%\s]+$";
            public const string ErrorText = @"contains some invalid characters";
        }

        public static class YearWhitelist
        {
            public const string RegularExpression = @"^[0-9]{4}$";
            public const string ErrorText = @"must contain a 4 digit year";
        }

        public static class PhoneNumberWhitelist
        {
            public const string RegularExpression = @"^[0-9+\s-()]{8,16}$";
            public const string ErrorText = @"must only contain numbers";
        }

        public static class PasswordWhitelist
        {
            //Modified from https://www.owasp.org/index.php/OWASP_Validation_Regex_Repository
            //public const string RegularExpression = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[A-Z]).{8,127}$";
            //Modified to http://stackoverflow.com/questions/5859632/regular-expression-for-password-validation

            public const string RegularExpression = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,127}$";
            public const string ErrorText = @" must be at least 8 character with upper and lower case and have at least a number or a special character";
        }

        public static class PostcodeWhitelist
        {
            // See http://stackoverflow.com/questions/164979/uk-postcode-regex-comprehensive
            public const string RegularExpression =
                "^(([gG][iI][rR] {0,}0[aA]{2})|((([a-pr-uwyzA-PR-UWYZ][a-hk-yA-HK-Y]?[0-9][0-9]?)|(([a-pr-uwyzA-PR-UWYZ][0-9][a-hjkstuwA-HJKSTUW])|([a-pr-uwyzA-PR-UWYZ][a-hk-yA-HK-Y][0-9][abehmnprv-yABEHMNPRV-Y]))) {0,}[0-9][abd-hjlnp-uw-zABD-HJLNP-UW-Z]{2}))$"; //"^(GIR 0AA)|((([A-Z-[QVX]][0-9][0-9]?)|(([A-Z-[QVX]][A-Z-[IJZ]][0-9][0-9]?)|(([A-Z-[QVX]][0-9][A-HJKSTUW])|([A-Z-[QVX]][A-Z-[IJZ]][0-9][ABEHMNPRVWXY])))) [0-9][A-Z-[CIKMOV]]{2})$";
            public const string ErrorText = @" is not a valid format";
        }
    }
}