using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.Apprenticeships.Common.Caching;
using SFA.Apprenticeships.Services.Models.ReferenceData;
using SFA.Apprenticeships.Services.ReferenceData.Abstract;
using SFA.Apprenticeships.Services.ReferenceData.Models;
using SFA.Apprenticeships.Web.Common.Providers;

namespace SFA.Apprenticeships.Web.Common.Tests.Providers
{
    [TestFixture]
    public class LegacyReferenceDataProviderTests
    {
        private Mock<IReferenceDataService> _service;
        private Mock<ICacheClient> _cache;
        private IList<Services.Models.ReferenceData.Framework> _frameworks;
        private IList<County> _counties;
        private IList<ErrorCode> _errorCodes;
        private IList<LocalAuthority> _localAuthorities;
        private IList<Region> _regions;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _service = new Mock<IReferenceDataService>();

            _frameworks = new List<Services.Models.ReferenceData.Framework> {new Services.Models.ReferenceData.Framework {Id = "Framework", Description = "Framework.1"}};
            _counties = new List<County> { new County { Id = "County", Description = "County.1" } };
            _errorCodes = new List<ErrorCode> { new ErrorCode() { Id = "ErrorCode", Description = "ErrorCode.1" } };
            _localAuthorities = new List<LocalAuthority> { new LocalAuthority() { Id = "LocalAuthority", Description = "LocalAuthority.1" } };
            _regions = new List<Region> { new Region() { Id = "Region", Description = "Region.1" } };

            _service.Setup(x=>x.GetReferenceData(LegacyReferenceDataType.Framework)).Returns(new List<ILegacyReferenceData>(_frameworks));
            _service.Setup(x=>x.GetReferenceData(LegacyReferenceDataType.County)).Returns(new List<ILegacyReferenceData>(_counties));
            _service.Setup(x=>x.GetReferenceData(LegacyReferenceDataType.ErrorCode)).Returns(new List<ILegacyReferenceData>(_errorCodes));
            _service.Setup(x=>x.GetReferenceData(LegacyReferenceDataType.LocalAuthority)).Returns(new List<ILegacyReferenceData>(_localAuthorities));
            _service.Setup(x=>x.GetReferenceData(LegacyReferenceDataType.Region)).Returns(new List<ILegacyReferenceData>(_regions));           
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
            var provider = new LegacyReferenceDataProvider(_service.Object, _cache.Object);

            var viewModel = provider.Get(type);

            viewModel.Should().NotBeNull();
            viewModel.Count().Should().Be(1);
            viewModel.First(x => x.Id == type.ToString()).Description.Should().Be(string.Format("{0}.1", type));

            _cache.Verify(
                x => x.Put(It.IsAny<LegacyDataProviderCacheKeyEntry>(), It.IsAny<IList<ILegacyReferenceData>>(), It.IsAny<LegacyReferenceDataType>()), Times.Once);

            _service.Verify(x => x.GetReferenceData(type), Times.Once);
        }

        [TestCase(LegacyReferenceDataType.County)]
        [TestCase(LegacyReferenceDataType.ErrorCode)]
        [TestCase(LegacyReferenceDataType.Framework)]
        [TestCase(LegacyReferenceDataType.LocalAuthority)]
        [TestCase(LegacyReferenceDataType.Region)]
        public void DoesGetReturnLegacyReferenceDataWithCache(LegacyReferenceDataType type)
        {
            _cache.Setup(x=>x.Get<IList<ILegacyReferenceData>>(new LegacyDataProviderCacheKeyEntry().Key("Framework"))).Returns(new List<ILegacyReferenceData>(_frameworks));
            _cache.Setup(x=>x.Get<IList<ILegacyReferenceData>>(new LegacyDataProviderCacheKeyEntry().Key("County"))).Returns(new List<ILegacyReferenceData>(_counties));
            _cache.Setup(x=>x.Get<IList<ILegacyReferenceData>>(new LegacyDataProviderCacheKeyEntry().Key("ErrorCode"))).Returns(new List<ILegacyReferenceData>(_errorCodes));
            _cache.Setup(x=>x.Get<IList<ILegacyReferenceData>>(new LegacyDataProviderCacheKeyEntry().Key("Region"))).Returns(new List<ILegacyReferenceData>(_regions));
            _cache.Setup(x=>x.Get<IList<ILegacyReferenceData>>(new LegacyDataProviderCacheKeyEntry().Key("LocalAuthority"))).Returns(new List<ILegacyReferenceData>(_localAuthorities));

            var provider = new LegacyReferenceDataProvider(_service.Object, _cache.Object);

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
