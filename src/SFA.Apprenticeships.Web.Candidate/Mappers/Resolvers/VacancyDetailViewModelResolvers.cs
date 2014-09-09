namespace SFA.Apprenticeships.Web.Candidate.Mappers.Resolvers
{
    using AutoMapper;
    using Domain.Entities.Vacancies;

    public class VacancyDetailViewModelResolvers
    {
        public class EmployerNameResolver : ValueResolver<VacancyDetail, string>
        {
            protected override string ResolveCore(VacancyDetail vacancyDetail)
            {
                return vacancyDetail.IsEmployerAnonymous ?
                    vacancyDetail.AnonymousEmployerName :
                    vacancyDetail.EmployerName;
            }
        }

        public class WageResolver : ValueResolver<VacancyDetail, string>
        {
            protected override string ResolveCore(VacancyDetail vacancyDetail)
            {
                return vacancyDetail.WageType == WageType.Text ?
                    vacancyDetail.WageDescription :
                    string.Format("£{0:N2}", vacancyDetail.Wage);
            }
        }

        // TODO: US490: AG: remove when NAS Gateway service becomes available.
        public class RealityCheckResolver : ValueResolver<VacancyDetail, string>
        {
            protected override string ResolveCore(VacancyDetail vacancyDetail)
            {
                return vacancyDetail.Id % 2 == 0
                    ? null
                    : "You must be capable of standing during all working hours. A degree of physical work will be required.";
            }
        }
    }
}