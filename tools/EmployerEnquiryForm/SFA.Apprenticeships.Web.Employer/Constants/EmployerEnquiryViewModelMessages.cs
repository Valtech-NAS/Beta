namespace SFA.Apprenticeships.Web.Employer.Constants
{
    public static class EmployerEnquiryViewModelMessages
    {
        public static class FirstnameMessages
        {
            public const string LabelText = "First name";
            public const string RequiredErrorText = "Please enter first name";
            public const string TooLongErrorText = "First name mustn’t exceed 35 characters";
            public const string WhiteListRegularExpression = Whitelists.NameWhitelist.RegularExpression;
            public const string WhiteListErrorText = "First name " + Whitelists.NameWhitelist.ErrorText;
        }

        public static class LastnameMessages
        {
            public const string LabelText = "Last name";
            public const string RequiredErrorText = "Please enter last name";
            public const string TooLongErrorText = "Last name mustn’t exceed 35 characters";
            public const string WhiteListRegularExpression = Whitelists.NameWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Last name " + Whitelists.NameWhitelist.ErrorText;
        }

        public static class CompanynameMessages
        {
            public const string LabelText = "Company name";
            public const string RequiredErrorText = "Please enter your company name";
            public const string TooLongErrorText = "Company name mustn’t exceed 35 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Company name " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class PositionMessages
        {
            public const string LabelText = "Position at company";
            public const string RequiredErrorText = "Please enter position at company";
            public const string TooLongErrorText = "Position mustn’t exceed 35 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Position " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class EmailAddressMessages
        {
            public const string LabelText = "Email address";

            public const string HintText =
                "This'll be used for further communication to resolve your query. Please make sure you enter an valid email address";

            public const string RequiredErrorText = "Please enter email address";
            public const string TooLongErrorText = "Email address mustn’t exceed 100 characters";
            public const string WhiteListRegularExpression = Whitelists.EmailAddressWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Email address " + Whitelists.EmailAddressWhitelist.ErrorText;
        }

        public static class WorkPhoneNumberMessages
        {
            public const string LabelText = "Work phone number";
            public const string HintText = "If you don't have a landline number, enter your mobile number";
            public const string RequiredErrorText = "Please enter work phone number";
            public const string LengthErrorText = "Work Phone number must be between 8 and 16 digits";
            public const string WhiteListRegularExpression = Whitelists.PhoneNumberWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Work Phone number " + Whitelists.PhoneNumberWhitelist.ErrorText;
        }

        public static class MobileNumberMessages
        {
            public const string LabelText = "Mobile number";
            public const string LengthErrorText = "Mobile number must be between 8 and 16 digits";
            public const string WhiteListRegularExpression = Whitelists.PhoneNumberWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Mobile number " + Whitelists.PhoneNumberWhitelist.ErrorText;
        }

        public static class EmployeCountMessages
        {
            public const string LabelText = "Total number of employees";
            public const string HintText = "How many employees do you have?";
            public const string RequiredErrorText = "Please select total number of employees or if you don't know then please select don't know";
        }

        public static class WorkSectorMessages
        {
            public const string LabelText = "Industry sector";
            public const string HintText = "What industry sector do you operate in?";
            public const string RequiredErrorText = "Please select your company sector";
        }

        public static class PreviousExperienceTypeMessages
        {
            public const string LabelText = "Previous experience";
            public const string HintText = "Have you had any previous experience with Apprenticeships / Traineeships?";
            public const string RequiredErrorText = "Please select previous experience (Yes/No/Don't Know)";
        }

        public static class NameTitleMessages
        {
            public const string LabelText = "Title";
            public const string HintText = "Please select your name title";
            public const string RequiredErrorText = "Please select your name title";
        }

        public static class EnquirySourceMessages
        {
            public const string LabelText = "What has prompted you to make an enquiry?";
            public const string RequiredErrorText = "Please select what has prompted you to make an enquiry?";
        }

        public static class EnquiryDescriptionMessages
        {
            public const string LabelText = "Please tell us the nature of your query";
            public const string HintText = "Please tell us the nature of your query";
            public const string RequiredErrorText = "Please tell us the nature of your query";
            public const string TooLongErrorText = "Enquiry description mustn’t exceed 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Enquiry description " + Whitelists.FreetextWhitelist.ErrorText;
        }

    }
}