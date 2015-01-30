namespace SFA.Apprenticeships.Infrastructure.Common.UnitTests.IoC
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;
    using Common.IoC;
    using StructureMap;

    [TestFixture]
    public class CommonRegistryTests
    {
        [Test]
        public void CheckRegistryDoesntThrowException()
        {
            Action test = () => new Container(ce => ce.AddRegistry<CommonRegistry>());

            test.ShouldNotThrow<Exception>();
        }
    }
}