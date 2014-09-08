namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers
{
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
                case "InProgress":
                    return ApplicationStatuses.Submitted;

                case "Successful":
                    return ApplicationStatuses.Successful;

                case "Unsuccessful":
                    return ApplicationStatuses.Unsuccessful;

                case "PastApplication":
                case "Withdrawn":
                    return ApplicationStatuses.ExpiredOrWithdrawn;

                default:
                    return ApplicationStatuses.Unknown;
            }
        }
    }
}
