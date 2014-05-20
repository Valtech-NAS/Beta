namespace SFA.Apprenticeships.Common.Tests.DependencyResolutionTests
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;
    using SFA.Apprenticeships.Common.IoC;

    [TestFixture]
    public class DependencyRegistrationTest
    {
        [Test]
        public void CheckDependencyRegistrationDoesntThrowException()
        {
            Action test = () => IoC.Initialize();

            test.ShouldNotThrow<Exception>();
        }
    }
}