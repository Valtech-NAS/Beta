namespace SFA.Apprenticeships.Web.Candidate
{
    using System.Linq;
    using System.Web.Mvc;
    using Common.IoC;

    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            var oldProvider = FilterProviders.Providers.Single(f => f is FilterAttributeFilterProvider);
            FilterProviders.Providers.Remove(oldProvider);
            FilterProviders.Providers.Add(new DependencyResolverFilterProvider());
        }
    }
}
