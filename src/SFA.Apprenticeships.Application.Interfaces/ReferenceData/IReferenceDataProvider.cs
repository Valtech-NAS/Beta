namespace SFA.Apprenticeships.Application.Interfaces.ReferenceData
{
    using System.Collections.Generic;
    using Domain.Entities.ReferenceData;

    public interface IReferenceDataProvider
    {
        IEnumerable<ReferenceDataItem> GetReferenceData(string type);
    }
}
