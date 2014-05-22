namespace SFA.Apprenticeships.Web.Common.Tests.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using SFA.Apprenticeships.Common.Interfaces.Enums.ReferenceDataService;
    using SFA.Apprenticeships.Common.Interfaces.ReferenceData;
    using SFA.Apprenticeships.Web.Common.Providers;
    using Model = SFA.Apprenticeships.Common.Entities.ReferenceData;

    [TestFixture]
    public class LegacyReferenceDataProviderTests
    {
        private Mock<IReferenceDataService> _service;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _service = new Mock<IReferenceDataService>();

            _service.Setup(x => x.GetReferenceData(LegacyReferenceDataType.Framework)).Returns(new List<ILegacyReferenceData>{new Model.Framework{ Id = "Framework", Description = "Framework.1" }});
            _service.Setup(x => x.GetReferenceData(LegacyReferenceDataType.County)).Returns(new List<ILegacyReferenceData> { new Model.Framework { Id = "County", Description = "County.1" } });
            _service.Setup(x => x.GetReferenceData(LegacyReferenceDataType.ErrorCode)).Returns(new List<ILegacyReferenceData> { new Model.Framework { Id = "ErrorCode", Description = "ErrorCode.1" } });
            _service.Setup(x => x.GetReferenceData(LegacyReferenceDataType.LocalAuthority)).Returns(new List<ILegacyReferenceData> { new Model.Framework { Id = "LocalAuthority", Description = "LocalAuthority.1" } });
            _service.Setup(x => x.GetReferenceData(LegacyReferenceDataType.Region)).Returns(new List<ILegacyReferenceData> { new Model.Framework { Id = "Region", Description = "Region.1" } });           
        }

        [SetUp]
        public void TestSetup()
        {
            _service.ResetCalls();
        }

        [TestCase(LegacyReferenceDataType.County)]
        [TestCase(LegacyReferenceDataType.ErrorCode)]
        [TestCase(LegacyReferenceDataType.Framework)]
        [TestCase(LegacyReferenceDataType.LocalAuthority)]
        [TestCase(LegacyReferenceDataType.Region)]
        public void DoesGetReturnLegacyReferenceDataWithoutCache(LegacyReferenceDataType type)
        {
            var provider = new LegacyReferenceDataProvider(_service.Object);

            var viewModel = provider.Get(type);

            viewModel.Should().NotBeNull();
            viewModel.Count().Should().Be(1);
            viewModel.First(x => x.Id == type.ToString()).Description.Should().Be(string.Format("{0}.1", type));

            _service.Verify(x => x.GetReferenceData(type), Times.Once);
        }
    }
}
