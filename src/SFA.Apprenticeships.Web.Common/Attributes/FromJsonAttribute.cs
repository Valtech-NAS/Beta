using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SFA.Apprenticeships.Web.Common.Attributes
{
    public class FromJsonAttribute : CustomModelBinderAttribute
    {
        private static readonly JavaScriptSerializer Serializer = new JavaScriptSerializer();

        public override IModelBinder GetBinder()
        {
            return new JsonModelBinder();
        }

        #region Nested type: JsonModelBinder

        private class JsonModelBinder : IModelBinder
        {
            #region IModelBinder Members

            public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {
                var stringified = controllerContext.HttpContext.Request[bindingContext.ModelName];
                return string.IsNullOrEmpty(stringified)
                    ? null
                    : Serializer.Deserialize(stringified, bindingContext.ModelType);
            }

            #endregion
        }

        #endregion
    }
}
