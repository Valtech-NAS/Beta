namespace SFA.Apprenticeships.Domain.Entities.UnitTests.Location
{
    using System;
    using NUnit.Framework;
    using Locations;

    [TestFixture]
    public class LocationHelperPartialPostcodeTests
    {
        [TestCase("CV1")]
        [TestCase("CV12")]
        [TestCase("B1")]
        public void ShouldBeIdentitiedAsPartialPostcode(string postcode)
        {
            Assert.IsTrue(LocationHelper.IsPartialPostcode(postcode));
        }

        [TestCase("London")]
        [TestCase(" C V 1 2WT ")]
        [TestCase("CV")]
        [TestCase("")]
        [TestCase(default(string))]
        [TestCase("CV12WT")]
        [TestCase("cv12wt")]
        [TestCase("CV1 2WT")]
        [TestCase(" CV1 2WT ")]
        [TestCase("CV123")]
        [TestCase("CV1234")]
        [TestCase("CV1 2")]
        [TestCase("CV12W")]
        [TestCase("CV1 2W")]
        public void ShouldNotBeIdentitiedAsPartialPostcode(string postcode)
        {
            Assert.IsFalse(LocationHelper.IsPartialPostcode(postcode));
        }
    }
}
