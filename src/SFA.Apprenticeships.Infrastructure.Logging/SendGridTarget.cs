namespace SFA.Apprenticeships.Infrastructure.Logging
{
    using System.Collections.Generic;
    using Application.Interfaces.Messaging;
    using Communication.IoC;
    using NLog;
    using NLog.Targets;
    using StructureMap;

    [Target("SendGridTarget")]
    public class SendGridTarget : TargetWithLayout
    {
        private ILoggerEmailDispatcher _loggerEmailDispatcher;

        public ILoggerEmailDispatcher LoggerEmailDispatcher
        {
            get
            {
                if (_loggerEmailDispatcher != null)
                {
                    return _loggerEmailDispatcher;
                }

                ObjectFactory.Initialize(x => x.AddRegistry<CommunicationRegistry>());

                _loggerEmailDispatcher = ObjectFactory.GetInstance<ILoggerEmailDispatcher>();

                return _loggerEmailDispatcher;
            }
        }

        protected override void Write(LogEventInfo logEvent)
        {
            LogMessageViaEmail(logEvent);
        }

        private void LogMessageViaEmail(LogEventInfo logEvent)
        {
            var request = new LogRequest
            {
                LogMessage = Layout.Render(logEvent),
                MessageType = CandidateMessageTypes.EmailLogger,
                Tokens = new[]
                {
                    new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.LogMessage,
                        Layout.Render(logEvent))
                }
            };

            LoggerEmailDispatcher.SendLogViaEmail(request);
        }
    }
}