namespace SFA.Apprenticeships.Web.Common.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CuttingEdge.Conditions;
    using Models.Common;
    using Application.Interfaces.ReferenceData;

    public class WebReferenceDataProvider : IWebReferenceDataProvider
    {
        private readonly IReferenceDataService _service;

        public WebReferenceDataProvider(IReferenceDataService service)
        {
            Condition.Requires("service");

            _service = service;
        }

        public IEnumerable<WebReferenceDataViewModel> Get(WebReferenceDataTypes type)
        {
            var result = new List<WebReferenceDataViewModel>();
            var serviceData = _service.GetReferenceData(type.ToString());

            result.AddRange(
                serviceData.Select(
                    item =>
                        new WebReferenceDataViewModel
                        {
                            Id = item.Id,
                            Description = item.Description
                        }));

            return result;
        }
    }
}
