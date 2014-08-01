namespace SFA.Apprenticeships.Application.Interfaces.ReferenceData
{
    using System.Collections.Generic;
    using Domain.Entities.ReferenceData;

    public interface IReferenceDataService //todo: may be redundant TBC
    {
        IEnumerable<ReferenceDataItem> GetReferenceData(string type);
    }
}
