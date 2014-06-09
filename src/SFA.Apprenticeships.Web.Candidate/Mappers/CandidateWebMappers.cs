namespace SFA.Apprenticeships.Web.Candidate.Mappers
{
    using SFA.Apprenticeships.Application.Interfaces.Search;
    using SFA.Apprenticeships.Domain.Entities.Location;
    using SFA.Apprenticeships.Domain.Entities.Vacancy;
    using SFA.Apprenticeships.Infrastructure.Common.Mappers;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch;

    public class CandidateWebMappers : MapperEngine
    {
        public override void Initialize()
        {
            Mapper.CreateMap<SearchResults<VacancySummary>, VacancySearchResponseViewModel>()
                .ForMember(x => x.Vacancies, opt => opt.MapFrom(src => src.Results));

            Mapper.CreateMap<LocationViewModel, Location>();
        }
    }

}
