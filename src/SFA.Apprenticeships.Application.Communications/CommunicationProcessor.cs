namespace SFA.Apprenticeships.Application.Communications
{
    using System;
    using System.Linq;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;

    public class CommunicationProcessor : ICommunicationProcessor
    {
        private readonly IExpiringApprenticeshipApplicationDraftRepository _expiringDraftRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IMessageBus _bus;

        public CommunicationProcessor(IExpiringApprenticeshipApplicationDraftRepository expiringDraftRepository, ICandidateReadRepository candidateReadRepository, IMessageBus bus)
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
                    var communicationMessage = CommunicationRequestFactory.GetCommunicationMessage(candidate, candidateDailyDigest.Value);
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
