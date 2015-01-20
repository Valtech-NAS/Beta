namespace SFA.Apprenticeships.Application.Communications
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Interfaces.Messaging;

    public class CommunicationProcessor : ICommunicationProcessor
    {
        private readonly IExpiringDraftRepository _expiringDraftRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IMessageBus _bus;

        public CommunicationProcessor(IExpiringDraftRepository expiringDraftRepository, ICandidateReadRepository candidateReadRepository, IMessageBus bus)
        {
            _expiringDraftRepository = expiringDraftRepository;
            _candidateReadRepository = candidateReadRepository;
            _bus = bus;
        }

        public void SendDailyDigests(Guid batchId)
        {
            var candidatesDailyDigest = _expiringDraftRepository.GetCandidatesDailyDigest();

            foreach (var candidateDailyDigest in candidatesDailyDigest)
            {
                var candidate = _candidateReadRepository.Get(candidateDailyDigest.Key);

                if (candidate.CommunicationPreferences.AllowEmail || candidate.CommunicationPreferences.AllowMobile)
                {
                    var communicationMessage = new CommunicationRequest
                    {
                        EntityId = candidate.EntityId,
                        MessageType = MessageTypes.DailyDigest
                    };

                    var commTokens = new List<KeyValuePair<CommunicationTokens, string>>();
                    int counter = 1;

                    commTokens.Add(new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.CandidateEmailAddress, candidate.RegistrationDetails.EmailAddress));
                    commTokens.Add(new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.CandidateMobileNumber, candidate.RegistrationDetails.PhoneNumber));
                    commTokens.Add(new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.TotalItems, candidateDailyDigest.Value.Count().ToString(CultureInfo.InvariantCulture)));

                    foreach (var draft in candidateDailyDigest.Value)
                    {
                        if (counter <= 10)
                        {
                            var pipeDelimitedDraftValues = string.Join("|",
                                new[] {draft.Title, draft.EmployerName, draft.ClosingDate.ToLongDateString()});
                            var token =
                                new KeyValuePair<CommunicationTokens, string>(
                                    (CommunicationTokens) Enum.Parse(typeof (CommunicationTokens), "Item" + counter++),
                                    pipeDelimitedDraftValues);
                            commTokens.Add(token);
                        }
                    }

                    communicationMessage.Tokens = commTokens;
                    _bus.PublishMessage(communicationMessage);

                    // Update candidates expiring drafts to sent
                    candidateDailyDigest.Value.ToList().ForEach(dd =>
                    {
                        dd.BatchId = batchId;
                        _expiringDraftRepository.Save(dd);
                    });
                }
                else
                {
                    // Delete candidates expiring drafts
                    candidateDailyDigest.Value.ToList().ForEach(_expiringDraftRepository.Delete);
                }
            }
        }
    }
}
