namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mappers
{
    using Candidate.ViewModels.VacancySearch;
    using Domain.Entities.Vacancies;
    using Domain.Entities.Vacancies.Apprenticeships;
    using FluentAssertions;
    using NUnit.Framework;
    using Candidate.Mappers;

    [TestFixture]
    public class VacancyDetailViewModelMapperTests
    {
        [Test]
        public void ShouldCreateMap()
        {
            var apprenticeshipCandidateWebMappers = new ApprenticeshipCandidateWebMappers();

            apprenticeshipCandidateWebMappers.Mapper.AssertConfigurationIsValid();

            var traineeshipCandidateWebMappers = new TraineeshipCandidateWebMappers();

            traineeshipCandidateWebMappers.Mapper.AssertConfigurationIsValid();
        }

        [Test]
        public void ShouldNotAnonymiseEmployerNameIfEmployerHasNotOptedForAnonymity()
        {
            var vacancyDetail = new ApprenticeshipVacancyDetail
            {
                EmployerName = "Acme Corp",
                IsEmployerAnonymous = false
            };

            var model = new ApprenticeshipCandidateWebMappers().Map<ApprenticeshipVacancyDetail, VacancyDetailViewModel>(vacancyDetail);

            model.Should().NotBeNull();
            model.EmployerName.Should().Be(vacancyDetail.EmployerName);
        }

        [Test]
        public void ShouldAnonymiseEmployerNameIfEmployerHasOptedForAnonymity()
        {
            var vacancyDetail = new ApprenticeshipVacancyDetail
            {
                EmployerName = "Acme Corp",
                AnonymousEmployerName = "Blue Chip Corp",
                IsEmployerAnonymous = true
            };

            var model = new ApprenticeshipCandidateWebMappers().Map<ApprenticeshipVacancyDetail, VacancyDetailViewModel>(vacancyDetail);

            model.Should().NotBeNull();
            model.EmployerName.Should().Be(vacancyDetail.AnonymousEmployerName);
        }

        [Test]
        public void ShouldShowWageIfWageTypeIsWeekly()
        {
            var vacancyDetail = new ApprenticeshipVacancyDetail
            {
                WageType = WageType.Weekly,
                Wage = 101.19m
            };

            var model = new ApprenticeshipCandidateWebMappers().Map<ApprenticeshipVacancyDetail, VacancyDetailViewModel>(vacancyDetail);
            const string expectedWage = "£101.19";

            model.Should().NotBeNull();
            model.Wage.Should().Be(expectedWage);
        }

        [Test]
        public void ShouldShowWageDescriptionIfWageTypeIsText()
        {
            var vacancyDetail = new ApprenticeshipVacancyDetail
            {
                WageType = WageType.Text,
                WageDescription = "Competetive"
            };

            var model = new ApprenticeshipCandidateWebMappers().Map<ApprenticeshipVacancyDetail, VacancyDetailViewModel>(vacancyDetail);

            model.Should().NotBeNull();
            model.Wage.Should().Be(vacancyDetail.WageDescription);
        }

        [Test]
        public void ShouldReturnTheEmployerWebsiteWithHttpIfItAlreadyHasIt()
        {
            var vacancyDetail = new ApprenticeshipVacancyDetail
            {
                EmployerWebsite = "http://wwww.someweb.com"
            };

            var model = new ApprenticeshipCandidateWebMappers().Map<ApprenticeshipVacancyDetail, VacancyDetailViewModel>(vacancyDetail);

            model.Should().NotBeNull();
            model.EmployerWebsite.Should().StartWith("http://");
            model.IsWellFormedEmployerWebsiteUrl.Should().Be(true);
        }

        [Test]
        public void ShouldReturnTheEmployerWebsiteWithHttpIfItDoesntHaveIt()
        {
            var vacancyDetail = new ApprenticeshipVacancyDetail
            {
                EmployerWebsite = "wwww.someweb.com"
            };

            var model = new ApprenticeshipCandidateWebMappers().Map<ApprenticeshipVacancyDetail, VacancyDetailViewModel>(vacancyDetail);

            model.Should().NotBeNull();
            model.EmployerWebsite.Should().StartWith("http://");
            model.IsWellFormedEmployerWebsiteUrl.Should().Be(true);
        }

        [Test]
        public void ShouldReturnTheEmployerWebsiteWithHttpsIfItStartsWithIt()
        {
            var vacancyDetail = new ApprenticeshipVacancyDetail
            {
                EmployerWebsite = "https://wwww.someweb.com"
            };

            var model = new ApprenticeshipCandidateWebMappers().Map<ApprenticeshipVacancyDetail, VacancyDetailViewModel>(vacancyDetail);

            model.Should().NotBeNull();
            model.EmployerWebsite.Should().StartWith("https://");
            model.IsWellFormedEmployerWebsiteUrl.Should().Be(true);
        }

        [Test]
        public void ShouldReturnRawStringIfEmployerWebsiteIsNotWellFormed()
        {
            var vacancyDetail = new ApprenticeshipVacancyDetail
            {
                EmployerWebsite = "www.somedomain.co.uk / www.anotherdomain.co.uk"
            };

            var model = new ApprenticeshipCandidateWebMappers().Map<ApprenticeshipVacancyDetail, VacancyDetailViewModel>(vacancyDetail);

            model.Should().NotBeNull();
            model.EmployerWebsite.Should().Be(vacancyDetail.EmployerWebsite);
            model.IsWellFormedEmployerWebsiteUrl.Should().Be(false);
        }

        [Test]
        public void ShouldReturnTheVacancyUrlWithHttpIfItAlreadyHasIt()
        {
            var vacancyDetail = new ApprenticeshipVacancyDetail
            {
                VacancyUrl = "http://wwww.someweb.com"
            };

            var model = new ApprenticeshipCandidateWebMappers().Map<ApprenticeshipVacancyDetail, VacancyDetailViewModel>(vacancyDetail);

            model.Should().NotBeNull();
            model.VacancyUrl.Should().StartWith("http://");
            model.IsWellFormedVacancyUrl.Should().Be(true);
        }

        [Test]
        public void ShouldReturnTheVacancyUrlWithHttpIfItDoesntHaveIt()
        {
            var vacancyDetail = new ApprenticeshipVacancyDetail
            {
                VacancyUrl = "wwww.someweb.com"
            };

            var model = new ApprenticeshipCandidateWebMappers().Map<ApprenticeshipVacancyDetail, VacancyDetailViewModel>(vacancyDetail);

            model.Should().NotBeNull();
            model.VacancyUrl.Should().StartWith("http://");
            model.IsWellFormedVacancyUrl.Should().Be(true);
        }

        [Test]
        public void ShouldReturnTheVacancyUrlWithHttpsIfItStartsWithIt()
        {
            var vacancyDetail = new ApprenticeshipVacancyDetail
            {
                VacancyUrl = "https://wwww.someweb.com"
            };

            var model = new ApprenticeshipCandidateWebMappers().Map<ApprenticeshipVacancyDetail, VacancyDetailViewModel>(vacancyDetail);

            model.Should().NotBeNull();
            model.VacancyUrl.Should().StartWith("https://");
            model.IsWellFormedVacancyUrl.Should().Be(true);
        }

        [Test]
        public void ShouldReturnRawStringIfVacancyUrlIsNotWellFormed()
        {
            var vacancyDetail = new ApprenticeshipVacancyDetail
            {
                VacancyUrl = "www.somedomain.co.uk / www.anotherdomain.co.uk"
            };

            var model = new ApprenticeshipCandidateWebMappers().Map<ApprenticeshipVacancyDetail, VacancyDetailViewModel>(vacancyDetail);

            model.Should().NotBeNull();
            model.VacancyUrl.Should().Be(vacancyDetail.VacancyUrl);
            model.IsWellFormedVacancyUrl.Should().Be(false);
        }
    }
}
