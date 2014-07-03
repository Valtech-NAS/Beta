namespace SFA.Apprenticeships.Application.Candidate
{
    using System;
    using CuttingEdge.Conditions;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;
    using Interfaces.Candidates;

    public class CandidateService : ICandidateService
    {
        private readonly IApplicationWriteRepository _applicationWriteRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ICandidateWriteRepository _candidateWriteRepository;
        //todo: private readonly IApplicationSubmissionQueue _applicationSubmissionQueue;

        public CandidateService(IApplicationWriteRepository applicationWriteRepository, ICandidateReadRepository candidateReadRepository, ICandidateWriteRepository candidateWriteRepository)
        {
            _applicationWriteRepository = applicationWriteRepository;
            _candidateReadRepository = candidateReadRepository;
            _candidateWriteRepository = candidateWriteRepository;
            //todo: _applicationSubmissionQueue = applicationSubmissionQueue
        }

        public Candidate GetCandidate(int candidateId)
        {
            return _candidateReadRepository.Get(candidateId);
        }

        public Candidate SaveCandidate(Candidate candidate)
        {
            Condition.Requires(candidate);

            return _candidateWriteRepository.Save(candidate);
        }

        public ApplicationDetail SaveApplication(ApplicationDetail application)
        {
            Condition.Requires(application);

            return _applicationWriteRepository.Save(application);
        }

        public void SubmitApplication(ApplicationDetail application)
        {
            Condition.Requires(application);

            //todo: _applicationSubmissionQueue.Queue(application);
            throw new NotImplementedException();
        }
    }
}
