namespace SFA.Apprenticeships.Web.Candidate.Constants
{
    public static class Whitelists
    {
        public static class NameWhitelist
        {
            public const string RegularExpression = @"^[a-zA-Z0-9',+\-]+$";
            public const string ErrorText = "must only contain lower and upper case letters";
        }

        public static class FreetextWhitelist
        {
            public const string RegularExpression = @"^[a-zA-Z0-9?$@#()""'!,+\-=_:;.&€£*%\s]+$";
            public const string ErrorText = @"must only contain lower and upper case letters, spaces, tabs, digits or one of the following ?$@#()""'!,+\-=_:;.&€£*%";
        }
    }
}