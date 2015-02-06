namespace SFA.Apprenticeships.Application.Interfaces.Users
{
    using System;

    public interface ICodeGenerator
    {
        string GenerateAlphaNumeric(int length = 6);

        string GenerateNumeric(int length = 4);
    }
}
