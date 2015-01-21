namespace SFA.Apprenticeships.Web.Common.ActionResults
{
    using System.Web.Mvc;

    public abstract class HttpCustomStatusCodeResult : ActionResult
    {
        private readonly int _statusCode;
        private readonly int _subStatusCode;

        protected HttpCustomStatusCodeResult(int statusCode, int subStatusCode = 0)
        {
            _statusCode = statusCode;
            _subStatusCode = subStatusCode;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.StatusCode = _statusCode;
            context.HttpContext.Response.SubStatusCode = _subStatusCode;
        }
    }
}
