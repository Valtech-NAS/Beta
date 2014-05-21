using System.Diagnostics;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure.ServiceRuntime;
using SFA.Apprenticeships.Common.Entities.Vacancy;
using SFA.Apprenticeships.Common.IoC;
using SFA.Apprenticeships.Services.WorkerRole.VacancyEtl.Load;
using SFA.Apprenticeships.Services.WorkerRole.VacancyEtl.Queue;

namespace SFA.Apprenticeships.Services.WorkerRole.VacancyEtl
{
    using EasyNetQ;
    using SFA.Apprenticeships.Services.Legacy.Vacancy.Abstract;
    using SFA.Apprenticeships.Services.WorkerRole.VacancyEtl.Consumers;
    using StructureMap;

    public class EtlWorkerRole : RoleEntryPoint
    {
        private VacancySchedulerConsumer _vacancySchedulerConsumer;

        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.TraceInformation("EtlWorkerRole entry point called");

            IoC.Initialize();
            Trace.TraceInformation("IoC initialized");

            ElasticsearchLoad<VacancySummary>.Setup();
            Trace.TraceInformation("Elasticsearch setup complete");

            RabbitQueue.Setup();
            Trace.TraceInformation("RabbitMq setup complete");

            _vacancySchedulerConsumer = new VacancySchedulerConsumer(ObjectFactory.GetInstance<IBus>(), ObjectFactory.GetInstance<IVacancySummaryService>());

            while (true)
            {
                var task = _vacancySchedulerConsumer.CheckScheduleQueue();
                task.Wait();
                Thread.Sleep(5000);
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }

        public override void OnStop()
        {
            // add logic for when stopping ervice
            base.OnStop();
        }
    }
}