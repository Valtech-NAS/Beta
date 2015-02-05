namespace SFA.Apprenticeships.Web.Common.IoC
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using StructureMap;

    public class DependencyResolverFilterProvider : FilterAttributeFilterProvider
    {
        private readonly IContainer _container;

        public DependencyResolverFilterProvider(IContainer container)
        {
            _container = container;
        }

        public override IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var filters = base.GetFilters(controllerContext, actionDescriptor).ToList();

            foreach (var filter in filters)
            {
                //DI via Setter Injection
                _container.BuildUp(filter.Instance);
            }

            return filters;
        }
    }
}