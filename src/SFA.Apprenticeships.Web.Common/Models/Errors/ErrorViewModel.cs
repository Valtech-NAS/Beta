using System.Web.Mvc;

namespace SFA.Apprenticeships.Web.Common.Models.Errors
{
    public class ErrorViewModel
    {
        public string PageTitle { get; set; }
        public string WarningMessage { get; set; }
        public bool ShowCloseButton { get; set; }
        public HandleErrorInfo HandleErrorInfo { get; set; }
        public bool ShowExceptionInDebug { get; set; }
    }
}
