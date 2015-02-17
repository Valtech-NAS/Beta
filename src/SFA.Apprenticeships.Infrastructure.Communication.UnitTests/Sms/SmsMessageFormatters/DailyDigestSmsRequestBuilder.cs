namespace SFA.Apprenticeships.Infrastructure.Communication.UnitTests.Sms.SmsMessageFormatters
{
    using System;
    using System.Collections.Generic;
    using Application.Communications;
    using Application.Interfaces.Communications;
    using Domain.Entities.Communication;
    using Domain.Entities.UnitTests.Builder;

    public class DailyDigestSmsRequestBuilder
    {
        private List<ExpiringApprenticeshipApplicationDraft> _expiringDrafts;

        public DailyDigestSmsRequestBuilder()
        {
            _expiringDrafts = new List<ExpiringApprenticeshipApplicationDraft>();
        }

        public DailyDigestSmsRequestBuilder WithExpiringDrafts(List<ExpiringApprenticeshipApplicationDraft> expiringDrafts)
        {
            _expiringDrafts = expiringDrafts;
            return this;
        }

        public SmsRequest Build()
        {
            var candidateId = Guid.NewGuid();
            var candidate = new CandidateBuilder(candidateId).Build();
            var communicationRequest = CommunicationRequestFactory.GetCommunicationMessage(candidate, _expiringDrafts);
            var smsRequest = new SmsRequestBuilder().WithMessageType(MessageTypes.DailyDigest).WithTokens(communicationRequest.Tokens).Build();
            return smsRequest;
        } 
    }
}