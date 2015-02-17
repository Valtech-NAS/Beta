namespace SFA.Apprenticeships.Infrastructure.Communication.UnitTests.Email.EmailMessageFormatters
{
    using System;
    using System.Collections.Generic;
    using Application.Communications;
    using Application.Interfaces.Communications;
    using Domain.Entities.Communication;
    using Domain.Entities.UnitTests.Builder;

    public class DailyDigestEmailRequestBuilder
    {
        private List<ExpiringApprenticeshipApplicationDraft> _expiringDrafts;

        public DailyDigestEmailRequestBuilder()
        {
            _expiringDrafts = new List<ExpiringApprenticeshipApplicationDraft>();
        }

        public DailyDigestEmailRequestBuilder WithExpiringDrafts(List<ExpiringApprenticeshipApplicationDraft> expiringDrafts)
        {
            _expiringDrafts = expiringDrafts;
            return this;
        }

        public EmailRequest Build()
        {
            var candidateId = Guid.NewGuid();
            var candidate = new CandidateBuilder(candidateId).Build();
            var communicationRequest = CommunicationRequestFactory.GetCommunicationMessage(candidate, _expiringDrafts);
            var emailRequest = new EmailRequestBuilder().WithMessageType(MessageTypes.DailyDigest).WithTokens(communicationRequest.Tokens).Build();
            return emailRequest;
        }
    }
}