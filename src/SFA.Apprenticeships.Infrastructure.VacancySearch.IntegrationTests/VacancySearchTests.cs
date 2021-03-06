﻿namespace SFA.Apprenticeships.Infrastructure.VacancySearch.IntegrationTests
{
    using System.Linq;
    using Application.Interfaces.Logging;
    using Application.Interfaces.Vacancies;
    using Configuration;
    using Domain.Entities.Locations;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Mapping;
    using Elastic.Common.Configuration;
    using Elastic.Common.IoC;
    using FluentAssertions;
    using IoC;
    using Moq;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class VacancySearchTests
    {
        private const string RetailAndCommercialEnterprise = "HBY"; //"Retail and Commercial Enterprise";
        private IElasticsearchClientFactory _elasticsearchClientFactory;
        private IMapper _mapper;
        private Mock<ILogService> _logger = new Mock<ILogService>();

        [SetUp]
        public void FixtureSetUp()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<ElasticsearchCommonRegistry>();
                x.AddRegistry<VacancySearchRegistry>();
            });

            _elasticsearchClientFactory = container.GetInstance<IElasticsearchClientFactory>();
            _mapper = container.GetInstance<IMapper>();
        }

        [Test, Category("Integration")]
        public void ShouldReturnFrameworksCount()
        {
            var vacancySearchProvider = new ApprenticeshipsSearchProvider(_elasticsearchClientFactory, _mapper,
                SearchConfiguration.Instance, _logger.Object);

            var vacancies = vacancySearchProvider.FindVacancies(GetCommonSearchParameters());

            vacancies.AggregationResults.Should().HaveCount(c => c > 0);
        }

        [Test, Category("Integration")]
        public void ShouldSearchBySector()
        {

            var vacancySearchProvider = new ApprenticeshipsSearchProvider(_elasticsearchClientFactory, _mapper,
                SearchConfiguration.Instance, _logger.Object);

            var searchParameters = GetCommonSearchParameters();
            searchParameters.Sector = RetailAndCommercialEnterprise;

            var vacancies = vacancySearchProvider.FindVacancies(searchParameters);

            vacancies.AggregationResults.Should().HaveCount(c => c > 0);
        }

        [Test, Category("Integration")]
        public void ShouldSearchBySectorAndFramework()
        {
            var vacancySearchProvider = new ApprenticeshipsSearchProvider(_elasticsearchClientFactory, _mapper,
                SearchConfiguration.Instance, _logger.Object);

            var searchParameters = GetCommonSearchParameters();
            searchParameters.Sector = RetailAndCommercialEnterprise;
            searchParameters.Frameworks = new[] {"582"};

            var vacancies = vacancySearchProvider.FindVacancies(searchParameters);

            vacancies.AggregationResults.Should().HaveCount(n => n > 1);
        }

        [Test, Category("Integration")]
        public void ShouldSearchAllEngland()
        {
            //TODO: this test could be too fragile
            var vacancySearchProvider = new ApprenticeshipsSearchProvider(_elasticsearchClientFactory, _mapper,
                SearchConfiguration.Instance, _logger.Object);

            var searchParameters = GetEnglandSearchParameters("it");

            var vacancies = vacancySearchProvider.FindVacancies(searchParameters);

            vacancies.Results.Where(r => r.Distance > 40).Should().NotBeEmpty();
        }

        [Test, Category("Integration")]
        public void ShouldSortByPostedDate()
        {
            var vacancySearchProvider = new ApprenticeshipsSearchProvider(_elasticsearchClientFactory, _mapper,
                SearchConfiguration.Instance, _logger.Object);

            var searchParameters = GetPostedDateSearchParameters();

            var vacancies = vacancySearchProvider.FindVacancies(searchParameters);

            vacancies.Results.Should().BeInDescendingOrder(r => r.Id);
        }

        private static ApprenticeshipSearchParameters GetCommonSearchParameters()
        {
            return new ApprenticeshipSearchParameters
            {
                ApprenticeshipLevel = "Intermediate",
                Location = new Location
                {
                    Name = "London",
                    GeoPoint = new GeoPoint
                    {
                        Latitude = 51.5072,
                        Longitude = 0.1275
                    }
                },
                PageNumber = 1,
                PageSize = 5,
                SearchRadius = 50,
                SortType = VacancySearchSortType.ClosingDate,
                VacancyLocationType = ApprenticeshipLocationType.NonNational
            };
        }

        private static ApprenticeshipSearchParameters GetEnglandSearchParameters(string keyword)
        {
            var searchParameters = GetCommonSearchParameters();
            searchParameters.SearchRadius = 0;
            searchParameters.Keywords = keyword;
            searchParameters.ApprenticeshipLevel = string.Empty;
            searchParameters.PageSize = 50;
            searchParameters.SortType = VacancySearchSortType.Relevancy;

            return searchParameters;
        }

        private static ApprenticeshipSearchParameters GetPostedDateSearchParameters()
        {
            var searchParameters = GetCommonSearchParameters();
            searchParameters.SortType = VacancySearchSortType.RecentlyAdded;

            return searchParameters;
        }
    }
}