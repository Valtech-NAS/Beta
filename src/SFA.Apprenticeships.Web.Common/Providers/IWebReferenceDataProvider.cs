namespace SFA.Apprenticeships.Web.Common.Providers
{
    using System.Collections.Generic;
    using Models.Common;

    public interface IWebReferenceDataProvider
    {
        IEnumerable<WebReferenceDataViewModel> Get(WebReferenceDataTypes type);
    }
}
