namespace SFA.Apprenticeships.Application.Interfaces.ReferenceData
{
    using System.Collections.Generic;
    using Domain.Entities.ReferenceData;

    //todo: review this interface - looks like mostly redundant operations for the app service layer
    public interface IReferenceDataService
    {
        IEnumerable<Occupation> GetApprenticeshipOccupations();

        IEnumerable<Framework> GetApprenticeshipFrameworks();

        IEnumerable<County> GetCounties();

        IEnumerable<ErrorCode> GetErrorCodes(); //TODO: Should this be here? Probably not

        IEnumerable<LocalAuthority> GetLocalAuthorities();

        IEnumerable<Region> GetRegions();
    }
}
