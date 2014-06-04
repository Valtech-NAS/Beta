namespace SFA.Apprenticeships.Infrastructure.Common.Tests.IoC
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;
    using SFA.Apprenticeships.Infrastructure.Common.IoC;
    using StructureMap;

    [TestFixture]
    public class CommonRegistryTests
    {
        [Test]
        public void CheckRegistryDoesntThrowException()
        {
            Action test = () =>
            {
                ObjectFactory.Initialize(x =>
                {
                    x.AddRegistry<CommonRegistry>();
                });
            };

            test.ShouldNotThrow<Exception>();
        }
    }
}