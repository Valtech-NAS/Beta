namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Builders
{
    using System;
    using System.Linq;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Vacancies;
    using Domain.Entities.Vacancies.Traineeships;
    using Domain.Interfaces.Repositories;
    using IoC;
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
                VacancyStatus = VacancyStatuses.Live,
                Vacancy = new TraineeshipSummary
                {
                    Id = _vacancyId,
                    ClosingDate = _closingDate
                }
            };

            var repo = WebTestRegistry.Container.GetInstance<ITraineeshipApplicationWriteRepository>();

            TraineeshipApplicationDetail.CandidateDetails = RegistrationBuilder.Build();

            repo.Save(TraineeshipApplicationDetail);

            return TraineeshipApplicationDetail;
        }

        public void DeleteTraineeshipApplications(Guid userCandidateId)
        {
            var writerepo = WebTestRegistry.Container.GetInstance<ITraineeshipApplicationWriteRepository>();
            var readrepo = WebTestRegistry.Container.GetInstance<ITraineeshipApplicationReadRepository>();

            var candidateApplications = readrepo.GetForCandidate(userCandidateId);
            candidateApplications.ToList().ForEach(a => writerepo.Delete(a.ApplicationId));
        }
    }
}
