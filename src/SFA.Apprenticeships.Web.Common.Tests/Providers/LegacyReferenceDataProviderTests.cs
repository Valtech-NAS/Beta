using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
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
        private IReferenceDataService _service;
        private ICacheClient _cache;
        private IList<Model.Framework> _frameworks;
        private IList<Model.County> _counties;
        private IList<Model.ErrorCode> _errorCodes;
        private IList<Model.LocalAuthority> _localAuthorities;
        private IList<Model.Region> _regions;

        [TestFixtureSetUp]
        public void Setup()
        {
            _service = Substitute.For<IReferenceDataService>();

            _frameworks = new List<Model.Framework> {new Model.Framework {CodeName = "Framework", FullName = "Framework.1"}};
            _counties = new List<Model.County> { new Model.County { CodeName = "County", FullName = "County.1" } };
            _errorCodes = new List<Model.ErrorCode> { new Model.ErrorCode() { CodeName = "ErrorCode", FullName = "ErrorCode.1" } };
            _localAuthorities = new List<Model.LocalAuthority> { new Model.LocalAuthority() { CodeName = "LocalAuthority", FullName = "LocalAuthority.1" } };
            _regions = new List<Model.Region> { new Model.Region() { CodeName = "Region", FullName = "Region.1" } };

            _service.GetReferenceData(Arg.Is(LegacyReferenceDataType.Framework)).Returns(new List<Model.ILegacyReferenceData>(_frameworks));
            _service.GetReferenceData(Arg.Is(LegacyReferenceDataType.County)).Returns(new List<Model.ILegacyReferenceData>(_counties));
            _service.GetReferenceData(Arg.Is(LegacyReferenceDataType.ErrorCode)).Returns(new List<Model.ILegacyReferenceData>(_errorCodes));
            _service.GetReferenceData(Arg.Is(LegacyReferenceDataType.LocalAuthority)).Returns(new List<Model.ILegacyReferenceData>(_localAuthorities));
            _service.GetReferenceData(Arg.Is(LegacyReferenceDataType.Region)).Returns(new List<Model.ILegacyReferenceData>(_regions));           
        }

        [SetUp]
        public void TestSetup()
        {
            _cache = Substitute.For<ICacheClient>();
            _service.ClearReceivedCalls();
        }

        [TestCase(LegacyReferenceDataType.County)]
        [TestCase(LegacyReferenceDataType.ErrorCode)]
        [TestCase(LegacyReferenceDataType.Framework)]
        [TestCase(LegacyReferenceDataType.LocalAuthority)]
        [TestCase(LegacyReferenceDataType.Region)]
        public void DoesGetReturnLegacyReferenceDataWithoutCache(LegacyReferenceDataType type)
        {
            var provider = new LegacyReferenceDataProvider(_service);

            var viewModel = provider.Get(type);

            viewModel.Should().NotBeNull();
            viewModel.Count().Should().Be(1);
            viewModel.First(x => x.Id == type.ToString()).Description.Should().Be(string.Format("{0}.1", type));

            _cache.Received(0).Put(Arg.Any<LegacyDataProviderCacheKeyEntry>(), Arg.Any<IList<Model.ILegacyReferenceData>>(), Arg.Any<LegacyReferenceDataType>());
            _service.Received(1).GetReferenceData(type);
        }

        [TestCase(LegacyReferenceDataType.County)]
        [TestCase(LegacyReferenceDataType.ErrorCode)]
        [TestCase(LegacyReferenceDataType.Framework)]
        [TestCase(LegacyReferenceDataType.LocalAuthority)]
        [TestCase(LegacyReferenceDataType.Region)]
        public void DoesGetReturnLegacyReferenceDataWithEmptyCache(LegacyReferenceDataType type)
        {
            _cache.ClearReceivedCalls();

            var provider = new LegacyReferenceDataProvider(_service, _cache);

            var viewModel = provider.Get(type);

            viewModel.Should().NotBeNull();
            viewModel.Count().Should().Be(1);
            viewModel.First(x => x.Id == type.ToString()).Description.Should().Be(string.Format("{0}.1", type));

            _cache.Received(1).Put(Arg.Any<LegacyDataProviderCacheKeyEntry>(), Arg.Any<IList<Model.ILegacyReferenceData>>(), Arg.Any<LegacyReferenceDataType>());
            _service.Received(1).GetReferenceData(type);
        }

        [TestCase(LegacyReferenceDataType.County)]
        [TestCase(LegacyReferenceDataType.ErrorCode)]
        [TestCase(LegacyReferenceDataType.Framework)]
        [TestCase(LegacyReferenceDataType.LocalAuthority)]
        [TestCase(LegacyReferenceDataType.Region)]
        public void DoesGetReturnLegacyReferenceDataWithCache(LegacyReferenceDataType type)
        {
            _cache.Get<IList<Model.ILegacyReferenceData>>(Arg.Is(new LegacyDataProviderCacheKeyEntry().Key("Framework"))).Returns(new List<Model.ILegacyReferenceData>(_frameworks));
            _cache.Get<IList<Model.ILegacyReferenceData>>(Arg.Is(new LegacyDataProviderCacheKeyEntry().Key("County"))).Returns(new List<Model.ILegacyReferenceData>(_counties));
            _cache.Get<IList<Model.ILegacyReferenceData>>(Arg.Is(new LegacyDataProviderCacheKeyEntry().Key("ErrorCode"))).Returns(new List<Model.ILegacyReferenceData>(_errorCodes));
            _cache.Get<IList<Model.ILegacyReferenceData>>(Arg.Is(new LegacyDataProviderCacheKeyEntry().Key("Region"))).Returns(new List<Model.ILegacyReferenceData>(_regions));
            _cache.Get<IList<Model.ILegacyReferenceData>>(Arg.Is(new LegacyDataProviderCacheKeyEntry().Key("LocalAuthority"))).Returns(new List<Model.ILegacyReferenceData>(_localAuthorities));

            var provider = new LegacyReferenceDataProvider(_service, _cache);

            var viewModel = provider.Get(type);

            viewModel.Should().NotBeNull();
            viewModel.Count().Should().Be(1);
            viewModel.First(x => x.Id == type.ToString()).Description.Should().Be(string.Format("{0}.1", type));

            _cache.Received(0).Put(Arg.Any<LegacyDataProviderCacheKeyEntry>(), Arg.Any<IList<Model.ILegacyReferenceData>>(), Arg.Any<LegacyReferenceDataType>());
            _service.Received(0).GetReferenceData(type);
        }
    }
}
