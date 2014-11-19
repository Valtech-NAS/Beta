namespace SFA.Apprenticeships.Infrastructure.UserDirectory.Hash
{
    public interface IPasswordHash
    {
        string Generate(string userId, string password, string secretKey);

        bool Validate(string hash, string userId, string password, string secretKey);
    }
}