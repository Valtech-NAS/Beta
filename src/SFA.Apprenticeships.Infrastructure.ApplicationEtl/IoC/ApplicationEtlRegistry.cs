namespace SFA.Apprenticeships.Infrastructure.ApplicationEtl.IoC
{
    using System;
    using Application.ApplicationUpdate.Entities;
    using Domain.Interfaces.Messaging;
    using Messaging;
    using StructureMap.Configuration.DSL;

    public class ApplicationEtlRegistry : Registry
    {
        public ApplicationEtlRegistry()
        {
            For<IProcessControlQueue<StorageQueueMessage>>().Use<AzureScheduleQueue>();

            //todo: ApplicationEtlRegistry - similar to VacancyEtlRegistry
        }
    }
}
