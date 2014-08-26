namespace SFA.Apprenticeships.Web.Common.IoC
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using StructureMap;

    public class DependencyResolverFilterProvider : FilterAttributeFilterProvider
    {
        public override IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var filters = base.GetFilters(controllerContext, actionDescriptor);

            foreach (var filter in filters)
            {
                //DI via Setter Injection
                ObjectFactory.BuildUp(filter.Instance);
            }

            return filters;
        }
    }
}