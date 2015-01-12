namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipApplication
{
    using Candidate.Mediators;
    using Candidate.ViewModels.Applications;
    using Candidate.ViewModels.Candidate;
    using Candidate.ViewModels.VacancySearch;
    using NUnit.Framework;

    [TestFixture]
    public class AddEmptyQualificationRowsTests : TestsBase
    {
        [Test]
        public void Ok()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new VacancyDetailViewModel()
            };

            var response = Mediator.AddEmptyQualificationRows(viewModel);

            response.AssertCode(Codes.ApprenticeshipApplication.AddEmptyQualificationRows.Ok, true);
        }
    }
}