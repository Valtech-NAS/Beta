namespace SFA.Apprenticeships.Application.Registration
{
    using System.Text;
    using System.Security.Cryptography;
    using Interfaces.Users;

    public class CodeGenerator : ICodeGenerator
    {
        public const int CodeLength = 6;

        // Letters O and I are omitted to avoid confusion with numbers 0 and 1.
        public const string Alphanumerics = "0123456789ABCDEFGHJKLMNPQRSTUVWXYZ";

        public string Generate()
        {
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
