namespace SFA.Apprenticeships.Domain.Entities.UnitTests.Vacancies
{
    using Entities.Vacancies;
    using NUnit.Framework;

    [TestFixture]
    public class VacancyHelperTests
    {
        [TestCase("VAC000123456", true)]
        [TestCase("vac000123456", true)]
        [TestCase("vAc000123456", true)]
        [TestCase("000123456", true)]
        [TestCase("00000123456", true)]
        [TestCase("0000000000000000000123456", true)]
        [TestCase("123456", true)]
        [TestCase("VRN000123456", false)]
        [TestCase("chef", false)]
        [TestCase("12345", false)]
        [TestCase("", false)]
        [TestCase(null, false)]
        public void IsVacancyReference(string value, bool expectTrue)
        {
            Assert.That(VacancyHelper.IsVacancyReference(value), Is.EqualTo(expectTrue));
        }

        [TestCase("VAC000123456", true, "123456")]
        [TestCase("vac000123456", true, "123456")]
        [TestCase("vAc000123456", true, "123456")]
        [TestCase("000123456", true, "123456")]
        [TestCase("00000123456", true, "123456")]
        [TestCase("0000000000000000000123456", true, "123456")]
        [TestCase("123456", true, "123456")]
        [TestCase("VRN000123456", false, null)]
        [TestCase("chef", false, null)]
        [TestCase("12345", false, null)]
        [TestCase("", false, null)]
        [TestCase(null, false, null)]
        public void TryGetVacancyReference(string value, bool expectParse, string expectedVacancyReference)
        {
            string vacancyReference;
            Assert.That(VacancyHelper.TryGetVacancyReference(value, out vacancyReference), Is.EqualTo(expectParse));
            Assert.That(vacancyReference, Is.EqualTo(expectedVacancyReference));
        }
    }
}