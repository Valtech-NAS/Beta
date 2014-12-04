namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers
{
    using System;
    using AutoMapper;
    using Domain.Entities.Applications;
    using NLog;

    public class ApplicationStatusResolver : ValueResolver<string, ApplicationStatuses>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected override ApplicationStatuses ResolveCore(string source)
        {
            switch (source)
            {
                case "Sent":
                case "New":
                case "In progress":
                case "Unsent":
                    return ApplicationStatuses.Submitted;

                case "Successful":
                    return ApplicationStatuses.Successful;

                case "Unsuccessful":
                    return ApplicationStatuses.Unsuccessful;

                case "PastApplication":
                case "Withdrawn":
                    return ApplicationStatuses.ExpiredOrWithdrawn;

                default:
                    Logger.Error("Unknown Application Status received from NAS Gateway Service, defaulting to Unknown: \"{0}\".", source);
                    return ApplicationStatuses.Unknown;
            }
        }
    }
}
