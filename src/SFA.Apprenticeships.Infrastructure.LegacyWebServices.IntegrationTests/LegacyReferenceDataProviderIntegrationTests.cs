namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.IntegrationTests
{
    using System.Linq;
    using Application.Interfaces.ReferenceData;
    using Common.IoC;
    using Domain.Entities.ReferenceData;
    using FluentAssertions;
    using IoC;
    using NUnit.Framework;
    using ReferenceData;
    using StructureMap;

    [TestFixture]
    public class LegacyReferenceDataProviderIntegrationTests
    {
        private IReferenceDataProvider _provider;

        [TestFixtureSetUp]
        public void SetUp()
        {
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LegacyWebServicesRegistry>();
            });

            _provider = ObjectFactory.GetNamedInstance<IReferenceDataProvider>("LegacyReferenceDataProvider");
        }

        [TestCase("Counties", 46)]
        public void GetApprenticeshipFrameworksShouldReturnList(string referenceDataType, int numberReturned)
        {
            var test = _provider.GetReferenceData(referenceDataType);
            var items = test as ReferenceDataItem[] ?? test.ToArray();
            items.Should().NotBeNullOrEmpty();
            items.Count().Should().Be(numberReturned);
        }
    }
}