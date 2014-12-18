namespace SFA.Apprenticeships.Web.Candidate.Mappers.Resolvers
{
    using System;
    using AutoMapper;
    using Domain.Entities.Vacancies;
    using Domain.Entities.Vacancies.Apprenticeships;

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

        public class WageResolver : ValueResolver<VacancyDetail, string>
        {
            protected override string ResolveCore(VacancyDetail vacancyDetail)
            {
                return vacancyDetail.WageType == WageType.Text
                    ? vacancyDetail.WageDescription
                    : string.Format("£{0:N2}", vacancyDetail.Wage);
            }
        }

        public class UrlResolver : ValueResolver<string, string>
        {
            protected override string ResolveCore(string source)
            {
                return UrlValidator.IsValidUrl(source) ? new UriBuilder(source).Uri.ToString() : source;
            }
        }

        public class IsWellFormedUrlResolver : ValueResolver<string, bool>
        {
            protected override bool ResolveCore(string source)
            {
                return UrlValidator.IsValidUrl(source);
            }
        }

        private static class UrlValidator
        {
            public static bool IsValidUrl(string url)
            {
                if (string.IsNullOrWhiteSpace(url))
                {
                    return false;
                }

                try
                {
                    // Attempting to build the URL will throw an exception if it is invalid.
                    // ReSharper disable once UnusedVariable
                    var unused = new UriBuilder(url);

                    return true;
                }
                catch (UriFormatException)
                {
                    return false;
                }
            }
        }
    }
}