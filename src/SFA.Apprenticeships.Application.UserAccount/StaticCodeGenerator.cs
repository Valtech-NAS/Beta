namespace SFA.Apprenticeships.Application.UserAccount
{
    using Interfaces.Users;

    public class StaticCodeGenerator : ICodeGenerator
    {
        private const string DefaultAlphanumericCode = "ABC123";
        private const string DefaultNumericCode = "1234";

        public string GenerateAlphaNumeric(int length = 6)
        {
            // ignore length
            return DefaultAlphanumericCode;
        }

        public string GenerateNumeric(int length = 4)
        {
            // ignore length
            return DefaultNumericCode;
        }
    }
}
