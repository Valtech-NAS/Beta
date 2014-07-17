namespace SFA.Apprenticeships.Application.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using Registration;

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
                var generator = new CodeGenerator();

                // Act.
                var code = generator.Generate();

                // Assert.
                Assert.IsFalse(String.IsNullOrWhiteSpace(code));
                Assert.AreEqual(CodeGenerator.CodeLength, code.Length);
                Assert.IsTrue(code.All(c => CodeGenerator.Alphanumerics.Contains(c)));

                CollectionAssert.DoesNotContain(sampleCodes, code);

                sampleCodes.Add(code);
            }

            Assert.AreEqual(sampleSize, sampleCodes.Count);
        }
    }
}
