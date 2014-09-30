namespace SFA.Apprenticeships.Web.Common.Binders
{
    using System.ComponentModel;
    using System.Web.Mvc;

    public class TrimModelBinder : DefaultModelBinder
    {
        protected override void SetProperty(
            ControllerContext controllerContext,
            ModelBindingContext bindingContext,
            PropertyDescriptor propertyDescriptor, object value){

            if (propertyDescriptor.PropertyType == typeof(string))
            {
                var val = (string)value;
                if (!string.IsNullOrEmpty(val))
                {
                    val = val.Trim();
                }

                value = val;
            }

            base.SetProperty(controllerContext, bindingContext, propertyDescriptor, value);
        }
    }
}
