
using FluentAutomation.Interfaces;

namespace SFA.Apprenticeships.Web.Candidate.IntegrationTests.Pages
{
    public interface ISfaPage
    {
        void Verify();

        IActionSyntaxProvider I { get; }
    }
}
