namespace SFA.Apprenticeships.Web.Common.Tests.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using SFA.Apprenticeships.Common.Caching;

    using SFA.Apprenticeships.Web.Common.Models.Common;
    using SFA.Apprenticeships.Web.Common.Providers;
    using Model = SFA.Apprenticeships.Common.Entities.ReferenceData;
    using SFA.Apprenticeships.Common.Interfaces.Enums.ReferenceDataService;
    using SFA.Apprenticeships.Common.Interfaces.ReferenceData;

    [TestFixture]
    public class LegacyReferenceDataProviderTests
    {
        private Mock<IReferenceDataService> _service;
        private Mock<ICacheClient> _cache;
        private IList<ReferenceDataViewModel> _frameworks;
        private IList<ReferenceDataViewModel> _counties;
        private IList<ReferenceDataViewModel> _errorCodes;
        private IList<ReferenceDataViewModel> _localAuthorities;
        private IList<ReferenceDataViewModel> _regions;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _service = new Mock<IReferenceDataService>();

            _frameworks = new List<ReferenceDataViewModel> { new ReferenceDataViewModel { Id = "Framework", Description = "Framework.1" } };
            _counties = new List<ReferenceDataViewModel> { new ReferenceDataViewModel { Id = "County", Description = "County.1" } };
            _errorCodes = new List<ReferenceDataViewModel> { new ReferenceDataViewModel { Id = "ErrorCode", Description = "ErrorCode.1" } };
            _localAuthorities = new List<ReferenceDataViewModel> { new ReferenceDataViewModel { Id = "LocalAuthority", Description = "LocalAuthority.1" } };
            _regions = new List<ReferenceDataViewModel> { new ReferenceDataViewModel { Id = "Region", Description = "Region.1" } };

            _service.Setup(x => x.GetReferenceData(LegacyReferenceDataType.Framework)).Returns(new List<ILegacyReferenceData>{new Model.Framework{ Id = "Framework", Description = "Framework.1" }});
            _service.Setup(x => x.GetReferenceData(LegacyReferenceDataType.County)).Returns(new List<ILegacyReferenceData> { new Model.Framework { Id = "County", Description = "County.1" } });
            _service.Setup(x => x.GetReferenceData(LegacyReferenceDataType.ErrorCode)).Returns(new List<ILegacyReferenceData> { new Model.Framework { Id = "ErrorCode", Description = "ErrorCode.1" } });
            _service.Setup(x => x.GetReferenceData(LegacyReferenceDataType.LocalAuthority)).Returns(new List<ILegacyReferenceData> { new Model.Framework { Id = "LocalAuthority", Description = "LocalAuthority.1" } });
            _service.Setup(x => x.GetReferenceData(LegacyReferenceDataType.Region)).Returns(new List<ILegacyReferenceData> { new Model.Framework { Id = "Region", Description = "Region.1" } });           
        }

        [SetUp]
        public void TestSetup()
        {
            _cache = new Mock<ICacheClient>();
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

            _cache.Verify(
                x => x.Put(It.IsAny<LegacyDataProviderCacheKeyEntry>(), It.IsAny<IList<ILegacyReferenceData>>(), It.IsAny<LegacyReferenceDataType>()), Times.Never);

            _service.Verify(x => x.GetReferenceData(type), Times.Once);
        }

        [TestCase(LegacyReferenceDataType.County)]
        [TestCase(LegacyReferenceDataType.ErrorCode)]
        [TestCase(LegacyReferenceDataType.Framework)]
        [TestCase(LegacyReferenceDataType.LocalAuthority)]
        [TestCase(LegacyReferenceDataType.Region)]
        public void DoesGetReturnLegacyReferenceDataWithEmptyCache(LegacyReferenceDataType type)
        {
            var provider = new CacheLegacyReferenceDataProvider(_cache.Object, new LegacyReferenceDataProvider(_service.Object));

            var viewModel = provider.Get(type);

            viewModel.Should().NotBeNull();
            viewModel.Count().Should().Be(1);
            viewModel.First(x => x.Id == type.ToString()).Description.Should().Be(string.Format("{0}.1", type));

            _cache.Verify(
                x => x.Put(It.IsAny<LegacyDataProviderCacheKeyEntry>(), It.IsAny<IEnumerable<ReferenceDataViewModel>>(), It.IsAny<LegacyReferenceDataType>()), Times.Once);

            _service.Verify(x => x.GetReferenceData(type), Times.Once);
        }

        [TestCase(LegacyReferenceDataType.County)]
        [TestCase(LegacyReferenceDataType.ErrorCode)]
        [TestCase(LegacyReferenceDataType.Framework)]
        [TestCase(LegacyReferenceDataType.LocalAuthority)]
        [TestCase(LegacyReferenceDataType.Region)]
        public void DoesGetReturnLegacyReferenceDataWithCache(LegacyReferenceDataType type)
        {
            _cache.Setup(x => x.Get<IEnumerable<ReferenceDataViewModel>>(new LegacyDataProviderCacheKeyEntry().Key("Framework"))).Returns(_frameworks);
            _cache.Setup(x => x.Get<IEnumerable<ReferenceDataViewModel>>(new LegacyDataProviderCacheKeyEntry().Key("County"))).Returns(_counties);
            _cache.Setup(x => x.Get<IEnumerable<ReferenceDataViewModel>>(new LegacyDataProviderCacheKeyEntry().Key("ErrorCode"))).Returns(_errorCodes);
            _cache.Setup(x => x.Get<IEnumerable<ReferenceDataViewModel>>(new LegacyDataProviderCacheKeyEntry().Key("Region"))).Returns(_regions);
            _cache.Setup(x => x.Get<IEnumerable<ReferenceDataViewModel>>(new LegacyDataProviderCacheKeyEntry().Key("LocalAuthority"))).Returns(_localAuthorities);

            var provider = new CacheLegacyReferenceDataProvider(_cache.Object, new LegacyReferenceDataProvider(_service.Object));

            var viewModel = provider.Get(type);

            viewModel.Should().NotBeNull();
            viewModel.Count().Should().Be(1);
            viewModel.First(x => x.Id == type.ToString()).Description.Should().Be(string.Format("{0}.1", type));

            _cache.Verify(
                x => x.Put(It.IsAny<LegacyDataProviderCacheKeyEntry>(), It.IsAny<IList<ILegacyReferenceData>>(), It.IsAny<LegacyReferenceDataType>()), Times.Never);

            _service.Verify(x => x.GetReferenceData(type), Times.Never);
        }
    }
}
