namespace SFA.Apprenticeships.Application.Interfaces
{
    using System.Collections.Generic;
    using Domain.Entities;
    using Domain.Enums;

    public interface IReferenceDataService
    {
        IEnumerable<ReferenceData> Get(ReferenceDataTypes type);
    }

   
}