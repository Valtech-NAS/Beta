namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.ReferenceData
{
    using System;
    using Application.ReferenceData;
    using LegacyReferenceDataProxy;
    using NLog;
    using Wcf;

    public class ReferenceDataProvider : IReferenceDataProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IWcfService<ReferenceDataClient> _service;

        public ReferenceDataProvider(IWcfService<ReferenceDataClient> service)
        {
            _service = service;
        }
    }
}
