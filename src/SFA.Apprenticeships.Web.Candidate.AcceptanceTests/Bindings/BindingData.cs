namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Bindings
{
    internal static class BindingData
    {
        public const string UserEmailAddressTokenName = "EmailToken";
        public const string UserEmailAddress = "nas.exemplar+acceptancetests@gmail.com";
        
        public const string PasswordTokenName = "PasswordToken";
        public const string Password = "?Password01!";

        public const string InvalidEmailTokenName = "InvalidEmailToken";
        public const string InvalidEmail = "invalid@gmail.com";

        public const string InvalidPasswordTokenName = "InvalidPasswordToken";
        public const string InvalidPassword = "InvalidPassword01!";

        public const string NewPassword = "?Password02!";
        public const string NewPasswordTokenName = "NewPasswordToken";

        public const string PasswordResetCodeTokenName = "PasswordResetCodeToken";

        public const string ActivationCodeTokenName = "ActivationToken";
        public const string ActivationCode = "ACTIV1";

        public const string AccountUnlockCodeTokenName = "AccountUnlockCodeToken";
        public const string AccountUnlockCode = "UNLCK1";

        public const string VacancyIdToken = "VacancyId";
        public const string VacancyReferenceToken = "VacancyReference";

        public const int ExistentVacancyId = 445650;
    }
}