namespace SFA.Apprenticeships.Infrastructure.Repositories.IntegrationTests.Communication
{
    using System.Linq;
    using Domain.Entities.Communication;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using FizzWare.NBuilder;
    using FluentAssertions;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using NUnit.Framework;
    using Repositories.Communication.Entities;
    using StructureMap;

    [TestFixture]
    public class ExpiringDraftsRepositoryTests
    {
        private IConfigurationManager _configurationManager;
        private IExpiringDraftRepository _expiringDraftRepository;
        private MongoDatabase _database;
        private MongoCollection<MongoExpiringDraft> _collection;

        private const int _testVacancyId = -200;

        [TestFixtureSetUp]
        public void SetUp()
        {
            #pragma warning disable 0618
            _configurationManager = ObjectFactory.GetInstance<IConfigurationManager>();
            _expiringDraftRepository = ObjectFactory.GetInstance<IExpiringDraftRepository>();
            #pragma warning restore 0618

            var mongoConnectionString = _configurationManager.GetAppSetting("Communications.mongoDB");
            var mongoDbName = MongoUrl.Create(mongoConnectionString).DatabaseName;

            _database = new MongoClient(mongoConnectionString)
                .GetServer()
                .GetDatabase(mongoDbName);
            _collection = _database.GetCollection<MongoExpiringDraft>("expiringdrafts");
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            _collection.Remove(Query.EQ("VacancyId", _testVacancyId));
        }

        [Test, Category("Integration")]
        public void TestMultiUpsertAndGetCandidatesDailyDigest()
        {
            //Arrange
            var expiringDrafts =
                Builder<ExpiringDraft>.CreateListOfSize(3)
                    .All()
                    .With(ed => ed.VacancyId = _testVacancyId)
                    .With(ed => ed.IsSent = true)
                    .Build().ToList();

            //Act
            expiringDrafts.ForEach(ed =>
            {
                _expiringDraftRepository.Save(ed);
                ed.IsSent = false;
                _expiringDraftRepository.Save(ed);
            });

            
            //Assert
            var candidatesDailyDigest = _expiringDraftRepository.GetCandidatesDailyDigest();
            var returnedExpiringDrafts = candidatesDailyDigest.SelectMany(cand => cand.Value.ToArray());
            returnedExpiringDrafts.Count(ed => ed.VacancyId == _testVacancyId && !ed.IsSent)
                .Should()
                .Be(3);
        } 
    }
}
