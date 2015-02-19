namespace SFA.Apprenticeships.Application.ApplicationUpdate.Extensions
{
    using System;
    using Entities;

    internal static class ApplicationStatusSummaryExtensions
    {
        public static bool IsLegacySystemUpdate(this ApplicationStatusSummary applicationStatusSummary)
        {
            return applicationStatusSummary.ApplicationId == Guid.Empty;
        }
    }
}