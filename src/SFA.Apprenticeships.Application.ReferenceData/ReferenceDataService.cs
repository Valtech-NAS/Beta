namespace SFA.Apprenticeships.Application.ReferenceData
{
    using System;
    using System.Collections.Generic;
    using Interfaces.ReferenceData;
    using CuttingEdge.Conditions;
    using Domain.Entities.ReferenceData;

    public class ReferenceDataService : IReferenceDataService
    {
        private readonly IReferenceDataProvider _service;

        public ReferenceDataService(IReferenceDataProvider service)
        {
            Condition.Requires(service, "service").IsNotNull();

            _service = service;
        }

        public IEnumerable<ReferenceDataItem> GetReferenceData(string type)
        {
            return _service.GetReferenceData(type);
        }
    }
}
