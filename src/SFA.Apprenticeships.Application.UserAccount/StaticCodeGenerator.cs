namespace SFA.Apprenticeships.Application.UserAccount
{
    using Interfaces.Users;
    using NLog;

    public class StaticCodeGenerator : ICodeGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private const string DefaultCode = "ABC123";

        public string Generate()
        {
            Logger.Debug("Generating new default code.");
            return DefaultCode;
        }
    }
}