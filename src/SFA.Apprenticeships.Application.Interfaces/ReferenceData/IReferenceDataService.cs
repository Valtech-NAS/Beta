namespace SFA.Apprenticeships.Application.Interfaces.ReferenceData
{
    using System.Collections.Generic;
    using SFA.Apprenticeships.Domain.Entities.ReferenceData;

    public interface IReferenceDataService
    {
        IEnumerable<Occupation> GetApprenticeshipOccupations();
        IEnumerable<Framework> GetApprenticeshipFrameworks();
        IEnumerable<County> GetCounties();
        //TODO: Should this be here? Probably not
        IEnumerable<ErrorCode> GetErrorCodes();
        IEnumerable<LocalAuthority> GetLocalAuthorities();
        IEnumerable<Region> GetRegions();
    }
}
