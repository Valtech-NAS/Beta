﻿namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Configuration
{
    using System;

    public interface ILegacyServicesConfiguration
    {
        Guid SystemId { get; set; }
        string PublicKey { get; set; }
        string BlacklistedCategoryCodes { get; set; }
    }
}