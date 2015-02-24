namespace SFA.Apprenticeships.Web.Candidate.Mappers
{
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancies;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Communication;
    using Domain.Entities.Locations;
    using Domain.Entities.Users;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Infrastructure.Common.Mappers;
    using Resolvers;
    using ViewModels.Account;
    using ViewModels.Applications;
    using ViewModels.Home;
    using ViewModels.Locations;
    using ViewModels.Register;
    using ViewModels.VacancySearch;

    public class HomeWebMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<ContactMessageViewModel, ContactMessage>();
        }
    }
}