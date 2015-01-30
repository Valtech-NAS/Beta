using SFA.Apprenticeships.Domain.Entities.Vacancies.Apprenticeships;

namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Builders
{
    using System;
    using System.Linq;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Repositories;
    using IoC;
    using StructureMap;

    public class ApprenticeshipApplicationBuilder
    {
        private readonly Guid _candidateId = new Guid("00000000-0000-0000-0000-000000000001");
        private int _vacancyId;
        private readonly string _emailAddress = string.Empty;
        private ApplicationStatuses _applicationStatus = ApplicationStatuses.Unknown;
        private DateTime? _dateApplied = DateTime.Now;
        private DateTime _expirationDate = DateTime.Now.AddDays(30);

        public ApprenticeshipApplicationBuilder(Guid candidateId, string emailAddress)
        {
            _candidateId = candidateId;
            _emailAddress = emailAddress;
        }

        public ApprenticeshipApplicationBuilder WithVacancyId(int vacancyId)
        {
            _vacancyId = vacancyId;
            return this;
        }

        public ApprenticeshipApplicationBuilder WithApplicationStatus(ApplicationStatuses applicationStatus)
        {
            _applicationStatus = applicationStatus;
            return this;
        }

        public ApprenticeshipApplicationBuilder WithExpirationDate(DateTime expirationDate)
        {
            _expirationDate = expirationDate;
            return this;
        }

        public ApprenticeshipApplicationBuilder WithoutDateApplied()
        {
            _dateApplied = null;
            return this;
        }

        public ApprenticeshipApplicationDetail ApprenticeshipApplicationDetail { get; private set; }

        public RegistrationBuilder RegistrationBuilder { get; private set; }

        public ApprenticeshipApplicationDetail Build()
        {
            RegistrationBuilder = new RegistrationBuilder(_emailAddress);

            ApprenticeshipApplicationDetail = new ApprenticeshipApplicationDetail
            {
                CandidateId = _candidateId,
                CandidateInformation = new ApplicationTemplate(),
                Status = _applicationStatus,
                DateApplied = _dateApplied,
                VacancyStatus = VacancyStatuses.Live,
                Vacancy = new ApprenticeshipSummary
                {
                    Id = _vacancyId,
                    Title = "Vacancy " + _vacancyId,
                    ClosingDate = _expirationDate
                }
            };

            var repo = WebTestRegistry.Container.GetInstance<IApprenticeshipApplicationWriteRepository>();

            ApprenticeshipApplicationDetail.CandidateDetails = RegistrationBuilder.Build();

            repo.Save(ApprenticeshipApplicationDetail);

            return ApprenticeshipApplicationDetail;
        }

        public void DeleteApprenticeshipApplications(Guid userCandidateId)
        {
            var writerepo = WebTestRegistry.Container.GetInstance<IApprenticeshipApplicationWriteRepository>();
            var readrepo = WebTestRegistry.Container.GetInstance<IApprenticeshipApplicationReadRepository>();

            var candidateApplications = readrepo.GetForCandidate(userCandidateId);
            candidateApplications.ToList().ForEach(a => writerepo.Delete(a.ApplicationId));
        }
    }
}
