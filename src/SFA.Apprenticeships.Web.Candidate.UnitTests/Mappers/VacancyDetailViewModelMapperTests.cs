namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mappers
{
    using Candidate.ViewModels.VacancySearch;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Mapping;
    using FluentAssertions;
    using IoC;
    using NUnit.Framework;
    using StructureMap;


    [TestFixture]
    public class VacancyDetailViewModelMapperTests
    {
        private IMapper _mapper;

        public VacancyDetailViewModelMapperTests()
        {
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            ObjectFactory.Initialize(x => x.AddRegistry<CandidateWebRegistry>());
#pragma warning restore 0618
        }

        [SetUp]
        public void Setup()
        {
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            _mapper = ObjectFactory.GetInstance<IMapper>();
#pragma warning restore 0618
        }

        [Test]
        public void ShouldNotAnonymiseEmployerNameIfEmployerHasNotOptedForAnonymity()
        {
            var vacancyDetail = new VacancyDetail
            {
                EmployerName = "Acme Corp",
                IsEmployerAnonymous = false
            };

            var model = _mapper.Map<VacancyDetail, VacancyDetailViewModel>(vacancyDetail);

            model.Should().NotBeNull();
            model.EmployerName.Should().Be(vacancyDetail.EmployerName);
        }

        [Test]
        public void ShouldAnonymiseEmployerNameIfEmployerHasOptedForAnonymity()
        {
            var vacancyDetail = new VacancyDetail
            {
                EmployerName = "Acme Corp",
                AnonymousEmployerName = "Blue Chip Corp",
                IsEmployerAnonymous = true
            };

            var model = _mapper.Map<VacancyDetail, VacancyDetailViewModel>(vacancyDetail);

            model.Should().NotBeNull();
            model.EmployerName.Should().Be(vacancyDetail.AnonymousEmployerName);
        }

        [Test]
        public void ShouldShowWageIfWageTypeIsWeekly()
        {
            var vacancyDetail = new VacancyDetail
            {
                WageType = WageType.Weekly,
                Wage = 101.19m
            };

            var model = _mapper.Map<VacancyDetail, VacancyDetailViewModel>(vacancyDetail);
            const string expectedWage = "£101.19";

            model.Should().NotBeNull();
            model.Wage.Should().Be(expectedWage);
        }

        [Test]
        public void ShouldShowWageDescriptionIfWageTypeIsText()
        {
            var vacancyDetail = new VacancyDetail
            {
                WageType = WageType.Text,
                WageDescription = "Competetive"
            };

            var model = _mapper.Map<VacancyDetail, VacancyDetailViewModel>(vacancyDetail);

            model.Should().NotBeNull();
            model.Wage.Should().Be(vacancyDetail.WageDescription);
        }

        [Test]
        public void ShouldReturnTheEmployerWebsiteWithHttpIfItAlreadyHasIt()
        {
            var vacancyDetail = new VacancyDetail
            {
                EmployerWebsite = "http://wwww.someweb.com"
            };

            var model = _mapper.Map<VacancyDetail, VacancyDetailViewModel>(vacancyDetail);

            model.Should().NotBeNull();
            model.EmployerWebsite.Should().StartWith("http://");
            model.IsWellFormedEmployerWebsiteUrl.Should().Be(true);
        }

        [Test]
        public void ShouldReturnTheEmployerWebsiteWithHttpIfItDoesntHaveIt()
        {
            var vacancyDetail = new VacancyDetail
            {
                EmployerWebsite = "wwww.someweb.com"
            };  

            var model = _mapper.Map<VacancyDetail, VacancyDetailViewModel>(vacancyDetail);

            model.Should().NotBeNull();
            model.EmployerWebsite.Should().StartWith("http://");
            model.IsWellFormedEmployerWebsiteUrl.Should().Be(true);
        }

        [Test]
        public void ShouldReturnTheEmployerWebsiteWithHttpsIfItStartsWithIt()
        {
            var vacancyDetail = new VacancyDetail
            {
                EmployerWebsite = "https://wwww.someweb.com"
            };

            var model = _mapper.Map<VacancyDetail, VacancyDetailViewModel>(vacancyDetail);

            model.Should().NotBeNull();
            model.EmployerWebsite.Should().StartWith("https://");
            model.IsWellFormedEmployerWebsiteUrl.Should().Be(true);
        }

        [Test]
        public void ShouldReturnRawStringIfEmployerWebsiteIsNotWellFormed()
        {
            var vacancyDetail = new VacancyDetail
            {
                EmployerWebsite = "www.somedomain.co.uk / www.anotherdomain.co.uk"
            };

            var model = _mapper.Map<VacancyDetail, VacancyDetailViewModel>(vacancyDetail);

            model.Should().NotBeNull();
            model.EmployerWebsite.Should().Be(vacancyDetail.EmployerWebsite);
            model.IsWellFormedEmployerWebsiteUrl.Should().Be(false);
        }

        [Test]
        public void ShouldReturnTheVacancyUrlWithHttpIfItAlreadyHasIt()
        {
            var vacancyDetail = new VacancyDetail
            {
                VacancyUrl = "http://wwww.someweb.com"
            };

            var model = _mapper.Map<VacancyDetail, VacancyDetailViewModel>(vacancyDetail);

            model.Should().NotBeNull();
            model.VacancyUrl.Should().StartWith("http://");
            model.IsWellFormedVacancyUrl.Should().Be(true);
        }

        [Test]
        public void ShouldReturnTheVacancyUrlWithHttpIfItDoesntHaveIt()
        {
            var vacancyDetail = new VacancyDetail
            {
                VacancyUrl = "wwww.someweb.com"
            };

            var model = _mapper.Map<VacancyDetail, VacancyDetailViewModel>(vacancyDetail);

            model.Should().NotBeNull();
            model.VacancyUrl.Should().StartWith("http://");
            model.IsWellFormedVacancyUrl.Should().Be(true);
        }

        [Test]
        public void ShouldReturnTheVacancyUrlWithHttpsIfItStartsWithIt()
        {
            var vacancyDetail = new VacancyDetail
            {
                VacancyUrl = "https://wwww.someweb.com"
            };

            var model = _mapper.Map<VacancyDetail, VacancyDetailViewModel>(vacancyDetail);

            model.Should().NotBeNull();
            model.VacancyUrl.Should().StartWith("https://");
            model.IsWellFormedVacancyUrl.Should().Be(true);
        }

        [Test]
        public void ShouldReturnRawStringIfVacancyUrlIsNotWellFormed()
        {
            var vacancyDetail = new VacancyDetail
            {
                VacancyUrl = "www.somedomain.co.uk / www.anotherdomain.co.uk"
            };

            var model = _mapper.Map<VacancyDetail, VacancyDetailViewModel>(vacancyDetail);

            model.Should().NotBeNull();
            model.VacancyUrl.Should().Be(vacancyDetail.VacancyUrl);
            model.IsWellFormedVacancyUrl.Should().Be(false);
        }
    }
}
