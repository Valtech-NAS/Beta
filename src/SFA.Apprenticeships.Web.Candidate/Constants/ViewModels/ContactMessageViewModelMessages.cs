namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    using Common.Constants;

    public static class ContactMessageViewModelMessages
    {
        public static class FullNameMessages
        {
            public const string LabelText = "Full name";
            public const string RequiredErrorText = "Please enter full name";
            public const string TooLongErrorText = "Full name mustn’t exceed 71 characters";
            public const string WhiteListRegularExpression = Whitelists.NameWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Full name " + Whitelists.NameWhitelist.ErrorText;
        }

        public static class EmailAddressMessages
        {
            public const string LabelText = "Email address";
            public const string RequiredErrorText = "Please enter email address";
            public const string TooLongErrorText = "Email address mustn’t exceed 100 characters";
            public const string WhiteListRegularExpression = Whitelists.EmailAddressWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Email address " + Whitelists.EmailAddressWhitelist.ErrorText;
        }

        public class EnquiryMessages
        {
            public const string LabelText = "Briefly describe your question";
            public const string RequiredErrorText = "Please briefly describe your question";
            public const string TooLongErrorText = "Enquiry mustn’t exceed 100 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Description " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public class DetailsMessages
        {
            public const string LabelText = "More details about your question (optional)";
            public const string TooLongErrorText = "Your question mustn’t exceed 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Question " + Whitelists.FreetextWhitelist.ErrorText;
        }
    }
}
