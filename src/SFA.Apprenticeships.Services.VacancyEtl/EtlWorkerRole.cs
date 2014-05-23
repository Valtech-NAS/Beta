namespace SFA.Apprenticeships.Services.VacancyEtl
{
    using System.Net;
    using System.Threading;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using SFA.Apprenticeships.Common.Entities.Vacancy;
    using SFA.Apprenticeships.Common.Interfaces.Elasticsearch;
    using SFA.Apprenticeships.Common.IoC;
    using System;
    using System.ServiceModel;
    using SFA.Apprenticeships.Common.Messaging.Interfaces;
    using SFA.Apprenticeships.Services.Legacy.Vacancy.Abstract;
    using SFA.Apprenticeships.Services.VacancyEtl.Consumers;
    using SFA.Apprenticeships.Services.VacancyEtl.Load;
    using SFA.Apprenticeships.Services.VacancyEtl.Queue;
    using StructureMap;

    public class EtlWorkerRole : RoleEntryPoint
    {
        private VacancySchedulerConsumer _vacancySchedulerConsumer;
        private readonly static NLog.Logger Logger = NLog.LogManager.GetLogger("VacanyImporter");

        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Logger.Trace("EtlWorkerRole entry point called");

            IoC.Initialize();
            Logger.Trace("IoC initialized");

            ElasticsearchLoad<VacancySummary>.Setup(ObjectFactory.GetInstance<IElasticsearchService>());
            Logger.Trace("Elasticsearch setup complete");

            var bus = RabbitQueue.Setup();
            Logger.Trace("RabbitMq setup complete");

            _vacancySchedulerConsumer = new VacancySchedulerConsumer(
                                                bus,
                                                ObjectFactory.GetInstance<IAzureCloudClient>(),
                                                ObjectFactory.GetInstance<IVacancySummaryService>());
            Logger.Trace("VacancySchedulerConsumer setup complete");

            while (true)
            {
                try
                {
                    var task = _vacancySchedulerConsumer.CheckScheduleQueue();
                    task.Wait();
                }
                catch (CommunicationException ce)
                {
                    Logger.Warn("CommunicationException returned from legacy web services, error: {0}", ce.Message);
                }
                catch (TimeoutException te)
                {
                    Logger.Warn("TimeoutException returned from legacy web services, error: {0}", te.Message);
                }
                catch (Exception ex)
                {
                    Logger.Error("Unknown Exception returned from VacancySchedulerConsumer", ex);
                }
                finally
                {
                    Thread.Sleep(5000);
                }
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