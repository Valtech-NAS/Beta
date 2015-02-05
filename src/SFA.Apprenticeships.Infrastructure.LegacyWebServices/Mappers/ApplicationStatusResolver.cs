namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers
{
    using System;
    using AutoMapper;
    using Domain.Entities.Applications;

    public class ApplicationStatusResolver : ValueResolver<string, ApplicationStatuses>
    {
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
                    throw new ArgumentOutOfRangeException("source", "Unknown Application Status received from NAS Gateway Service: " + source);
            }
        }
    }
}
