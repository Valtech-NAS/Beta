namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipApplication
{
    using System.Collections.Generic;
    using Candidate.Mediators;
    using Candidate.ViewModels.Applications;
    using Candidate.ViewModels.Candidate;
    using Candidate.ViewModels.VacancySearch;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class AddEmptyWorkExperienceRowsTests : TestsBase
    {
        private static string SomeJobTitle;
        private const string BlankSpace = "  ";
        private const string SomeWorkExperienceDescription = "Work experience description";
        private const string SomeEmployer = "Some employer";
        private const int SomeMonth = 1;
        private const string SomeYear = "2012";

        [Test]
        public void Ok()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new VacancyDetailViewModel()
            };

            var response = Mediator.AddEmptyWorkExperienceRows(viewModel);

            response.AssertCode(Codes.ApprenticeshipApplication.AddEmptyWorkExperienceRows.Ok, true);
        }

        [Test]
        public void WillRemoveEmptyWorkExperienceRows()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = CreateCandidateWithOneWorkExerienceAndTwoEmptyWorkExperiences(),
                VacancyDetail = new VacancyDetailViewModel()
            };

            var response = Mediator.AddEmptyWorkExperienceRows(viewModel);

            response.AssertCode(Codes.ApprenticeshipApplication.AddEmptyWorkExperienceRows.Ok, true);
            response.ViewModel.Candidate.WorkExperience.Should().HaveCount(1);
        }

        private static ApprenticeshipCandidateViewModel CreateCandidateWithOneWorkExerienceAndTwoEmptyWorkExperiences()
        {
            SomeJobTitle = "Job title";
            return new ApprenticeshipCandidateViewModel
            {
                WorkExperience = new List<WorkExperienceViewModel>
                {
                    new WorkExperienceViewModel(),
                    new WorkExperienceViewModel
                    {
                        Description = SomeWorkExperienceDescription,
                        Employer = SomeEmployer,
                        FromMonth = SomeMonth,
                        FromYear = SomeYear,
                        JobTitle = SomeJobTitle,
                        ToMonth = SomeMonth,
                        ToYear = SomeYear
                    },
                    new WorkExperienceViewModel
                    {
                        Description = BlankSpace,
                        Employer = BlankSpace,
                        FromMonth = SomeMonth,
                        FromYear = BlankSpace,
                        JobTitle = BlankSpace,
                        ToMonth = SomeMonth,
                        ToYear = BlankSpace
                    }
                }
            };
        }
    }
}