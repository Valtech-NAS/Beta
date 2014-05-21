using System.Collections.Generic;
using SFA.Apprenticeships.Common.Interfaces.Enums.ReferenceDataService;

namespace SFA.Apprenticeships.Common.Interfaces.ReferenceData
{
    public interface IReferenceDataService
    {
        IEnumerable<ILegacyReferenceData> GetReferenceData(LegacyReferenceDataType type);
        IEnumerable<ILegacyReferenceData> GetApprenticeshipOccupations();
        IEnumerable<ILegacyReferenceData> GetApprenticeshipFrameworks();
        IEnumerable<ILegacyReferenceData> GetCounties();
        IEnumerable<ILegacyReferenceData> GetErrorCodes();
        IEnumerable<ILegacyReferenceData> GetLocalAuthorities();
        IEnumerable<ILegacyReferenceData> GetRegions();
    }
}
