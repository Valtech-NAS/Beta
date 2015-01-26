namespace SFA.Apprenticeships.Application.Interfaces.ReferenceData
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.ReferenceData;

    public interface IReferenceDataService
    {
        IEnumerable<ReferenceDataItem> GetReferenceData(string type);
    }
}
