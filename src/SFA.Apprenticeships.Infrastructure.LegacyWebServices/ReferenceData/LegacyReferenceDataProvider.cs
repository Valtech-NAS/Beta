namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.ReferenceData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CuttingEdge.Conditions;
    using Application.Interfaces.ReferenceData;
    using Domain.Entities.ReferenceData;
    using Common.Wcf;
    using Configuration;
    using ReferenceDataProxy;

    public class LegacyReferenceDataProvider : IReferenceDataProvider
    {
        private readonly IWcfService<IReferenceData> _service;
        private readonly ILegacyServicesConfiguration _legacyServicesConfiguration;

        public LegacyReferenceDataProvider(ILegacyServicesConfiguration legacyServicesConfiguration, IWcfService<IReferenceData> service)
        {
            _legacyServicesConfiguration = legacyServicesConfiguration;
            _service = service;
        }

        public IEnumerable<ReferenceDataItem> GetReferenceData(string type)
        {
            Condition.Requires(type, "type").IsNotNullOrWhiteSpace();

            switch (type.ToLower())
            {
                case "counties": 
                    return GetCounties();
                default:
                    throw new ArgumentOutOfRangeException("type", "Unrecognised reference data name '" + type + "'");
            }
        }

        #region Helpers

        IEnumerable<ReferenceDataItem> GetCounties()
        {
            var msgId = Guid.NewGuid();
            var request = new GetCountiesRequest
            {
                ExternalSystemId = _legacyServicesConfiguration.SystemId,
                PublicKey = _legacyServicesConfiguration.PublicKey,
                MessageId = msgId,
            };

            var rs = default(GetCountiesResponse);
            _service.Use(client => { rs = client.GetCounties(request); });

            if (rs != null)
            {
                return rs.Counties.Select(
                    item => new ReferenceDataItem
                    {
                        Id = item.CodeName,
                        Description = item.FullName
                    });
            }

            return default(IEnumerable<ReferenceDataItem>);
        }

        #endregion
    }
}
