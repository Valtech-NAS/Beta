namespace SFA.Apprenticeships.Web.Common.Modules
{
    using System;
    using System.Web;

    public class RemoveETagModule : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.PreSendRequestHeaders += OnPreSendRequestHeaders;
        }

        private void OnPreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Headers.Remove("ETag");
        }
    }
}