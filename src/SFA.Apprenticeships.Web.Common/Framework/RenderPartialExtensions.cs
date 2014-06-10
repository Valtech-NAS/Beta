namespace SFA.Apprenticeships.Web.Common.Framework
{
    using System;
    using System.IO;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.UI;

    public static class RenderPartialExtensions
    {
        public static string RenderPartialToString(this ControllerContext context, string partialViewName, object model)
        {
            return RenderPartialToStringMethod(context, partialViewName, model);
        }

        public static string RenderPartialToString(ControllerContext context, string partialViewName, ViewDataDictionary viewData, TempDataDictionary tempData)
        {
            return RenderPartialToStringMethod(context, partialViewName, viewData, tempData);
        }

        private static string RenderPartialToStringMethod(ControllerContext context, string partialViewName, ViewDataDictionary viewData, TempDataDictionary tempData)
        {
            var result = ViewEngines.Engines.FindPartialView(context, partialViewName);

            if (result.View != null)
            {
                var sb = new StringBuilder();
                using (var sw = new StringWriter(sb))
                {
                    using (var output = new HtmlTextWriter(sw))
                    {
                        var viewContext = new ViewContext(context, result.View, viewData, tempData, output);
                        result.View.Render(viewContext, output);
                    }
                }

                return sb.ToString();
            }

            return String.Empty;
        }

        private static string RenderPartialToStringMethod(ControllerContext context, string partialViewName, object model)
        {
            return RenderPartialToStringMethod(context, partialViewName, new ViewDataDictionary(model), new TempDataDictionary());
        }
    }
}