namespace SFA.Apprenticeships.Web.Common.Attributes
{
    using System;
    using System.Reflection;
    using System.Web.Mvc;
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MultipleFormActionsButtonAttribute : ActionNameSelectorAttribute
    {
        public string Name { get; set; }
        public string Argument { get; set; }

        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            var keyValue = string.Format("{0}:{1}", Name, Argument);
            var value = controllerContext.Controller.ValueProvider.GetValue(keyValue);

            if (value == null)
            {
                return false;
            }

            controllerContext.Controller.ControllerContext.RouteData.Values[Name] = Argument;

            return true;
        }
    }
}