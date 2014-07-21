namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.ReferenceData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CuttingEdge.Conditions;
    using Application.Interfaces.ReferenceData;
    using Configuration;
    using Domain.Entities.ReferenceData;
    using NLog;
    using ReferenceDataProxy;
    using Wcf;

    public class LegacyReferenceDataProvider : IReferenceDataProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
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

            Logger.Info("Calling legacy reference data for MessageId={0}", msgId);

            var rs = default(GetCountiesResponse);
            _service.Use(client => { rs = client.GetCounties(request); });

            if (rs != null)
            {
                Logger.Info("Returning legacy reference data response - GetCountiesResponse for MessageId={0}", msgId);

                return rs.Counties.Select(
                    item => new ReferenceDataItem
                    {
                        Id = item.CodeName,
                        Description = item.FullName
                    });
            }

            Logger.Info("No GetCountiesResponse from Legacy reference data provider");

            return default(IEnumerable<ReferenceDataItem>);
        }

        #endregion
    }
}
