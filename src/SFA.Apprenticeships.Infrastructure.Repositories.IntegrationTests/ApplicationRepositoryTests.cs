namespace SFA.Apprenticeships.Infrastructure.Repositories.IntegrationTests
{
    using System;
    using Applications.IoC;
    using Common.IoC;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class ApplicationRepositoryTests
    {
        [SetUp]
        public void SetUp()
        {
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<ApplicationRepositoryRegistry>();
                x.AddRegistry<CommonRegistry>();
            });
        }

        [Test]
        public void ShouldCreateApplication()
        {
            // arrange
            var writer = ObjectFactory.GetInstance<IApplicationWriteRepository>();
            var application = CreateTestApplication();

            // act, assert
            var savedApplication = writer.Save(application);
        }

        #region Helpers
        private static ApplicationDetail CreateTestApplication()
        {
            return new ApplicationDetail
            {
                Id = Guid.NewGuid()
            };
        }

        #endregion
    }
}
