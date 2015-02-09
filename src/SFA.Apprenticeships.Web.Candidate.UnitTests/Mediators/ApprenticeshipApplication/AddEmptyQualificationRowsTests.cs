namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipApplication
{
    using System.Collections.Generic;
    using Candidate.Mediators.Application;
    using Candidate.ViewModels.Applications;
    using Candidate.ViewModels.Candidate;
    using Candidate.ViewModels.VacancySearch;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class AddEmptyQualificationRowsTests : TestsBase
    {
        private const bool SomeBoolean = false;
        private const string SomeYear = "2011";
        private const string BlankSpace = "  ";
        private const string SomeGrade = "A";
        private const string SomeQualificationType = "QualificationType";
        private const string SomeSubject = "Subject";

        [Test]
        public void Ok()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new VacancyDetailViewModel()
            };

            var response = Mediator.AddEmptyQualificationRows(viewModel);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.AddEmptyQualificationRows.Ok, true);
        }

        [Test]
        public void WillRemoveEmptyQualficationRows()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = CreateCandidateWithOneQualificationAndTwoEmptyQualifications(),
                VacancyDetail = new VacancyDetailViewModel()
            };

            var response = Mediator.AddEmptyQualificationRows(viewModel);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.AddEmptyQualificationRows.Ok, true);
            response.ViewModel.Candidate.Qualifications.Should().HaveCount(1);
        }

        private static ApprenticeshipCandidateViewModel CreateCandidateWithOneQualificationAndTwoEmptyQualifications()
        {
            return new ApprenticeshipCandidateViewModel
            {
                Qualifications = new List<QualificationsViewModel>
                {
                    new QualificationsViewModel(),
                    new QualificationsViewModel
                    {
                        Grade = SomeGrade,
                        IsPredicted = SomeBoolean,
                        QualificationType = SomeQualificationType,
                        Subject = SomeSubject,
                        Year = SomeYear
                    },
                    new QualificationsViewModel
                    {
                        Grade = BlankSpace,
                        IsPredicted = SomeBoolean,
                        QualificationType = BlankSpace,
                        Subject = BlankSpace,
                        Year = BlankSpace
                    }
                }
            };
        }
    }
}