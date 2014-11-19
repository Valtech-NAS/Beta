namespace SFA.Apprenticeships.Infrastructure.UserDirectory.Hash
{
    public class PasswordHash : IPasswordHash
    {
        public string Generate(string userId, string password, string secretKey)
        {
            return BCrypt.Net.BCrypt.HashPassword(userId + password + secretKey);
        }

        public bool Validate(string hash, string userId, string password, string secretKey)
        {
            return BCrypt.Net.BCrypt.Verify(userId + password + secretKey, hash);
        }
    }
}