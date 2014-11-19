namespace SFA.Apprenticeships.Infrastructure.UserDirectory.Hash
{
    public class DummyPasswordHash : IPasswordHash
    {
        public string Generate(string userId, string password, string secretKey)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, userId + password + secretKey);
        }

        public bool Validate(string hash, string userId, string password, string secretKey)
        {
            return hash == Generate(userId, password, secretKey);
        }
    }
}