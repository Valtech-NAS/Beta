using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.Apprenticeships.Web.Common.IoC.DependencyResolution;

namespace SFA.Apprenticeships.Web.Common.IntegrationTests.DependencyResolutionTests
{
    using IoC = SFA.Apprenticeships.Web.Common.IoC.DependencyResolution.IoC;

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