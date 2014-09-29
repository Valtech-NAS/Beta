namespace SFA.Apprenticeships.Web.Candidate.Mappers.Resolvers
{
    using System;
    using System.Net;
    using System.Web.Http;
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
 
        public class VacancyUrlResolver : ValueResolver<VacancyDetail, string>
        {
            protected override string ResolveCore(VacancyDetail vacancyDetail)
            {
                return GetUri(vacancyDetail.VacancyUrl);
            }

            private static string GetUri(string s)
            {
                return new UriBuilder(s).Uri.ToString();
            }
        }
    }
}