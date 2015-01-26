namespace SFA.Apprenticeships.Application.ReferenceData
{
    using System.Collections.Generic;
    using Domain.Entities.ReferenceData;

    public interface IReferenceDataProvider
    {
        IEnumerable<Category> GetCategories();
    }
}
