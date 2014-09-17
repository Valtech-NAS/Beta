namespace SFA.Apprenticeships.Application.ReferenceData
{
    using System.Collections.Generic;
    using Domain.Entities.ReferenceData;
    using Interfaces.ReferenceData;

    public class ReferenceDataService : IReferenceDataService
    {
        private readonly IReferenceDataProvider _service;

        public ReferenceDataService(IReferenceDataProvider service)
        {
            _service = service;
        }

        public IEnumerable<ReferenceDataItem> GetReferenceData(string type)
        {
            return _service.GetReferenceData(type);
        }
    }
}