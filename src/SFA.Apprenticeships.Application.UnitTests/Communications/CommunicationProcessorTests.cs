namespace SFA.Apprenticeships.Application.UnitTests.Communications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Communications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Communication;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using FizzWare.NBuilder;
    using Interfaces.Messaging;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class CommunicationProcessorTests
    {
        private Mock<IExpiringDraftRepository> _expiringDraftRepository;
        private Mock<ICandidateReadRepository> _candidateReadRepository;
        private Mock<IMessageBus> _bus;
        private CommunicationProcessor _communicationProcessor;

        [SetUp]
        public void SetUp()
        {
            _expiringDraftRepository = new Mock<IExpiringDraftRepository>();
            _candidateReadRepository = new Mock<ICandidateReadRepository>();
            _bus = new Mock<IMessageBus>();
            _communicationProcessor = new CommunicationProcessor(_expiringDraftRepository.Object, _candidateReadRepository.Object, _bus.Object);
        }

        [Test]
        public void AllowEmailsFalseShouldDeleteDrafts()
        {
            _expiringDraftRepository.Setup(x => x.GetCandidatesDailyDigest()).Returns(GetDraftDigests(2, 2));
            _expiringDraftRepository.Setup(x => x.Delete(It.IsAny<ExpiringDraft>()));
            _candidateReadRepository.Setup(x => x.Get(It.IsAny<Guid>())).Returns(GetCandidate(false));

            var batchId = Guid.NewGuid();
            _communicationProcessor.SendDailyDigests(batchId);

            _expiringDraftRepository.Verify(x => x.GetCandidatesDailyDigest(), Times.Once);
            _candidateReadRepository.Verify(x => x.Get(It.IsAny<Guid>()), Times.Exactly(2));
            _expiringDraftRepository.Verify(x => x.Delete(It.IsAny<ExpiringDraft>()), Times.Exactly(4));
        }

        [Test]
        public void AllowEmailsTrueShouldSendMessageAndUpdate()
        {
            _expiringDraftRepository.Setup(x => x.GetCandidatesDailyDigest()).Returns(GetDraftDigests(2, 2));
            _expiringDraftRepository.Setup(x => x.Delete(It.IsAny<ExpiringDraft>()));
            _expiringDraftRepository.Setup(x => x.Save(It.IsAny<ExpiringDraft>()));
            _candidateReadRepository.Setup(x => x.Get(It.IsAny<Guid>())).Returns(GetCandidate(true));
            _bus.Setup(x => x.PublishMessage(It.IsAny<CommunicationRequest>()));

            var batchId = Guid.NewGuid();
            _communicationProcessor.SendDailyDigests(batchId);

            _expiringDraftRepository.Verify(x => x.GetCandidatesDailyDigest(), Times.Once);
            _candidateReadRepository.Verify(x => x.Get(It.IsAny<Guid>()), Times.Exactly(2));
            _expiringDraftRepository.Verify(x => x.Delete(It.IsAny<ExpiringDraft>()), Times.Never);
            _expiringDraftRepository.Verify(x => x.Save(It.Is<ExpiringDraft>(ed => ed.BatchId == batchId)), Times.Exactly(4));
            _bus.Verify(x => x.PublishMessage(It.IsAny<CommunicationRequest>()), Times.Exactly(2));
        }

        private Dictionary<Guid, List<ExpiringDraft>> GetDraftDigests(int noOfcandidates, int noOfDrafts)
        {
            var digest = new Dictionary<Guid, List<ExpiringDraft>>();

            for (int i = 0; i < noOfcandidates; i++)
            {
                var candidateId = Guid.NewGuid();
                var drafts =
                    Builder<ExpiringDraft>.CreateListOfSize(noOfDrafts)
                        .All()
                        .With(ed => ed.CandidateId == candidateId)
                        .Build()
                        .ToList();
                digest.Add(candidateId, drafts);
            }

            return digest;
        }

        private Candidate GetCandidate(bool allowEmails)
        {
            var candidate = new Candidate
            {
                CommunicationPreferences =
                {
                    AllowEmail = allowEmails
                }
            };

            return candidate;
        }
    }
}
