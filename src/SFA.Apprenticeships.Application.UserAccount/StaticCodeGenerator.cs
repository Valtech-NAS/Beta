namespace SFA.Apprenticeships.Application.UserAccount
{
    using Interfaces.Logging;
    using Interfaces.Users;

    public class StaticCodeGenerator : ICodeGenerator
    {
        private readonly ILogService _logger;
        public StaticCodeGenerator(ILogService logger)
        {
            _logger = logger;
        }

        private const string DefaultCode = "ABC123";

        public string Generate()
        {
            _logger.Debug("Generating new default code.");
            return DefaultCode;
        }
    }
}