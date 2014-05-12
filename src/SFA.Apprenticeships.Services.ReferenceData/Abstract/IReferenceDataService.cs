using System.Collections.Generic;
using SFA.Apprenticeships.Services.Models.ReferenceDataModels;

namespace SFA.Apprenticeships.Services.ReferenceData.Abstract
{
    public interface IReferenceDataService
    {
        IList<Frameworks> GetApprenticeshipFrameworks();
        IList<County> GetCounties();
    }
}
