namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Configuration
{
    using System;

    //todo: replace once new NAS Gateway operations available
    public interface ILegacyServicesConfiguration
    {
        Guid SystemId { get; set; }
        string PublicKey { get; set; }
    }
}
