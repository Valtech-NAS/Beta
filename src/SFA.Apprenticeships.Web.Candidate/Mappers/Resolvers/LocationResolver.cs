namespace SFA.Apprenticeships.Web.Candidate.Mappers.Resolvers
{
    using AutoMapper;
    using Domain.Entities.Locations;
    using ViewModels.VacancySearch;
    internal class LocationResolver : ITypeConverter<VacancySearchViewModel, Location>
    {
        public Location Convert(ResolutionContext context)
        {
            var viewModel = (VacancySearchViewModel)context.SourceValue;
            var location = new Location
            {
                Name = viewModel.Location,
                GeoPoint =
                    new GeoPoint
                    {
                        Latitude = viewModel.Latitude.GetValueOrDefault(),
                        Longitude = viewModel.Longitude.GetValueOrDefault()
                    }
            };

            return location;
        }
    }
}