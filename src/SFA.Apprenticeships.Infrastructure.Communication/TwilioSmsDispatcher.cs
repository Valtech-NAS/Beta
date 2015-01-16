namespace SFA.Apprenticeships.Infrastructure.Communication
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using Application.Interfaces.Messaging;
    using Domain.Entities.Exceptions;
    using NLog;
    using Sms;
    using Twilio;
    using ErrorCodes = Application.Interfaces.Messaging.ErrorCodes;

    public class TwilioSmsDispatcher : ISmsDispatcher
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _mobileNumberFrom;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IEnumerable<KeyValuePair<MessageTypes, SmsMessageGenerator>> _messageGenerators;

        public TwilioSmsDispatcher(TwilioConfiguration twilioConfiguration, IEnumerable<KeyValuePair<MessageTypes, SmsMessageGenerator>> messageGenerators)
        {
            _accountSid = twilioConfiguration.AccountSid;
            _authToken = twilioConfiguration.AuthToken;
            _mobileNumberFrom = twilioConfiguration.MobileNumberFrom;
            _messageGenerators = messageGenerators;
        }

        public void SendSms(SmsRequest request)
        {
            try
            {
                var twilio = new TwilioRestClient(_accountSid, _authToken);

                var message = GetMessageFrom(request);

                Logger.Debug("Dispatching sms: {0}", LogTwilioMessage(request));
                var response = twilio.SendMessage(_mobileNumberFrom, request.ToNumber, message);
                if (response.RestException != null)
                {
                    Logger.Error("Failed to dispatch sms: {0}", response.RestException.Message);
                    throw new CustomException(GetExceptionMessage(response.RestException), ErrorCodes.SmsError);
                }
                Logger.Info("Dispatched sms: {0} to {1}", message, request.ToNumber);
            }
            catch (Exception e)
            {
                Logger.Error("Failed to dispatch sms", e);
                throw new CustomException("Failed to dispatch sms", e, ErrorCodes.SmsError);
            }
        }

        private static string GetExceptionMessage(RestException restException)
        {
            return string.Format("Failed to dispatch sms. Code:{0}, Message:{1}, MoreInfo: {2}, Status:{3}",
                restException.Code, restException.Message, restException.MoreInfo, restException.Status);
        }

        private static string LogTwilioMessage(SmsRequest request)
        {
            return string.Format("To: {0}", request.ToNumber);
        }

        private string GetMessageFrom(SmsRequest request)
        {
            return _messageGenerators.First(m => m.Key == request.MessageType).Value.GetMessage(request.Tokens);
        }
    }

    public abstract class SmsMessageGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly TwilioConfiguration _configuration;
        protected string Message;

        protected SmsMessageGenerator(TwilioConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected TwilioTemplateConfiguration GetTemplateConfiguration(string templateName)
        {
            var template = _configuration.Templates.FirstOrDefault(each => each.Name == templateName);

            if (template != null)
            {
                return template;
            }

            var errorMessage = string.Format("GetTemplateConfiguration : Invalid SMS template name: {0}",
                templateName);
            Logger.Error(errorMessage);

            throw new ConfigurationErrorsException(errorMessage);
        }
        
        public abstract string GetMessage(IEnumerable<KeyValuePair<CommunicationTokens, string>> communicationTokens);
    }

    public class SmsActivationCodeMessageGenerator : SmsMessageGenerator
    {
        public SmsActivationCodeMessageGenerator(TwilioConfiguration configuration) : base(configuration)
        {
            Message = GetTemplateConfiguration("MessageTypes.SendActivationCode").Message;
        }

        public override string GetMessage(IEnumerable<KeyValuePair<CommunicationTokens, string>> communicationTokens)
        {
            return string.Format(Message, communicationTokens.First(ct => ct.Key == CommunicationTokens.ActivationCode).Value);
        }
    }

    public class SmsAccountUnlockCodeMessageGenerator : SmsMessageGenerator
    {
        public SmsAccountUnlockCodeMessageGenerator(TwilioConfiguration configuration) : base(configuration)
        {
            Message = GetTemplateConfiguration("MessageTypes.SendAccountUnlockCode").Message;
        }

        public override string GetMessage(IEnumerable<KeyValuePair<CommunicationTokens, string>> communicationTokens)
        {
            return string.Format(Message, communicationTokens.First(ct => ct.Key == CommunicationTokens.AccountUnlockCode).Value);
        }
    }

    public class SmsPasswordResetCodeMessageGenerator : SmsMessageGenerator
    {
        public SmsPasswordResetCodeMessageGenerator(TwilioConfiguration configuration) : base(configuration)
        {
            Message = GetTemplateConfiguration("MessageTypes.SendPasswordResetCode").Message;
        }

        public override string GetMessage(IEnumerable<KeyValuePair<CommunicationTokens, string>> communicationTokens)
        {
            return string.Format(Message, communicationTokens.First(ct => ct.Key == CommunicationTokens.PasswordResetCode).Value);
        }
    }

    public class SmsPasswordChangedMessageGenerator : SmsMessageGenerator
    {
        public SmsPasswordChangedMessageGenerator(TwilioConfiguration configuration) : base(configuration)
        {
            Message = GetTemplateConfiguration("MessageTypes.PasswordChanged").Message;
        }

        public override string GetMessage(IEnumerable<KeyValuePair<CommunicationTokens, string>> communicationTokens)
        {
            return Message;
        }
    }

    public class SmsApprenticeshipApplicationSubmittedMessageGenerator : SmsMessageGenerator
    {
        public SmsApprenticeshipApplicationSubmittedMessageGenerator(TwilioConfiguration configuration) : base(configuration)
        {
            Message = GetTemplateConfiguration("MessageTypes.ApprenticeshipApplicationSubmitted").Message;
        }

        public override string GetMessage(IEnumerable<KeyValuePair<CommunicationTokens, string>> communicationTokens)
        {
            return string.Format(Message, communicationTokens.First(ct => ct.Key == CommunicationTokens.ApplicationVacancyReference).Value);
        }
    }

    public class SmsTraineeshipApplicationSubmittedMessageGenerator : SmsMessageGenerator
    {
        public SmsTraineeshipApplicationSubmittedMessageGenerator(TwilioConfiguration configuration) : base(configuration)
        {
            Message = GetTemplateConfiguration("MessageTypes.TraineeshipApplicationSubmitted").Message;
        }

        public override string GetMessage(IEnumerable<KeyValuePair<CommunicationTokens, string>> communicationTokens)
        {
            return string.Format(Message, communicationTokens.First(ct => ct.Key == CommunicationTokens.ApplicationVacancyReference).Value);
        }
    }
}
