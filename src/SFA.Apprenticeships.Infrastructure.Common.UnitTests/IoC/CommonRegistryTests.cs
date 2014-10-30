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
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            Action test = () => ObjectFactory.Initialize(x => x.AddRegistry<CommonRegistry>());
#pragma warning restore 0618

            test.ShouldNotThrow<Exception>();
        }
    }
}