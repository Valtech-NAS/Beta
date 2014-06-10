namespace SFA.Apprenticeships.Web.Candidate.Mappers
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using SFA.Apprenticeships.Application.Interfaces.Search;
    using SFA.Apprenticeships.Application.Interfaces.Vacancy;
    using SFA.Apprenticeships.Domain.Entities.Location;
    using SFA.Apprenticeships.Domain.Entities.Vacancy;
    using SFA.Apprenticeships.Infrastructure.Common.Mappers;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch;

    public class CandidateWebMappers : MapperEngine
    {
        public override void Initialize()
        {
            Mapper.CreateMap<SearchResults<VacancySummaryResponse>, VacancySearchResponseViewModel>()
                .ForMember(x => x.Vacancies, opt => opt.MapFrom(src => src.Results));

            Mapper.CreateMap<LocationViewModel, Location>().ConvertUsing<LocationResolver>();

            Mapper.CreateMap<Location, LocationViewModel>()
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.Latitude, opt => opt.MapFrom(s => s.GeoPoint.Latitute))
                .ForMember(d => d.Longitude, opt => opt.MapFrom(s => s.GeoPoint.Longitude));

            Mapper.CreateMap<IEnumerable<Location>, IEnumerable<LocationViewModel>>().ConvertUsing<EnumerableLocationConverter>();
        }

        class EnumerableLocationConverter : ITypeConverter<IEnumerable<Location>, IEnumerable<LocationViewModel>>
        {
            public IEnumerable<LocationViewModel> Convert(ResolutionContext context)
            {
                return
                    from item in (IEnumerable<Location>)context.SourceValue
                    select context.Engine.Map<Location, LocationViewModel>(item);
            }
        }

        class LocationResolver : ITypeConverter<LocationViewModel, Location>
        {
            public Location Convert(ResolutionContext context)
            {
                var viewModel = (LocationViewModel) context.SourceValue;
                var location = new Location
                {
                    Name = "From LocationViewModel",
                    GeoPoint = new GeoPoint { Latitute = viewModel.Latitude, Longitude = viewModel.Longitude }
                };

                return location;
            }
        }
    }

}
