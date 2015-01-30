namespace SFA.Apprenticeships.Application.UserAccount
{
    using System.Text;
    using System.Security.Cryptography;
    using Interfaces.Logging;
    using Interfaces.Users;

    public class RandomCodeGenerator : ICodeGenerator
    {
        private readonly ILogService _logger;
        public RandomCodeGenerator(ILogService logger)
        {
            _logger = logger;
        }

        public const int CodeLength = 6;

        // Vowels are omitted to avoid confusion with numbers (e.g. O and I) and to avoid profanities,
        // other letters and numbers are omitted to conform to the DEC Alphabet (http://en.wikipedia.org/wiki/Alphanumeric)
        // (i.e. I, O, Q, S, Z, 1, 0, 5, 3 and 2).
        public const string Alphanumerics = "46789BCDFGHJKLMNPRSTVWXY";

        public string Generate()
        {
            _logger.Debug("Generating new code.");
            var bytes = GenerateRandomBytes(CodeLength);
            var sb = new StringBuilder(CodeLength);

            for (var i = 0; i < CodeLength; i++)
            {
                var index = bytes[i] % Alphanumerics.Length;

                sb.Append(Alphanumerics[index]);
            }

            return sb.ToString();
        }

        private static byte[] GenerateRandomBytes(int length)
        {
            var rng = RandomNumberGenerator.Create();
            var bytes = new byte[length];

            rng.GetBytes(bytes);

            return bytes;
        }
    }
}
