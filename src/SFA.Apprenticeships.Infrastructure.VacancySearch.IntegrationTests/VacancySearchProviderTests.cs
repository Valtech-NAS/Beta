namespace SFA.Apprenticeships.Infrastructure.VacancySearch.IntegrationTests
{
    using NUnit.Framework;
    using Domain.Entities.Location;
    using Elastic.Common.IoC;
    using StructureMap;

    public class VacancySearchProviderTests
    {
        [SetUp]
        public void SetUp()
        {
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<ElasticsearchCommonRegistry>();
            });
        }

        //TODO: populate index with test data and test search results are correct
        [Test]
        public void ShouldReturnPopulatedVacancySummaries()
        {
            var vsp = ObjectFactory.GetInstance<VacancySearchProvider>();
            var location = new Location()
            {
                Name = "SFA",
                GeoPoint = new GeoPoint()
                {
                    Latitude = 52.4009991288043,
                    Longitude = -1.50812239495425
                }
            };

            //var vacs = vsp.FindVacancies(location, 10);
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
