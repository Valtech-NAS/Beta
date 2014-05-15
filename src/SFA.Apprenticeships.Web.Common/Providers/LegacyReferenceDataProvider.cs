using System;
using System.Collections.Generic;
using System.Linq;
using SFA.Apprenticeships.Services.ReferenceData.Abstract;
using SFA.Apprenticeships.Services.ReferenceData.Models;
using SFA.Apprenticeships.Web.Common.Models.Common;

namespace SFA.Apprenticeships.Web.Common.Providers
{
    public class LegacyReferenceDataProvider : IReferenceDataProvider
    {
        private readonly IReferenceDataService _service;

        public LegacyReferenceDataProvider(IReferenceDataService service)
        {

            if (service == null)
            {
                throw new ArgumentNullException("service");
            }

            _service = service;
        }

        public IEnumerable<ReferenceDataViewModel> Get(LegacyReferenceDataType type)
        {
            var result = new List<ReferenceDataViewModel>();
            var serviceData = _service.GetReferenceData(type);

            // Create the view model
            result.AddRange(
                serviceData
                    .Select(
                        item =>
                            new ReferenceDataViewModel
                            {
                                Id = item.Id,
                                Description = item.Description
                            }));

            return result;
        }
    }
}
