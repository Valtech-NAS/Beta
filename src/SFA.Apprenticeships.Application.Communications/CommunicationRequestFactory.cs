namespace SFA.Apprenticeships.Application.Communications
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Domain.Entities.Candidates;
    using Domain.Entities.Communication;
    using Interfaces.Communications;

    public class CommunicationRequestFactory
    {
        public static CommunicationRequest GetCommunicationMessage(Candidate candidate, List<ExpiringApprenticeshipApplicationDraft> candidateDailyDigest)
        {
            var communicationMessage = new CommunicationRequest
            {
                EntityId = candidate.EntityId,
                MessageType = MessageTypes.DailyDigest
            };

            var commTokens = new List<CommunicationToken>();

            commTokens.Add(new CommunicationToken(CommunicationTokens.CandidateEmailAddress, candidate.RegistrationDetails.EmailAddress));
            commTokens.Add(new CommunicationToken(CommunicationTokens.CandidateMobileNumber, candidate.RegistrationDetails.PhoneNumber));
            commTokens.Add(new CommunicationToken(CommunicationTokens.ExpiringDraftsCount, candidateDailyDigest.Count().ToString(CultureInfo.InvariantCulture)));

            var drafts = string.Join("~", candidateDailyDigest.Select(d => string.Join("|", d.Title, d.EmployerName, d.ClosingDate.ToLongDateString())));
            commTokens.Add(new CommunicationToken(CommunicationTokens.ExpiringDrafts, drafts));

            communicationMessage.Tokens = commTokens;
            return communicationMessage;
        } 
    }
}