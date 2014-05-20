namespace SFA.Apprenticeships.Common.Configuration.LegacyServices
{
    using System;

    public interface ILegacyServicesConfiguration
    {
        /// <summary>
        /// 	Gets or sets the legacy services service id.
        /// </summary>
        Guid SystemId { get; set; }


        /// <summary>
        /// 	Gets or sets the legacy services public key
        /// </summary>
        string PublicKey { get; set; }
    }
}
