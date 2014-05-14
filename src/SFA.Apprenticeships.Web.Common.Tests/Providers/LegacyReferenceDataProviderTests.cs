using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.Apprenticeships.Common.Caching;
using SFA.Apprenticeships.Services.ReferenceData.Abstract;
using SFA.Apprenticeships.Services.ReferenceData.Models;
using SFA.Apprenticeships.Web.Common.Providers;
using Model = SFA.Apprenticeships.Services.Models.ReferenceDataModels;

namespace SFA.Apprenticeships.Web.Common.Tests.Providers
{
    [TestFixture]
    public class LegacyReferenceDataProviderTests
    {
        private Mock<IReferenceDataService> _service;
        private Mock<ICacheClient> _cache;
        private IList<Model.Framework> _frameworks;
        private IList<Model.County> _counties;
        private IList<Model.ErrorCode> _errorCodes;
        private IList<Model.LocalAuthority> _localAuthorities;
        private IList<Model.Region> _regions;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _service = new Mock<IReferenceDataService>();

            _frameworks = new List<Model.Framework> {new Model.Framework {CodeName = "Framework", FullName = "Framework.1"}};
            _counties = new List<Model.County> { new Model.County { CodeName = "County", FullName = "County.1" } };
            _errorCodes = new List<Model.ErrorCode> { new Model.ErrorCode() { CodeName = "ErrorCode", FullName = "ErrorCode.1" } };
            _localAuthorities = new List<Model.LocalAuthority> { new Model.LocalAuthority() { CodeName = "LocalAuthority", FullName = "LocalAuthority.1" } };
            _regions = new List<Model.Region> { new Model.Region() { CodeName = "Region", FullName = "Region.1" } };

            _service.Setup(x=>x.GetReferenceData(LegacyReferenceDataType.Framework)).Returns(new List<Model.ILegacyReferenceData>(_frameworks));
            _service.Setup(x=>x.GetReferenceData(LegacyReferenceDataType.County)).Returns(new List<Model.ILegacyReferenceData>(_counties));
            _service.Setup(x=>x.GetReferenceData(LegacyReferenceDataType.ErrorCode)).Returns(new List<Model.ILegacyReferenceData>(_errorCodes));
            _service.Setup(x=>x.GetReferenceData(LegacyReferenceDataType.LocalAuthority)).Returns(new List<Model.ILegacyReferenceData>(_localAuthorities));
            _service.Setup(x=>x.GetReferenceData(LegacyReferenceDataType.Region)).Returns(new List<Model.ILegacyReferenceData>(_regions));           
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
                x => x.Put(It.IsAny<LegacyDataProviderCacheKeyEntry>(), It.IsAny<IList<Model.ILegacyReferenceData>>(), It.IsAny<LegacyReferenceDataType>()), Times.Never);

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
                x => x.Put(It.IsAny<LegacyDataProviderCacheKeyEntry>(), It.IsAny<IList<Model.ILegacyReferenceData>>(), It.IsAny<LegacyReferenceDataType>()), Times.Once);

            _service.Verify(x => x.GetReferenceData(type), Times.Once);
        }

        [TestCase(LegacyReferenceDataType.County)]
        [TestCase(LegacyReferenceDataType.ErrorCode)]
        [TestCase(LegacyReferenceDataType.Framework)]
        [TestCase(LegacyReferenceDataType.LocalAuthority)]
        [TestCase(LegacyReferenceDataType.Region)]
        public void DoesGetReturnLegacyReferenceDataWithCache(LegacyReferenceDataType type)
        {
            _cache.Setup(x=>x.Get<IList<Model.ILegacyReferenceData>>(new LegacyDataProviderCacheKeyEntry().Key("Framework"))).Returns(new List<Model.ILegacyReferenceData>(_frameworks));
            _cache.Setup(x=>x.Get<IList<Model.ILegacyReferenceData>>(new LegacyDataProviderCacheKeyEntry().Key("County"))).Returns(new List<Model.ILegacyReferenceData>(_counties));
            _cache.Setup(x=>x.Get<IList<Model.ILegacyReferenceData>>(new LegacyDataProviderCacheKeyEntry().Key("ErrorCode"))).Returns(new List<Model.ILegacyReferenceData>(_errorCodes));
            _cache.Setup(x=>x.Get<IList<Model.ILegacyReferenceData>>(new LegacyDataProviderCacheKeyEntry().Key("Region"))).Returns(new List<Model.ILegacyReferenceData>(_regions));
            _cache.Setup(x=>x.Get<IList<Model.ILegacyReferenceData>>(new LegacyDataProviderCacheKeyEntry().Key("LocalAuthority"))).Returns(new List<Model.ILegacyReferenceData>(_localAuthorities));

            var provider = new LegacyReferenceDataProvider(_service.Object, _cache.Object);

            var viewModel = provider.Get(type);

            viewModel.Should().NotBeNull();
            viewModel.Count().Should().Be(1);
            viewModel.First(x => x.Id == type.ToString()).Description.Should().Be(string.Format("{0}.1", type));

            _cache.Verify(
                x => x.Put(It.IsAny<LegacyDataProviderCacheKeyEntry>(), It.IsAny<IList<Model.ILegacyReferenceData>>(), It.IsAny<LegacyReferenceDataType>()), Times.Never);

            _service.Verify(x => x.GetReferenceData(type), Times.Never);
        }
    }
}
