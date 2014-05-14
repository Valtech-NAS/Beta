using System.Collections.Generic;
using SFA.Apprenticeships.Services.Models.ReferenceData;
using SFA.Apprenticeships.Services.ReferenceData.Models;

namespace SFA.Apprenticeships.Services.ReferenceData.Abstract
{
    public interface IReferenceDataService
    {
        IList<ILegacyReferenceData> GetReferenceData(LegacyReferenceDataType type);
        IList<Framework> GetApprenticeshipFrameworks();
        IList<County> GetCounties();
        IList<ErrorCode> GetErrorCodes();
        IList<LocalAuthority> GetLocalAuthorities();
        IList<Region> GetRegions();
    }
}
