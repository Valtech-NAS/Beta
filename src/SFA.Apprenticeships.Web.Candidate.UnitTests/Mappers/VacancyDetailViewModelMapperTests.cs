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
            ObjectFactory.Initialize(x => x.AddRegistry<CandidateWebRegistry>());
        }

        [SetUp]
        public void Setup()
        {
            _mapper = ObjectFactory.GetInstance<IMapper>();
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
            model.EmployerName.ShouldBeEquivalentTo(vacancyDetail.EmployerName);
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
            model.EmployerName.ShouldBeEquivalentTo(vacancyDetail.AnonymousEmployerName);
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

            model.Should().NotBeNull();
            model.Wage.ShouldAllBeEquivalentTo("£101.19");
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
            model.Wage.ShouldAllBeEquivalentTo(vacancyDetail.WageDescription);
        }
    }
}
