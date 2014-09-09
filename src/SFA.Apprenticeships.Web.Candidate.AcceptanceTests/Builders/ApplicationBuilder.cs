namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Builders
{
    using System;
    using System.Linq;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;
    using StructureMap;

    public class ApplicationBuilder
    {
        private readonly Guid _candidateId = new Guid("00000000-0000-0000-0000-000000000001");
        private readonly string _emailAddress = string.Empty;
        private ApplicationStatuses _applicationStatus = ApplicationStatuses.Unknown;

        public ApplicationBuilder(Guid candidateId, string emailAddress)
        {
            _candidateId = candidateId;
            _emailAddress = emailAddress;
        }

        public ApplicationBuilder WithApplicationStatus(ApplicationStatuses applicationStatus)
        {
            _applicationStatus = applicationStatus;
            return this;
        }

        public ApplicationDetail ApplicationDetail { get; private set; }

        public RegistrationBuilder RegistrationBuilder { get; private set; }

        public ApplicationDetail Build()
        {
            RegistrationBuilder = new RegistrationBuilder(_emailAddress);

            ApplicationDetail = new ApplicationDetail
            {
                CandidateId = _candidateId,
                CandidateInformation = new ApplicationTemplate(),
                Status = _applicationStatus
            };

            var repo = ObjectFactory.GetInstance<IApplicationWriteRepository>();

            ApplicationDetail.CandidateDetails = RegistrationBuilder.Build();

            repo.Save(ApplicationDetail);

            return ApplicationDetail;
        }

        public void DeleteApplications(Guid userCandidateId)
        {
            var writerepo = ObjectFactory.GetInstance<IApplicationWriteRepository>();
            var readrepo = ObjectFactory.GetInstance<IApplicationReadRepository>();
            var candidateApplications = readrepo.GetForCandidate(UserBuilder.UserAndCandidateId);
            candidateApplications.ToList().ForEach(a => writerepo.Delete(a.ApplicationId));
        }

        public void DeleteApplications(string useremailAddress)
        {
            var writerepo = ObjectFactory.GetInstance<IApplicationWriteRepository>();
            var readrepo = ObjectFactory.GetInstance<IApplicationReadRepository>();
            var candidateApplication = readrepo.Get(ad => ad.CandidateDetails.EmailAddress == useremailAddress);

            while (candidateApplication != null)
            {
                writerepo.Delete(candidateApplication.EntityId);
                candidateApplication = readrepo.Get(ad => ad.CandidateDetails.EmailAddress == useremailAddress);
            }
            
        }
    }
}
