namespace SFA.Apprenticeships.Web.Candidate
{
    using System.Linq;
    using System.Web.Mvc;
    using Common.IoC;
    using StructureMap;

    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters, IContainer container)
        {
            var oldProvider = FilterProviders.Providers.Single(f => f is FilterAttributeFilterProvider);
            FilterProviders.Providers.Remove(oldProvider);
            FilterProviders.Providers.Add(new DependencyResolverFilterProvider(container));
        }
    }
}
