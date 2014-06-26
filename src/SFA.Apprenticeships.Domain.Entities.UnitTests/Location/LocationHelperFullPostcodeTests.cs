namespace SFA.Apprenticeships.Domain.Entities.UnitTests.Location
{
    using System;
    using NUnit.Framework;
    using Locations;

    [TestFixture]
    public class LocationHelperFullPostcodeTests
    {
        [TestCase("CV12WT")]
        [TestCase("cv12wt")]
        [TestCase("CV1 2WT")]
        [TestCase(" CV1 2WT ")]
        public void ShouldBeIdentitiedAsPostcode(string postcode)
        {
            Assert.IsTrue(LocationHelper.IsPostcode(postcode));
        }

        [TestCase("London")]
        [TestCase(" C V 1 2WT ")]
        [TestCase("")]
        [TestCase(default(string))]
        public void ShouldNotBeIdentitiedAsPostcode(string postcode)
        {
            Assert.IsFalse(LocationHelper.IsPostcode(postcode));
        }
    }
}
