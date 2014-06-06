﻿namespace SFA.Apprenticeships.Infrastructure.LocationLookup.IntegrationTests
{
    using System;
    using System.Linq;
    using Application.Interfaces.Location;
    using Elastic.Common.IoC;
    using IoC;
    using NUnit.Framework;
    using StructureMap;

    public class LocationLookupProviderTests
    {
        [SetUp]
        public void SetUp()
        {
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<ElasticsearchCommonRegistry>();
                x.AddRegistry<LocationLookupRegistry>();
            });
        }

        [Test]
        public void ShouldMatchExactPlaceName()
        {
            // arrange
            var service = ObjectFactory.GetInstance<ILocationLookupProvider>();
            const string term = "Cheylesmore";

            // act
            var results = service.FindLocation(term).ToList();

            // assert
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(term, results.First().Name);
        }

        [Test]
        public void ShouldMatchFuzzyPlaceName()
        {
            // arrange
            var service = ObjectFactory.GetInstance<ILocationLookupProvider>();
            const string term = "Cheyles";

            // act
            var results = service.FindLocation(term).ToList();

            // assert
            Assert.AreEqual(11, results.Count);
            Assert.AreEqual("Chesley (Kent)", results.First().Name);
        }

        [Test]
        public void ShouldMatchFuzzyCountyName()
        {
            // arrange
            var service = ObjectFactory.GetInstance<ILocationLookupProvider>();
            const string term = "Warwick";

            // act
            var results = service.FindLocation(term, 250).ToList();

            // assert
            Assert.AreEqual(88, results.Count);
            Assert.AreEqual("Warwick (Warwickshire)", results.First().Name);
        }

        [Test]
        public void ShouldMatchExactCountyNameAndReturnAllPlacesInCounty()
        {
            // arrange
            var service = ObjectFactory.GetInstance<ILocationLookupProvider>();
            const string term = "Warwickshire";

            // act
            var results = service.FindLocation(term, 250).ToList();

            // assert
            Assert.AreEqual(240, results.Count);
        }
    }
}
