namespace SFA.Apprenticeships.Web.Candidate.Constants
{
    using System.IO;

    public static class Whitelists
    {
        public static class NameWhitelist
        {
            public const string RegularExpression = @"^[a-zA-Z()',+\-\s]+$";
            public const string ErrorText = "must only contain lower and upper case letters";
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
            public const string ErrorText = @"must only contain lower and upper case letters, spaces, tabs, digits or one of the following ?$@#()""'!,+\-=_:;.&€£*%";
        }

        public static class YearWhitelist
        {
            public const string RegularExpression = @"^[0-9]{4}$";
            public const string ErrorText = @"must contain a 4 digit year";
        }

        public static class PhoneNumberWhitelist
        {
            public const string RegularExpression = @"^[0-9]{8,16}$";
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
            public const string RegularExpression = "(GIR 0AA)|((([A-Z-[QVX]][0-9][0-9]?)|(([A-Z-[QVX]][A-Z-[IJZ]][0-9][0-9]?)|(([A-Z-[QVX]][0-9][A-HJKSTUW])|([A-Z-[QVX]][A-Z-[IJZ]][0-9][ABEHMNPRVWXY])))) [0-9][A-Z-[CIKMOV]]{2})";
            public const string ErrorText = @" is not a valid format";
        }
    }
}