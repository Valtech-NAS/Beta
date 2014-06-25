namespace SFA.Apprenticeships.Web.Common.Framework
{
    using System;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;

    public class JsonpResult : JsonResult
    {
        readonly object _data;

        public JsonpResult()
        {
        }

        public JsonpResult(object data, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.AllowGet)
        {
            JsonRequestBehavior = jsonRequestBehavior;
            _data = data;
        }

        public override void ExecuteResult(ControllerContext controllerContext)
        {
            if (controllerContext != null)
            {
                var response = controllerContext.HttpContext.Response;
                var request = controllerContext.HttpContext.Request;

                string callbackfunction = request["callback"];
                if (string.IsNullOrEmpty(callbackfunction))
                {
                    throw new Exception("Callback function name must be provided in the request!");
                }
                response.ContentType = "application/x-javascript";
                if (_data != null)
                {
                    var serializer = new JavaScriptSerializer();
                    response.Write(string.Format("{0}({1});", callbackfunction, serializer.Serialize(_data)));
                }
            }
        }
    }
}