using System.Collections.Generic;
using SFA.Apprenticeships.Services.Models.ReferenceData;
using SFA.Apprenticeships.Services.ReferenceData.Models;

namespace SFA.Apprenticeships.Services.ReferenceData.Abstract
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
