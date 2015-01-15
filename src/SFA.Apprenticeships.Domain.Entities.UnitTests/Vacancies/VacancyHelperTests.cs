namespace SFA.Apprenticeships.Domain.Entities.UnitTests.Vacancies
{
    using Entities.Vacancies;
    using NUnit.Framework;

    [TestFixture]
    public class VacancyHelperTests
    {
        [TestCase("VAC000123456", true)]
        [TestCase("000123456", true)]
        [TestCase("123456", true)]
        [TestCase("VRN000123456", false)]
        [TestCase("chef", false)]
        [TestCase("12345", false)]
        [TestCase("", false)]
        [TestCase(null, false)]
        public void IsVacancyReferenceNumber(string value, bool expectTrue)
        {
            Assert.That(VacancyHelper.IsVacancyReferenceNumber(value), Is.EqualTo(expectTrue));
        }

        [TestCase("VAC000123456", true, 123456)]
        [TestCase("000123456", true, 123456)]
        [TestCase("123456", true, 123456)]
        [TestCase("VRN000123456", false, 0)]
        [TestCase("chef", false, 0)]
        [TestCase("12345", false, 0)]
        [TestCase("", false, 0)]
        [TestCase(null, false, 0)]
        public void TryGetVacancyReferenceNumber(string value, bool expectParse, int expectedVacancyReferenceNumber)
        {
            int vacancyReferenceNumber;
            Assert.That(VacancyHelper.TryGetVacancyReferenceNumber(value, out vacancyReferenceNumber), Is.EqualTo(expectParse));
            Assert.That(vacancyReferenceNumber, Is.EqualTo(expectedVacancyReferenceNumber));
        }
    }
}