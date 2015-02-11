namespace SFA.Apprenticeships.Application.Communications
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
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

            var commTokens = new List<CommunicationToken>
            {
                new CommunicationToken(CommunicationTokens.CandidateFirstName, candidate.RegistrationDetails.FirstName),
                new CommunicationToken(CommunicationTokens.CandidateEmailAddress, candidate.RegistrationDetails.EmailAddress),
                new CommunicationToken(CommunicationTokens.CandidateMobileNumber, candidate.RegistrationDetails.PhoneNumber),
                new CommunicationToken(CommunicationTokens.ExpiringDraftsCount, candidateDailyDigest.Count().ToString(CultureInfo.InvariantCulture))
            };

            var drafts = string.Join("~", candidateDailyDigest
                .OrderBy(p => p.ClosingDate)
                .Select(d => string.Join("|", WebUtility.UrlEncode(d.Title), WebUtility.UrlEncode(d.EmployerName), d.ClosingDate.ToLongDateString())));

            commTokens.Add(new CommunicationToken(CommunicationTokens.ExpiringDrafts, drafts));

            communicationMessage.Tokens = commTokens;

            return communicationMessage;
        } 
    }
}
