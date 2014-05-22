namespace SFA.Apprenticeships.Web.Common.Providers
{
    using System.Collections.Generic;
    using SFA.Apprenticeships.Web.Common.Models.Common;
    using SFA.Apprenticeships.Common.Interfaces.Enums.ReferenceDataService;

    /// <summary>
    /// Integration point and type mapper.
    /// Provides reference data for populating user inputs
    /// </summary>
    public interface IReferenceDataProvider
    {
        IEnumerable<ReferenceDataViewModel> Get(LegacyReferenceDataType type);
    }
}
