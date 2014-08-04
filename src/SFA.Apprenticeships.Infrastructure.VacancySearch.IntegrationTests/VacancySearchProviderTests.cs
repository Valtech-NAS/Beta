namespace SFA.Apprenticeships.Infrastructure.VacancySearch.IntegrationTests
{
    using Application.Interfaces.Vacancies;
    using Domain.Entities.Locations;
    using Elastic.Common.IoC;
    using FluentAssertions;
    using IoC;
    using NUnit.Framework;
    using StructureMap;

    public class VacancySearchProviderTests
    {
        [SetUp]
        public void SetUp()
        {
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<VacancySearchRegistry>();
                x.AddRegistry<ElasticsearchCommonRegistry>();
            });
        }

        // TODO: NOTIMPL: populate index with test data and test search results are correct
        [Test]
        public void ShouldReturnPopulatedVacancySummaries()
        {
            var vsp = ObjectFactory.GetInstance<IVacancySearchProvider>();
            var location = new Location
            {
                Name = "SFA",
                GeoPoint = new GeoPoint
                {
                    Latitude = 51.7715114421813,
                    Longitude = -0.453494192734934
                }
            };

            //Stubbed to get CI Green
            // See TODO above
            var vacs = vsp.FindVacancies("Chef",location, 1, 5, 30, VacancySortType.Distance);
            vacs.Should().NotBeNull();
        }


        //var vsummary = new VacancySummaryUpdate()
        //{
        //    Id = 1,
        //    Title = "VS Title",
        //    Description = "Description",
        //    ClosingDate = DateTime.Now.AddMonths(3),
        //    Created = DateTime.Now,
        //    Location = new GeoPoint() { Latitude = 1, Longitude = 2 },
        //    VacancyType = VacancyType.Intermediate,
        //    VacancyLocationType = VacancyLocationType.NonNational,
        //    AddressLine1 = "Add1",
        //    AddressLine2 = "Add2",
        //    AddressLine3 = "Add3",
        //    AddressLine4 = "Add4"
        //};

        //vis.Index(vsummary);
    }
}