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
                case "Sent": //todo: confirm value from w/s - awaiting response from John @ Capgemini
                    return ApplicationStatuses.Submitted;
                case "InProgress": //todo: confirm value from w/s - awaiting response from John @ Capgemini
                    return ApplicationStatuses.InProgress;
                case "Successful": //todo: confirm value from w/s - awaiting response from John @ Capgemini
                    return ApplicationStatuses.Successful;
                case "Unsuccessful": //todo: confirm value from w/s - awaiting response from John @ Capgemini
                    return ApplicationStatuses.Unsuccessful;
                case "PastApplication": //todo: confirm value from w/s - awaiting response from John @ Capgemini
                    return ApplicationStatuses.Expired;
                default:
                    return ApplicationStatuses.Unknown;
            }
        }
    }
}
