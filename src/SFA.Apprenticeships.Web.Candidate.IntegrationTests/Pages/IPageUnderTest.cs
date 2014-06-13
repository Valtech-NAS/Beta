
using FluentAutomation.Interfaces;

namespace SFA.Apprenticeships.Web.Candidate.IntegrationTests.Pages
{
    public interface IPageUnderTest
    {
        void Verify();

        INativeActionSyntaxProvider I { get; }
    }
}
