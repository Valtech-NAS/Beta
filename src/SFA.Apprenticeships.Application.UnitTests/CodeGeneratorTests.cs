namespace SFA.Apprenticeships.Application.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.UserAccount;
    using Interfaces.Logging;
    using Moq;
    using NUnit.Framework;
    
    [TestFixture]
    public class ActivationCodeGeneratorTests
    {
        [Test]
        public void ShouldGenerateRandomSixCharacterAlphanumericCodes()
        {
            const int sampleSize = 100;
            var sampleCodes = new List<string>();

            for (var i = 0; i < sampleSize; i++)
            {
                // Arrange.
                var generator = new RandomCodeGenerator(new Mock<ILogService>().Object);

                // Act.
                var code = generator.GenerateAlphaNumeric();

                // Assert.
                Assert.IsFalse(String.IsNullOrWhiteSpace(code));
                Assert.AreEqual(6, code.Length);
                Assert.IsTrue(code.All(c => RandomCodeGenerator.Alphanumerics.Contains(c)));

                CollectionAssert.DoesNotContain(sampleCodes, code);

                sampleCodes.Add(code);
            }

            Assert.AreEqual(sampleSize, sampleCodes.Count);
        }

        [Test]
        public void ShouldGenerateRandomFourCharacterNumericCodes()
        {
            const int sampleSize = 10;
            var sampleCodes = new List<string>();

            for (var i = 0; i < sampleSize; i++)
            {
                // Arrange.
                var generator = new RandomCodeGenerator(new Mock<ILogService>().Object);

                // Act.
                var code = generator.GenerateNumeric();

                // Assert.
                Assert.IsFalse(String.IsNullOrWhiteSpace(code));
                Assert.AreEqual(4, code.Length);
                Assert.IsTrue(code.All(c => RandomCodeGenerator.Numerics.Contains(c)));

                CollectionAssert.DoesNotContain(sampleCodes, code);

                sampleCodes.Add(code);
            }

            Assert.AreEqual(sampleSize, sampleCodes.Count);
        }
    }
}
