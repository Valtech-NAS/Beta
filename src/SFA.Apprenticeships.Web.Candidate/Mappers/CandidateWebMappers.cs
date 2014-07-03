namespace SFA.Apprenticeships.Web.Candidate.Mappers
{
    using System.Collections.Generic;
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancies;
    using AutoMapper;
    using Domain.Entities.Locations;
    using Domain.Entities.Vacancies;
    using Infrastructure.Common.Mappers;
    using ViewModels.Locations;
    using ViewModels.VacancySearch;

    public class CandidateWebMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<SearchResults<VacancySummaryResponse>, VacancySearchResponseViewModel>()
                .ConvertUsing<SearchResultsConverter>();

            Mapper.CreateMap<VacancySearchViewModel, Location>()
                .ConvertUsing<LocationResolver>();

            Mapper.CreateMap<Location, LocationViewModel>()
                .ForMember(d => d.Latitude, opt => opt.MapFrom(s => s.GeoPoint.Latitude))
                .ForMember(d => d.Longitude, opt => opt.MapFrom(s => s.GeoPoint.Longitude));

            Mapper.CreateMap<VacancyDetail, VacancyDetailViewModel>();
            Mapper.CreateMap<VacancySummaryResponse, VacancySummaryViewModel>();
            Mapper.CreateMap<Address, AddressViewModel>();
            Mapper.CreateMap<GeoPoint, GeoPointViewModel>();
        }

        protected class LocationResolver : ITypeConverter<VacancySearchViewModel, Location>
        {
            public Location Convert(ResolutionContext context)
            {
                var viewModel = (VacancySearchViewModel) context.SourceValue;
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

        protected class SearchResultsConverter : ITypeConverter<SearchResults<VacancySummaryResponse>, VacancySearchResponseViewModel>
        {
            public VacancySearchResponseViewModel Convert(ResolutionContext context)
            {
                var source = (SearchResults<VacancySummaryResponse>) context.SourceValue;

                var viewModel = new VacancySearchResponseViewModel
                {
                    TotalHits = source.Total,
                    Vacancies = context.Engine.Map<IEnumerable<VacancySummaryResponse>, IEnumerable<VacancySummaryViewModel>>(source.Results)
                };

                return viewModel;
            }
        }
    }
}