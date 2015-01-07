namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Builders
{
    using System;
    using System.Linq;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Vacancies.Traineeships;
    using Domain.Interfaces.Repositories;
    using StructureMap;

    public class TraineeshipApplicationBuilder
    {
        private readonly Guid _candidateId = new Guid("00000000-0000-0000-0000-000000000001");
        private int _vacancyId;
        private readonly string _emailAddress = string.Empty;

        private DateTime _closingDate = DateTime.Now.AddDays(30);

        public TraineeshipApplicationBuilder(Guid candidateId, string emailAddress)
        {
            _candidateId = candidateId;
            _emailAddress = emailAddress;
        }

        public TraineeshipApplicationBuilder WithVacancyId(int vacancyId)
        {
            _vacancyId = vacancyId;
            return this;
        }

        public TraineeshipApplicationBuilder WithClosingDate(DateTime closingDate)
        {
            _closingDate = closingDate;
            return this;
        }

        public TraineeshipApplicationDetail TraineeshipApplicationDetail { get; private set; }

        public RegistrationBuilder RegistrationBuilder { get; private set; }

        public TraineeshipApplicationDetail Build()
        {
            RegistrationBuilder = new RegistrationBuilder(_emailAddress);

            TraineeshipApplicationDetail = new TraineeshipApplicationDetail
            {
                CandidateId = _candidateId,
                CandidateInformation = new ApplicationTemplate(),
                DateApplied = DateTime.Now.AddDays(-1),
                Vacancy = new TraineeshipSummary
                {
                    Id = _vacancyId,
                    ClosingDate = _closingDate
                }
            };

#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            var repo = ObjectFactory.GetInstance<ITraineeshipApplicationWriteRepository>();
#pragma warning restore 0618

            TraineeshipApplicationDetail.CandidateDetails = RegistrationBuilder.Build();

            repo.Save(TraineeshipApplicationDetail);

            return TraineeshipApplicationDetail;
        }

        public void DeleteTraineeshipApplications(Guid userCandidateId)
        {
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            var writerepo = ObjectFactory.GetInstance<ITraineeshipApplicationWriteRepository>();
            var readrepo = ObjectFactory.GetInstance<ITraineeshipApplicationReadRepository>();
#pragma warning restore 0618

            var candidateApplications = readrepo.GetForCandidate(userCandidateId);
            candidateApplications.ToList().ForEach(a => writerepo.Delete(a.ApplicationId));
        }
    }
}
