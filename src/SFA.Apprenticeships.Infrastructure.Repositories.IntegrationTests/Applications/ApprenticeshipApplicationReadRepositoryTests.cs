namespace SFA.Apprenticeships.Infrastructure.Repositories.IntegrationTests.Applications
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using FizzWare.NBuilder;
    using FluentAssertions;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using NUnit.Framework;
    using Repositories.Applications.Entities;
    using StructureMap;

    [TestFixture]
    public class ApprenticeshipApplicationReadRepositoryTests
    {
        private IConfigurationManager _configurationManager;
        private IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private IApprenticeshipApplicationWriteRepository _apprenticeshipApplicationWriteRepository;
        private MongoDatabase _database;
        private MongoCollection<MongoApprenticeshipApplicationDetail> _collection;

        private const int TestVacancyId = -100;

        [TestFixtureSetUp]
        public void SetUp()
        {
            #pragma warning disable 0618
            _configurationManager = ObjectFactory.GetInstance<IConfigurationManager>();
            _apprenticeshipApplicationReadRepository = ObjectFactory.GetInstance<IApprenticeshipApplicationReadRepository>();
            _apprenticeshipApplicationWriteRepository = ObjectFactory.GetInstance<IApprenticeshipApplicationWriteRepository>();
            #pragma warning restore 0618

            var mongoConnectionString = _configurationManager.GetAppSetting("Applications.mongoDB");
            var mongoDbName = MongoUrl.Create(mongoConnectionString).DatabaseName;

            _database = new MongoClient(mongoConnectionString)
                .GetServer()
                .GetDatabase(mongoDbName);
            _collection = _database.GetCollection<MongoApprenticeshipApplicationDetail>("apprenticeships");
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            _collection.Remove(Query.EQ("Vacancy._id", TestVacancyId));
        }

        [Test, Category("Integration")]
        public void GetApprenticeshipApplicationsReturnsCorrectResults()
        {
            //Arrange - Build Applications
            var applications = BuildApprenticeshipApplicationDetails();
            applications.ForEach(a => _apprenticeshipApplicationWriteRepository.Save(a));

            //Act - Get application vacancy statuses
            var summaries = _apprenticeshipApplicationReadRepository
                .GetApplicationSummaries(TestVacancyId)
                .ToList();

            //Assert - the correct number of applicaitons with the correct vacancy state
            summaries.Count(v => v.VacancyStatus == VacancyStatuses.Unknown).Should().Be(10);
            summaries.Count(v => v.VacancyStatus == VacancyStatuses.Live).Should().Be(8);
            summaries.Count(v => v.VacancyStatus == VacancyStatuses.Unavailable).Should().Be(7);
            summaries.Count(v => v.VacancyStatus == VacancyStatuses.Expired).Should().Be(6);
        }

        public List<ApprenticeshipApplicationDetail> BuildApprenticeshipApplicationDetails()
        {
            var vacancyStatus =
                Builder<ApprenticeshipApplicationDetail>.CreateListOfSize(31)
                    .All()
                    .With(a => a.Vacancy.Id = TestVacancyId)
                    .TheFirst(10)
                    .With(a => a.VacancyStatus = VacancyStatuses.Unknown)
                    .TheNext(8)
                    .With(a => a.VacancyStatus = VacancyStatuses.Live)
                    .TheNext(7)
                    .With(a => a.VacancyStatus = VacancyStatuses.Unavailable)
                    .TheNext(6)
                    .With(a => a.VacancyStatus = VacancyStatuses.Expired)
                    .Build().ToList();

            return vacancyStatus.ToList();
        } 
    }
}
