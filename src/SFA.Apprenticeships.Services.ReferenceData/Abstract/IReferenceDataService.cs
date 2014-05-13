using System.Collections.Generic;
using SFA.Apprenticeships.Services.Models.ReferenceDataModels;

namespace SFA.Apprenticeships.Services.ReferenceData.Abstract
{
    public interface IReferenceDataService
    {
        IList<Framework> GetApprenticeshipFrameworks();
        IList<County> GetCounties();
        IList<ErrorCode> GetErrorCodes();
        IList<LocalAuthority> GetLocalAuthorities();
        IList<Region> GetRegions();
    }
}
