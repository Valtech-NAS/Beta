namespace SFA.Apprenticeships.Application.Communication
{
    using System.Collections.Generic;
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

        public void SendDailyDigests()
        {
            var candidatesDailyDigest = _expiringDraftRepository.GetCandidatesDailyDigest();

            foreach (var candidateDailyDigest in candidatesDailyDigest)
            {
                var candidate = _candidateReadRepository.Get(candidateDailyDigest.Key);

                if (candidate.CommunicationPreferences.AllowEmail || candidate.CommunicationPreferences.AllowMobile)
                {
                    // TODO: Create and put communication object on queue with all details.

                    var communicationMessage = new CommunicationRequest()
                    {
                        EntityId = candidate.EntityId,
                        //TODO:
                        //MessageType = MessageTypes.DailyDigest
                    };

                    var commTokens = new List<KeyValuePair<CommunicationTokens, string>>();

                    foreach (var expiringDraft in candidateDailyDigest.Value)
                    {
                        //TODO: Repeating tokens needs re-worked
                    }

                    _bus.PublishMessage(communicationMessage);

                    // Update candidates expiring drafts to sent
                    candidateDailyDigest.Value.ToList().ForEach(dd =>
                    {
                        dd.IsSent = true;
                        _expiringDraftRepository.Save(dd);
                    });
                }
            }
        }
    }
}
