namespace SFA.Apprenticeships.Infrastructure.UserDirectory.Hash
{
    public class DummyPasswordHash : IPasswordHash
    {
        public string Generate(string userId, string password, string secretKey)
        {
            return userId + password;
        }

        public bool Validate(string hash, string userId, string password, string secretKey)
        {
            return userId + password == hash;
        }
    }
}