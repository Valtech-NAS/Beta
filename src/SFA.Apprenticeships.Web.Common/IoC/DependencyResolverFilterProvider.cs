namespace SFA.Apprenticeships.Web.Common.IoC
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using StructureMap;

    public class DependencyResolverFilterProvider : FilterAttributeFilterProvider
    {
        public override IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var filters = base.GetFilters(controllerContext, actionDescriptor).ToList();

            foreach (var filter in filters)
            {
                //DI via Setter Injection
#pragma warning disable 0618
                // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
                ObjectFactory.BuildUp(filter.Instance);
#pragma warning restore 0618
            }

            return filters;
        }
    }
}