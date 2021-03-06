﻿namespace SFA.Apprenticeships.Infrastructure.Communication.Sms
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using Application.Interfaces.Communications;
    using Application.Interfaces.Logging;
    using Domain.Entities.Exceptions;
    using Twilio;
    using ErrorCodes = Application.Interfaces.Communications.ErrorCodes;

    public class TwilioSmsDispatcher : ISmsDispatcher
    {
        private readonly ILogService _logger;

        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _mobileNumberFrom;

        private readonly IEnumerable<KeyValuePair<MessageTypes, SmsMessageFormatter>> _messageFormatters;

        public TwilioSmsDispatcher(ITwillioConfiguration twilioConfiguration, IEnumerable<KeyValuePair<MessageTypes, SmsMessageFormatter>> messageFormatters, ILogService logger)
        {
            _accountSid = twilioConfiguration.AccountSid;
            _authToken = twilioConfiguration.AuthToken;
            _mobileNumberFrom = twilioConfiguration.MobileNumberFrom;
            _messageFormatters = messageFormatters;
            _logger = logger;
        }

        public void SendSms(SmsRequest request)
        {
            try
            {
                var twilio = new TwilioRestClient(_accountSid, _authToken);

                var message = GetMessageFrom(request);

                _logger.Debug("Dispatching sms: {0}", LogTwilioMessage(request));
                var response = twilio.SendMessage(_mobileNumberFrom, request.ToNumber, message);
                if (response.RestException != null)
                {
                    _logger.Error("Failed to dispatch sms: {0}", response.RestException.Message);
                    throw new CustomException(GetExceptionMessage(response.RestException), ErrorCodes.SmsError);
                }
                _logger.Info("Dispatched sms: {0} to {1}", message, request.ToNumber);
            }
            catch (Exception e)
            {
                _logger.Error("Failed to dispatch sms", e);
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
            if (!_messageFormatters.Any(mf => mf.Key == request.MessageType))
            {
                var errorMessage = string.Format("GetMessageFrom: No message formatter exists for MessageType name: {0}", request.MessageType);
                _logger.Error(errorMessage);

                throw new ConfigurationErrorsException(errorMessage);
            }

            return _messageFormatters.First(m => m.Key == request.MessageType).Value.GetMessage(request.Tokens);
        }
    }
}
