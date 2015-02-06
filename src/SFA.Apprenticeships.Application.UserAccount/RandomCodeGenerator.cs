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

        public const string Numerics = "123456789"; // note, no "0"

        // Vowels are omitted to avoid confusion with numbers (e.g. O and I) and to avoid profanities,
        // other letters and numbers are omitted to conform to the DEC Alphabet (http://en.wikipedia.org/wiki/Alphanumeric)
        // (i.e. I, O, Q, S, Z, 1, 0, 5, 3 and 2).
        public const string Alphanumerics = "46789BCDFGHJKLMNPRSTVWXY";

        public string GenerateAlphaNumeric(int length = 6)
        {
            _logger.Debug("Generating new alphanumeric code");

            return Generate(Alphanumerics, length);
        }

        public string GenerateNumeric(int length = 4)
        {
            _logger.Debug("Generating new numeric code");

            return Generate(Numerics, length);
        }

        #region Helpers
        private static string Generate(string characters, int length)
        {
            var bytes = GenerateRandomBytes(length);
            var sb = new StringBuilder(length);

            for (var i = 0; i < length; i++)
            {
                var index = bytes[i] % characters.Length;

                sb.Append(characters[index]);
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

        #endregion
    }
}
