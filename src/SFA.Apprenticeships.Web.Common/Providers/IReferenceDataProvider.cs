using System.Collections.Generic;
using SFA.Apprenticeships.Web.Common.Models.Common;

namespace SFA.Apprenticeships.Web.Common.Providers
{
    /// <summary>
    /// Integration point and type mapper.
    /// Provides reference data for populating user inputs
    /// </summary>
    public interface IReferenceDataProvider
    {
        IEnumerable<ReferenceDataViewModel> Get(ReferenceDataType type);
    }
}
