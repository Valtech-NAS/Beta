namespace SFA.Apprenticeships.Web.Candidate.Mappers.Resolvers
{
    using System;
    using AutoMapper;
    using Domain.Entities.Vacancies;

    public class VacancyDetailViewModelResolvers
    {
        public class EmployerNameResolver : ValueResolver<VacancyDetail, string>
        {
            protected override string ResolveCore(VacancyDetail vacancyDetail)
            {
                return vacancyDetail.IsEmployerAnonymous
                    ? vacancyDetail.AnonymousEmployerName
                    : vacancyDetail.EmployerName;
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
                try
                {
                    return string.IsNullOrEmpty(s) ? string.Empty : new UriBuilder(s).Uri.ToString();
                }
                catch
                {
                    return s;
                }
            }
        }

        public class WageResolver : ValueResolver<VacancyDetail, string>
        {
            protected override string ResolveCore(VacancyDetail vacancyDetail)
            {
                return vacancyDetail.WageType == WageType.Text
                    ? vacancyDetail.WageDescription
                    : string.Format("£{0:N2}", vacancyDetail.Wage);
            }
        }

        public class EmployerWebsiteResolver : ValueResolver<string, string>
        {
            protected override string ResolveCore(string source)
            {
                try
                {
                    return string.IsNullOrEmpty(source) ? string.Empty : new UriBuilder(source).Uri.ToString();
                }
                catch
                {
                    return source;
                }
            }
        }
    }
}
