namespace SFA.Apprenticeships.Application.ReferenceData
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.ReferenceData;
    using Interfaces.ReferenceData;

    public class ReferenceDataService : IReferenceDataService
    {
        private readonly IReferenceDataProvider _referenceDataProvider;

        public ReferenceDataService(IReferenceDataProvider referenceDataProvider)
        {
            _referenceDataProvider = referenceDataProvider;
        }

        public IEnumerable<Category> GetCategories()
        {
            return _referenceDataProvider.GetCategories();
        }
    }
}
